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

    private string respostaCorreta = "7513";
    public GameObject painelPuzzle;

    public AudioSource audioSource;

    public AudioClip somErro;

    public AudioClip somAcerto;

    private HudVidaController hudController;

    public GameObject painelDica;

    public void Start()
    {
        puzzle = PuzzleSaver.Instance;
        hudController = HudVidaController.Instance;

        if (!puzzle.puzzle1_sala1)
        {
            botaoAvancar.gameObject.SetActive(false);
            textoFeedback.gameObject.SetActive(false);
        }

        // Checa se o jogador acabou de ver o anúncio
        if (PlayerPrefs.GetInt("WatchedAd", 0) == 1)
        {
            MostrarDica();
            PlayerPrefs.SetInt("WatchedAd", 0); // resetar
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
        //se nao incluir numero 7,5,1,3
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

        SceneManager.LoadScene("Sala I"); // Volta para a cena inicial

    }

    public void Avancar()
    {
        puzzle.puzzle1_sala1 = true;
        PuzzleProgressManager.Instance.MarkSolved("Puzzle1_Sala1");
        SceneManager.LoadScene("Sala I"); // Volta para a cena inicial
    }


    public void Dicas()
    {
        // Salva a cena atual para poder voltar depois do "anúncio"
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);

        // Marca que ainda não assistiu o anúncio
        PlayerPrefs.SetInt("WatchedAd", 0);

        // Vai para a cena de ads simulados
        SceneManager.LoadScene("Ads");
    }
    
    public void MostrarDica()
    {
        if (painelDica != null)
        {
            painelDica.SetActive(true);
        }
    }


}
 