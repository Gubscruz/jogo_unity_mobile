using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Puzzle2_sala3 : MonoBehaviour
{
    public int[] resposta = new int[5];
    public Texture spriteON;
    public Texture spriteOFF;
    public RawImage[] alavancaImagens;

    public TextMeshProUGUI textoFeedback;
    public Button botaoAvancar;

    private PuzzleSaver puzzle;
    private HudVidaController hudController;

    public AudioSource audioSource;
    public AudioClip somErro;
    public AudioClip somAcerto;

    public DicasController dicasController;

    void Start()
    {
        hudController = HudVidaController.Instance;
        puzzle = PuzzleSaver.Instance;

        if (!puzzle.puzzle2_sala3)
        {
            textoFeedback.gameObject.SetActive(false);
            botaoAvancar.gameObject.SetActive(false);
        }

        // Inicializa alavancas como ligadas (1)
        for (int i = 0; i < resposta.Length; i++)
        {
            resposta[i] = 1;
            alavancaImagens[i].texture = spriteON;
        }

        // Se voltou de anúncio, mostrar dica
        if (PlayerPrefs.GetInt("WatchedAd", 0) == 1)
        {
            PlayerPrefs.SetInt("WatchedAd", 0);
            dicasController.ShowDica();
        }
    }

    public void alavanca1() => AlternarAlavanca(0);
    public void alavanca2() => AlternarAlavanca(1);
    public void alavanca3() => AlternarAlavanca(2);
    public void alavanca4() => AlternarAlavanca(3);
    public void alavanca5() => AlternarAlavanca(4);

    private void AlternarAlavanca(int index)
    {
        resposta[index] = 1 - resposta[index];
        alavancaImagens[index].texture = (resposta[index] == 1) ? spriteON : spriteOFF;
        Debug.Log($"Alavanca {index + 1} agora está em {resposta[index]}");
    }

    public void Verificar()
    {
        if (resposta[0] == 0 && resposta[1] == 1 && resposta[2] == 1 && resposta[3] == 0 && resposta[4] == 1)
        {
            audioSource.PlayOneShot(somAcerto);
            textoFeedback.text = "Correto!";
            textoFeedback.gameObject.SetActive(true);
            botaoAvancar.gameObject.SetActive(true);
            Debug.Log("Puzzle resolvido corretamente!");
        }
        else
        {
            hudController.PerderVida();
            audioSource.PlayOneShot(somErro);
            textoFeedback.text = "Não parece estar certo...";
            textoFeedback.gameObject.SetActive(true);
            Debug.Log("Puzzle incorreto!");
        }
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Sala III");
        Debug.Log("Voltar para a parte anterior do jogo!");
    }

    public void Avancar()
    {
        puzzle.puzzle2_sala3 = true;
        PuzzleProgressManager.Instance.MarkSolved("Puzzle2_Sala3");
        SceneManager.LoadScene("Sala III");
        Debug.Log("Avançar para a próxima parte do jogo!");
    }

    public void Dicas()
    {
        // Redireciona para cena de anúncio antes de exibir a dica
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("WatchedAd", 0);
        SceneManager.LoadScene("Ads");
    }
}
