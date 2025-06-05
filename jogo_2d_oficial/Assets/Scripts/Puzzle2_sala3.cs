using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Puzzle2_sala3 : MonoBehaviour
{

    public int[] resposta = new int[5];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Texture spriteON;
    public Texture spriteOFF;
    public UnityEngine.UI.RawImage[] alavancaImagens;

    public TextMeshProUGUI textoFeedback; // Referência ao texto de instruções

    public Button botaoAvancar; // Referência ao botão de fechar o puzzle

    private PuzzleSaver puzzle;

    public AudioSource audioSource; // Referência ao AudioSource

    public AudioClip somErro; // Referência ao som de erro
    public AudioClip somAcerto; // Referência ao som de acerto

    private HudVidaController hudController;

    public DicasController dicasController; // Referência ao controlador de dicas

    public void alavanca1()
    {
        resposta[0] = 1 - resposta[0]; // alterna entre 0 e 1
        alavancaImagens[0].texture = (resposta[0] == 1) ? spriteON : spriteOFF;
        Debug.Log("Alavanca 1 agora está em " + resposta[0]);
    }

    public void alavanca2()
    {
        resposta[1] = 1 - resposta[1]; // alterna entre 0 e 1
        alavancaImagens[1].texture = (resposta[1] == 1) ? spriteON : spriteOFF;
        Debug.Log("Alavanca 2 agora está em " + resposta[1]);
    }

    public void alavanca3()
    {
        resposta[2] = 1 - resposta[2]; // alterna entre 0 e 1
        alavancaImagens[2].texture = (resposta[2] == 1) ? spriteON : spriteOFF;
        Debug.Log("Alavanca 2 agora está em " + resposta[2]);
    }

    public void alavanca4()
    {
        resposta[3] = 1 - resposta[3]; // alterna entre 0 e 1
        alavancaImagens[3].texture = (resposta[3] == 1) ? spriteON : spriteOFF;
        Debug.Log("Alavanca 2 agora está em " + resposta[3]);
    }

    public void alavanca5()
    {
        resposta[4] = 1 - resposta[4]; // alterna entre 0 e 1
        alavancaImagens[4].texture = (resposta[4] == 1) ? spriteON : spriteOFF;
        Debug.Log("Alavanca 2 agora está em " + resposta[4]);
    }



    void Start()
    {

        hudController = HudVidaController.Instance;
        puzzle = PuzzleSaver.Instance;
        if (!puzzle.puzzle2_sala3)
        {
            textoFeedback.gameObject.SetActive(false); // Desativa o feedback de resposta incorreta
            botaoAvancar.gameObject.SetActive(false); // Desativa o botão de avançar no início

        }

        resposta[0] = 1;
        resposta[1] = 1;
        resposta[2] = 1;
        resposta[3] = 1;
        resposta[4] = 1;

        alavancaImagens[0].texture = spriteON;
        alavancaImagens[1].texture = spriteON;
        alavancaImagens[2].texture = spriteON;
        alavancaImagens[3].texture = spriteON;
        alavancaImagens[4].texture = spriteON;
    }

    public void Verificar()
    {

        //reposta correta (13) - 0, 1, 1, 0, 1
        if (resposta[0] == 0 && resposta[1] == 1 && resposta[2] == 1 && resposta[3] == 0 && resposta[4] == 1)
        {
            Debug.Log("Puzzle resolvido corretamente!");
            audioSource.PlayOneShot(somAcerto); // Toca o som de acerto
            textoFeedback.text = "Correto!"; // Atualiza o feedback de resposta correta
            textoFeedback.gameObject.SetActive(true); // Ativa o feedback de resposta correta
            botaoAvancar.gameObject.SetActive(true); // Ativa o botão de avançar
            // Aqui você pode adicionar o código para avançar para a próxima parte do jogo
        }
        else
        {
            hudController.PerderVida();
            audioSource.PlayOneShot(somErro); // Toca o som de erro
            Debug.Log("Puzzle incorreto!");
            textoFeedback.text = "Não parece estar certo..."; // Atualiza o feedback de resposta incorreta
            textoFeedback.gameObject.SetActive(true); // Ativa o feedback de resposta incorreta
        }
    }


    public void Voltar()
    {
        SceneManager.LoadScene("Sala III"); // Volta para a cena inicial
        // Aqui você pode adicionar a lógica para voltar ao jogo, como fechar o painel do puzzle
        Debug.Log("Voltar para a parte anterior do jogo!");
    }

    public void Avancar()
    {
        // Aqui você pode adicionar a lógica para avançar no jogo, como abrir uma porta ou trocar de cena
        puzzle.puzzle2_sala3 = true;
        PuzzleProgressManager.Instance.MarkSolved("Puzzle2_Sala3");
        SceneManager.LoadScene("Sala III"); // Avança para a próxima sala
        Debug.Log("Avançar para a próxima parte do jogo!");
    }

    public void Dicas()
    {
        
        dicasController.ShowDica(); // Chama o método de exibição de dicas
    }

}
