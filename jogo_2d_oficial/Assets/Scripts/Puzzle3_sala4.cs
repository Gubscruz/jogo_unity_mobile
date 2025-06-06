using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class Puzzle3_sala4 : MonoBehaviour
{
    public Button[] botoes;
    public Color corSelecionada = new Color(1f, 0.9f, 0.6f);
    public Color corOriginal = Color.white;
    public Color corHover = new Color(0.9f, 0.9f, 1f);

    private bool[] selecionado;
    private List<int> ordemClicada = new List<int>();

    public TextMeshProUGUI textoFeedback;
    public Button avancarBotao;

    private PuzzleSaver puzzle;
    private HudVidaController hudController;

    public AudioSource audioSource;
    public AudioClip somErro;
    public AudioClip somAcerto;

    public int[] sequenciaCorreta = new int[] { 2, 3, 0 };

    public DicasController dicasController;

    public void Dicas()
    {
        // Salva a cena atual e vai para o anúncio
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("WatchedAd", 0);
        SceneManager.LoadScene("Ads");
    }

    void Start()
    {
        puzzle = PuzzleSaver.Instance;
        hudController = HudVidaController.Instance;

        if (!puzzle.puzzle3_sala4)
        {
            textoFeedback.gameObject.SetActive(false);
            avancarBotao.gameObject.SetActive(false);
        }

        // Se voltou da cena de anúncio, exibe a dica
        if (PlayerPrefs.GetInt("WatchedAd", 0) == 1)
        {
            PlayerPrefs.SetInt("WatchedAd", 0);
            dicasController.ShowDica();
        }

        selecionado = new bool[botoes.Length];

        for (int i = 0; i < botoes.Length; i++)
        {
            int index = i;
            botoes[i].onClick.AddListener(() => AlternarCor(index));

            EventTrigger trigger = botoes[i].gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = botoes[i].gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry enter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            enter.callback.AddListener((data) => OnHoverEnter(index));
            trigger.triggers.Add(enter);

            EventTrigger.Entry exit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            exit.callback.AddListener((data) => OnHoverExit(index));
            trigger.triggers.Add(exit);
        }
    }

    void AlternarCor(int index)
    {
        RawImage imagem = botoes[index].transform.parent.GetComponent<RawImage>();
        if (imagem == null) return;

        selecionado[index] = !selecionado[index];

        if (selecionado[index])
        {
            imagem.color = corSelecionada;
            ordemClicada.Add(index);
        }
        else
        {
            imagem.color = corOriginal;
            ordemClicada.RemoveAll(i => i == index);
        }

        if (TodosDesmarcados())
        {
            ordemClicada.Clear();
        }
    }

    void OnHoverEnter(int index)
    {
        RawImage imagem = botoes[index].transform.parent.GetComponent<RawImage>();
        if (imagem != null)
            imagem.color = corHover;
    }

    void OnHoverExit(int index)
    {
        RawImage imagem = botoes[index].transform.parent.GetComponent<RawImage>();
        if (imagem != null)
            imagem.color = selecionado[index] ? corSelecionada : corOriginal;
    }

    bool TodosDesmarcados()
    {
        foreach (bool s in selecionado)
        {
            if (s) return false;
        }
        return true;
    }

    public void Verificar()
    {
        if (ordemClicada.Count != sequenciaCorreta.Length)
        {
            hudController.PerderVida();
            audioSource.PlayOneShot(somErro);
            textoFeedback.text = "Isso não parece estar certo... Lembre-se tudo na vida tem uma ordem!";
            textoFeedback.gameObject.SetActive(true);
            return;
        }

        for (int i = 0; i < sequenciaCorreta.Length; i++)
        {
            if (ordemClicada[i] != sequenciaCorreta[i])
            {
                hudController.PerderVida();
                textoFeedback.text = "Isso não parece estar certo... Lembre-se tudo na vida tem uma ordem!";
                audioSource.PlayOneShot(somErro);
                textoFeedback.gameObject.SetActive(true);
                return;
            }
        }

        audioSource.PlayOneShot(somAcerto);
        textoFeedback.text = "Correto! Você conseguiu!";
        avancarBotao.gameObject.SetActive(true);
        textoFeedback.gameObject.SetActive(true);
    }

    public void Avancar()
    {
        puzzle.puzzle3_sala4 = true;
        PuzzleProgressManager.Instance.MarkSolved("Puzzle3_Sala4");
        SceneManager.LoadScene("Sala IV");
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Sala IV");
    }
}
