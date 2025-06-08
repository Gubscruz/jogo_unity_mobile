using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    private CanvasGroup canvasGroup;

    private int currentStep = 0;
    private bool stepCompleted = false;

    private void Start()
    {
        canvasGroup = tutorialText.GetComponent<CanvasGroup>();
        tutorialText.text = "Use o joystick para se movimentar.";
        canvasGroup.alpha = 1f;
        tutorialText.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
    {
        Debug.Log("TAB pressionado");
    }
        // Essas entradas funcionam mesmo com Time.timeScale = 0
        switch (currentStep)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                    Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                    stepCompleted = true;
                break;

            case 1:
                if (Input.GetKeyDown(KeyCode.Tab))
                    stepCompleted = true;
                break;

            case 2:
                if (Input.GetKeyDown(KeyCode.E))
                    stepCompleted = true;
                break;
        }

        if (stepCompleted)
            AdvanceStep();
    }

    void AdvanceStep()
    {
        stepCompleted = false;
        currentStep++;

        switch (currentStep)
        {
            case 1:
                tutorialText.text = "Use o botão pause para abrir o menu de pause";
                break;
            case 2:
                tutorialText.text = "Clique no interagível para interagir com os puzzles.";
                break;
            case 3:
                tutorialText.text = "Boa sorte!";
                StartCoroutine(HideTutorialUnscaled(2f));
                break;
        }
    }

    IEnumerator HideTutorialUnscaled(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // Fade out manual
        float fadeTime = 0.5f;
        for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            canvasGroup.alpha = 1f - (t / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        tutorialText.gameObject.SetActive(false);
    }
}
