using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    private Slot[] slots;
    public GameObject panel;
    public GameObject botaoAvancar;
    public TextMeshProUGUI textoFeedback;
    public TextMeshProUGUI anoMorte;

    private PuzzleSaver puzzle;
    private HudVidaController hudController;

    public AudioSource audioSource;
    public AudioClip somErro;
    public AudioClip somAcerto;

    public DicasController dicasController;

    public void Start()
    {
        puzzle = PuzzleSaver.Instance;
        hudController = HudVidaController.Instance;

        if (!puzzle.puzzle1_sala2)
        {
            botaoAvancar.gameObject.SetActive(false);
            textoFeedback.gameObject.SetActive(false);
        }

        // Se o jogador acabou de assistir ao anúncio, mostrar a dica
        if (PlayerPrefs.GetInt("WatchedAd", 0) == 1)
        {
            PlayerPrefs.SetInt("WatchedAd", 0); // Reseta a flag
            dicasController.ShowDica();
        }
    }

    public void DefinirSlots(Slot[] listaSlots)
    {
        slots = listaSlots;
    }

    public void CheckPuzzle()
    {
        foreach (Slot slot in slots)
        {
            if (slot.currentItem == null)
            {
                Debug.Log("Slot vazio!");
                return;
            }

            ItemDragHandle item = slot.currentItem.GetComponent<ItemDragHandle>();
            if (item.itemId != slot.slotId)
            {
                hudController.PerderVida();
                audioSource.PlayOneShot(somErro);
                Debug.Log($"Item {item.itemId} está no slot {slot.slotId} → incorreto");
                textoFeedback.text = "Não parece estar certo...";
                textoFeedback.gameObject.SetActive(true);
                return;
            }
        }

        audioSource.PlayOneShot(somAcerto);
        Debug.Log("Puzzle resolvido corretamente!");
        anoMorte.text = "Última anotação no bloco dos residentes permanentes. Encontrada sem sinais de violência nos aposentos superiores. - Data: 13/05/1895";
        textoFeedback.text = "Correto!";
        textoFeedback.gameObject.SetActive(true);
        botaoAvancar.gameObject.SetActive(true);
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Sala II");
    }

    public void Avancar()
    {
        puzzle.puzzle1_sala2 = true;
        SceneManager.LoadScene("Sala II");
        PuzzleProgressManager.Instance.MarkSolved("Puzzle1_Sala2");
        Debug.Log("Avançar para a próxima parte do jogo!");
    }

    public void Dicas()
    {
        // Salva o nome da cena atual
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);

        // Marca que o jogador ainda não viu a dica
        PlayerPrefs.SetInt("WatchedAd", 0);

        // Vai para a cena de anúncios simulados
        SceneManager.LoadScene("Ads");
    }
}
