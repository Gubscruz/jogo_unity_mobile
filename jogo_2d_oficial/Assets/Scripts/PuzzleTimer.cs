using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class PuzzleTimer : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text timerText;

    [Header("Settings")]
    public float duration = 10f;

    [Header("Dependencies")]
    private HudVidaController hudVidaController;

    private Coroutine timerRoutine;
    private float tempoRestante;
    private bool timerAtivo = false;

    void Awake()
    {
        timerText.gameObject.SetActive(false);

        if (HudVidaController.Instance != null)
        {
            hudVidaController = HudVidaController.Instance;
        }
        else
        {
            Debug.LogWarning("HUD Vida Controller não encontrado!");
        }
    }

    public void StartPuzzle()
    {
        timerText.gameObject.SetActive(true);

        if (PlayerPrefs.HasKey("Timer_Sala3"))
        {
            tempoRestante = PlayerPrefs.GetFloat("Timer_Sala3");
            PlayerPrefs.DeleteKey("Timer_Sala3");
        }
        else
        {
            tempoRestante = duration;
        }

        if (timerRoutine != null) StopCoroutine(timerRoutine);
        timerRoutine = StartCoroutine(Timer());
    }

    public void OnResolverClicked()
    {
        timerText.text = "Acabou o tempo!";
        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
            timerRoutine = null;
        }
    }

    IEnumerator Timer()
    {
        timerAtivo = true;

        while (tempoRestante > 0f)
        {
            tempoRestante -= Time.deltaTime;
            float remaining = Mathf.Max(tempoRestante, 0f);

            int minutes = (int)remaining / 60;
            int seconds = (int)remaining % 60;
            int milliseconds = (int)((remaining - Mathf.Floor(remaining)) * 1000f);

            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            yield return null;
        }

        timerAtivo = false;
        timerRoutine = null;
        timerText.gameObject.SetActive(false);

        hudVidaController.PerderVida();
        SceneManager.LoadScene("Sala III");
    }

    // Chamar antes de ir para o anúncio
    public void PausarEGuardar()
    {
        timerAtivo = false;

        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
            timerRoutine = null;
        }

        PlayerPrefs.SetFloat("Timer_Sala3", tempoRestante);
    }
}
