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

    public AudioSource audioSource;
    public AudioClip somErro;
    public AudioClip somAcerto;

    private HudVidaController hudController;

    public DicasController dicasController; // Referência ao controlador de dicas

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
            botaoAvancar.gameObject.SetActive(false); // Desativa o botão de avançar no início
            textoFeedback.gameObject.SetActive(false); // Desativa o feedback de resposta incorreta
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
            audioSource.PlayOneShot(somAcerto); // Toca o som de acerto
            textoFeedback.text = "Correto!";
            textoFeedback.gameObject.SetActive(true);
            botaoAvancar.gameObject.SetActive(true);
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(codigoDigitado, @"[a-zA-Z]"))
        {
            audioSource.PlayOneShot(somErro); // Toca o som de erro
            textoFeedback.text = "A resposta não deve conter letras.";
            textoFeedback.gameObject.SetActive(true);
            hudController.PerderVida();
        }
        else if (codigoDigitado.Length == 6)
        {
            audioSource.PlayOneShot(somErro); // Toca o som de erro
            textoFeedback.text = "Não parece estar certo...";
            textoFeedback.gameObject.SetActive(true);
            hudController.PerderVida();
        }
        else
        {
            audioSource.PlayOneShot(somErro); // Toca o som de erro
            textoFeedback.text = "Ainda há números perdidos....";
            textoFeedback.gameObject.SetActive(true);
            hudController.PerderVida();
        }
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Sala II"); // Volta para a cena inicial

    }

    public void Avancar()
    {
        puzzle.puzzle2_sala2 = true;
        PuzzleProgressManager.Instance.MarkSolved("Puzzle2_Sala2");
        SceneManager.LoadScene("Sala II"); // Volta para a cena inicial
        Debug.Log("Avançar para a próxima parte do jogo!");
    }
    
    public void Dicas()
    {
        dicasController.ShowDica(); // Chama o método de exibição de dicas do controlador
    }
}
