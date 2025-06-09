using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Puzzle2_sala2 : MonoBehaviour
{
    public TextMeshProUGUI textoLivro;
    public TextMeshProUGUI textoFeedback;
    public TMP_InputField[] camposCodigo;

    private string respostaCorreta = "321525";

    public Button botaoAvancar;

    private PuzzleSaver puzzle;
    private HudVidaController hudController;

    public AudioSource audioSource;
    public AudioClip somErro;
    public AudioClip somAcerto;

    public DicasController dicasController;

    public void abrirLivro1()
    {
        textoLivro.text = "Três homens lutaram pelo duelo proibido. Apenas um foi enterrado.";
        textoLivro.gameObject.SetActive(true);
    }

    public void abrirLivro2()
    {
        textoLivro.text = "Dias após a doença, os aldeões começaram a rezar. Ela partiu antes do quinto sino.";
        textoLivro.gameObject.SetActive(true);
    }

    public void abrirLivro3()
    {
        textoLivro.text = "Mentiras encobriram o crime. O número foi alterado duas vezes.";
        textoLivro.gameObject.SetActive(true);
    }

    public void abrirLivro4()
    {
        textoLivro.text = "Os documentos estavam datados de maio. A chave estava escrita com tinta vermelha no rodapé.";
        textoLivro.gameObject.SetActive(true);
    }

    void Start()
    {
        hudController = HudVidaController.Instance;
        puzzle = PuzzleSaver.Instance;

        if (!puzzle.puzzle2_sala2)
        {
            foreach (var campo in camposCodigo)
            {
                campo.text = "";
            }
            botaoAvancar.gameObject.SetActive(false);
            textoFeedback.gameObject.SetActive(false);
        }

        // Verifica se o jogador voltou da FakeAdScene
        if (PlayerPrefs.GetInt("WatchedAd", 0) == 1)
        {
            PlayerPrefs.SetInt("WatchedAd", 0); // Reseta a flag
            dicasController.ShowDica(); // Exibe a dica
        }
    }

    public void Verificar()
    {
        string codigoDigitado = "";
        foreach (var campo in camposCodigo)
        {
            codigoDigitado += campo.text;
        }

        if (codigoDigitado == respostaCorreta)
        {
            audioSource.PlayOneShot(somAcerto);
            textoFeedback.text = "Correto!";
            textoFeedback.gameObject.SetActive(true);
            botaoAvancar.gameObject.SetActive(true);
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(codigoDigitado, @"[a-zA-Z]"))
        {
            audioSource.PlayOneShot(somErro);
            textoFeedback.text = "A resposta não deve conter letras.";
            textoFeedback.gameObject.SetActive(true);
            hudController.PerderVida();
        }
        else if (codigoDigitado.Length == 6)
        {
            audioSource.PlayOneShot(somErro);
            textoFeedback.text = "Não parece estar certo...";
            textoFeedback.gameObject.SetActive(true);
            hudController.PerderVida();
        }
        else
        {
            audioSource.PlayOneShot(somErro);
            textoFeedback.text = "Ainda há números perdidos....";
            textoFeedback.gameObject.SetActive(true);
            hudController.PerderVida();
        }
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Sala II");
    }

    public void Avancar()
    {
        puzzle.puzzle2_sala2 = true;
        PuzzleProgressManager.Instance.MarkSolved("Puzzle2_Sala2");
        SceneManager.LoadScene("Sala II");
    }

    public void Dicas()
    {
        // Redireciona para anúncio simulado antes de mostrar dica
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("WatchedAd", 0);
        SceneManager.LoadScene("Ads");
    }
}
