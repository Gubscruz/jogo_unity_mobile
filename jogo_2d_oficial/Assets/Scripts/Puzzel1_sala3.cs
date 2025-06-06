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

    public DicasController dicasController; // Referência ao controlador de dicas


    public void Start()
    {

        puzzle = PuzzleSaver.Instance;
        if (!puzzle.puzzle1_sala3)
        {
            //loop do som de tick
            audioSource2.clip = tick;
            audioSource2.loop = true;
            audioSource2.Play();
            timer.StartPuzzle();
            inputResposta.text = "";
            botaoAvancar.gameObject.SetActive(false);
            textoFeedback.gameObject.SetActive(false);

        }
    }


    public void Verificar()
    {

        string respostaDoJogador = inputResposta.text.Trim().ToLower();
        if (respostaCorreta == respostaDoJogador)
        {
            audioSource.PlayOneShot(somAcerto); // Toca o som de acerto
            textoFeedback.text = "Resposta Correta!";
            textoFeedback.gameObject.SetActive(true);
            botaoAvancar.gameObject.SetActive(true);
            timer.OnResolverClicked();
            // SceneManager.LoadScene("Sala4");
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(inputResposta.text, @"[a-zA-Z]"))
        {
            audioSource.PlayOneShot(somErro); // Toca o som de erro
            textoFeedback.text = "A resposta não deve conter letras.";
            textoFeedback.gameObject.SetActive(true);
        }
        else
        {
            audioSource.PlayOneShot(somErro); // Toca o som de erro
            textoFeedback.text = "Não parece estar certo...";
            textoFeedback.gameObject.SetActive(true);
        }
    }

    public void Voltar()
    {
        audioSource2.Stop(); // Para o som de tick
        SceneManager.LoadScene("Sala III"); // Volta para a cena inicia
    }

    public void Avancar()
    {
        audioSource2.Stop(); // Para o som de tick
        puzzle.puzzle1_sala3 = true;
        PuzzleProgressManager.Instance.MarkSolved("Puzzle1_Sala3");
        SceneManager.LoadScene("Sala III"); // Avança para a próxima sala

    }
    
    public void Dicas()
    {
        
        dicasController.ShowDica(); // Chama o método de exibição de dicas
    }

    



}
