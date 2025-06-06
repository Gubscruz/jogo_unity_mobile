using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Puzzle_Sala1 : MonoBehaviour
{
    public TMP_InputField inputResposta;
    public Button botaoAvancar;
    public TextMeshProUGUI textoFeedback;

    private PuzzleSaver puzzle;
    private HudVidaController hudController;

    private string respostaCorreta = "7513";

    public GameObject painelPuzzle;

    public AudioSource audioSource;
    public AudioClip somErro;
    public AudioClip somAcerto;

    public DicasController dicasController;

    void Start()
    {
        puzzle = PuzzleSaver.Instance;
        hudController = HudVidaController.Instance;

        if (!puzzle.puzzle1_sala1)
        {
            botaoAvancar.gameObject.SetActive(false);
            textoFeedback.gameObject.SetActive(false);
        }

        // Se o jogador acabou de ver o anúncio, mostra a dica
        if (PlayerPrefs.GetInt("WatchedAd", 0) == 1)
        {
            PlayerPrefs.SetInt("WatchedAd", 0); // Reseta
            dicasController.ShowDica(); // Mostra dica
        }
    }

    public void Verificar()
    {
        string respostaDoJogador = inputResposta.text.Trim().ToLower();

        if (respostaDoJogador == respostaCorreta.ToLower())
        {
            textoFeedback.text = "Correto!";
            textoFeedback.gameObject.SetActive(true);
            botaoAvancar.gameObject.SetActive(true);
            audioSource.PlayOneShot(somAcerto);
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(respostaDoJogador, @"[a-zA-Z]"))
        {
            textoFeedback.text = "A resposta não deve conter letras.";
            textoFeedback.gameObject.SetActive(true);
            audioSource.PlayOneShot(somErro);
            hudController.PerderVida();
        }
        else if (!respostaDoJogador.Contains("7") || !respostaDoJogador.Contains("5") || !respostaDoJogador.Contains("1") || !respostaDoJogador.Contains("3"))
        {
            textoFeedback.text = "A resposta deve conter os números 7, 5, 1 e 3.";
            textoFeedback.gameObject.SetActive(true);
            audioSource.PlayOneShot(somErro);
            hudController.PerderVida();
        }
        else
        {
            textoFeedback.text = "Não parece estar certo...";
            textoFeedback.gameObject.SetActive(true);
            audioSource.PlayOneShot(somErro);
            hudController.PerderVida();
        }
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Sala I");
    }

    public void Avancar()
    {
        puzzle.puzzle1_sala1 = true;
        PuzzleProgressManager.Instance.MarkSolved("Puzzle1_Sala1");
        SceneManager.LoadScene("Sala I");
    }

    // Botão "Ver Dicas" chama esta função
    public void Dicas()
    {
        // Salva cena atual
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);

        // Marca que ainda não assistiu ao anúncio
        PlayerPrefs.SetInt("WatchedAd", 0);

        // Vai pra tela de ads simulados
        SceneManager.LoadScene("Ads");
    }
}
