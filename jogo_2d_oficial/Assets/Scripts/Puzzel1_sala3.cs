using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Puzzel1_sala3 : MonoBehaviour
{
    public Button botaoAvancar;
    public TextMeshProUGUI textoFeedback;
    public TMP_InputField inputResposta;

    private string respostaCorreta = "75";

    private PuzzleSaver puzzle;

    public PuzzleTimer timer;

    public AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioClip somErro;
    public AudioClip somAcerto;
    public AudioClip tick;

    public DicasController dicasController;

    public void Start()
    {
        puzzle = PuzzleSaver.Instance;

        if (!puzzle.puzzle1_sala3)
        {
            // Ativa o som de tick
            audioSource2.clip = tick;
            audioSource2.loop = true;
            audioSource2.Play();

            timer.StartPuzzle(); // Timer com tempo restaurado se houver

            inputResposta.text = "";
            botaoAvancar.gameObject.SetActive(false);
            textoFeedback.gameObject.SetActive(false);
        }

        // Se voltou da cena de anúncio
        if (PlayerPrefs.GetInt("WatchedAd", 0) == 1)
        {
            PlayerPrefs.SetInt("WatchedAd", 0);
            dicasController.ShowDica();
        }
    }

    public void Verificar()
    {
        string respostaDoJogador = inputResposta.text.Trim().ToLower();

        if (respostaCorreta == respostaDoJogador)
        {
            audioSource.PlayOneShot(somAcerto);
            textoFeedback.text = "Resposta Correta!";
            textoFeedback.gameObject.SetActive(true);
            botaoAvancar.gameObject.SetActive(true);
            timer.OnResolverClicked();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(inputResposta.text, @"[a-zA-Z]"))
        {
            audioSource.PlayOneShot(somErro);
            textoFeedback.text = "A resposta não deve conter letras.";
            textoFeedback.gameObject.SetActive(true);
        }
        else
        {
            audioSource.PlayOneShot(somErro);
            textoFeedback.text = "Não parece estar certo...";
            textoFeedback.gameObject.SetActive(true);
        }
    }

    public void Voltar()
    {
        audioSource2.Stop();
        SceneManager.LoadScene("Sala III");
    }

    public void Avancar()
    {
        audioSource2.Stop();
        puzzle.puzzle1_sala3 = true;
        PuzzleProgressManager.Instance.MarkSolved("Puzzle1_Sala3");
        SceneManager.LoadScene("Sala III");
    }

    public void Dicas()
    {
        timer.PausarEGuardar(); // Pausa e salva o tempo restante
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("WatchedAd", 0);
        SceneManager.LoadScene("Ads");
    }
}
