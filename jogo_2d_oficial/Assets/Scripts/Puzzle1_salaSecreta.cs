using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class Puzzle1_salaSecreta : MonoBehaviour
{
    private Slot[] slotsPerson;
    private Slot[] slotsSymbol;

    public GameObject dicas;
    public GameObject botaoAvancar;
    public TextMeshProUGUI textoFeedback;

    private bool personResolved = false;
    private bool symbolResolved = false;

    private PuzzleSaver puzzle;
    private HudVidaController hudController;

    public AudioSource audioSource;
    public AudioClip somErro;
    public AudioClip somAcerto;

    public DicasController dicasController;

    public void Start()
    {
        hudController = HudVidaController.Instance;
        puzzle = PuzzleSaver.Instance;

        if (puzzle.puzzle1_salaSecreta)
        {
            textoFeedback.gameObject.SetActive(false);
            botaoAvancar.gameObject.SetActive(false);
        }

        // Se voltou de um anúncio, mostrar a dica
        if (PlayerPrefs.GetInt("WatchedAd", 0) == 1)
        {
            PlayerPrefs.SetInt("WatchedAd", 0);
            dicasController.ShowDica();
        }
    }

    public void DefinirSlotsPerson(Slot[] listaSlots)
    {
        slotsPerson = listaSlots;
    }

    public void DefinirSlotsSymbol(Slot[] listaSlots)
    {
        slotsSymbol = listaSlots;
    }

    public void checkPerson()
    {
        foreach (Slot slot in slotsPerson)
        {
            if (slot.currentItem == null) return;

            ItemDragHandle2 item = slot.currentItem.GetComponent<ItemDragHandle2>();
            if (item.itemId != slot.slotId)
            {
                Debug.Log($"Puzzle de pessoas incorreto! Item {item.itemId} está no slot {slot.slotId}");
                return;
            }
        }

        Debug.Log("Puzzle de pessoas resolvido corretamente!");
        personResolved = true;
    }

    public void checkSymbol()
    {
        foreach (Slot slot in slotsSymbol)
        {
            if (slot.currentItem == null) return;

            ItemDragHandle2 item = slot.currentItem.GetComponent<ItemDragHandle2>();
            if (item.itemId != slot.slotId)
            {
                Debug.Log($"Puzzle de símbolos incorreto! Item {item.itemId} está no slot {slot.slotId}");
                return;
            }
        }

        Debug.Log("Puzzle de símbolos resolvido corretamente!");
        symbolResolved = true;
    }

    public void Verificar()
    {
        checkPerson();
        checkSymbol();

        if (personResolved && symbolResolved)
        {
            audioSource.PlayOneShot(somAcerto);
            textoFeedback.text = "Correto!";
            textoFeedback.gameObject.SetActive(true);
            botaoAvancar.gameObject.SetActive(true);
            Debug.Log("Ambos os puzzles foram resolvidos corretamente!");
        }
        else if (personResolved)
        {
            audioSource.PlayOneShot(somErro);
            hudController.PerderVida();
            textoFeedback.text = "Lembre-se de ordenar os simbolos também";
            textoFeedback.gameObject.SetActive(true);
        }
        else if (symbolResolved)
        {
            audioSource.PlayOneShot(somErro);
            hudController.PerderVida();
            textoFeedback.text = "Lembre-se de ordenar as pessoas também";
            textoFeedback.gameObject.SetActive(true);
        }
        else
        {
            audioSource.PlayOneShot(somErro);
            hudController.PerderVida();
            textoFeedback.text = "Isso não parece estar certo... Lembre-se tudo na vida tem uma ordem!";
            textoFeedback.gameObject.SetActive(true);
        }
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Sala Secreta");
    }

    public void Avancar()
    {
        SceneManager.LoadScene("Sala Secreta");
        puzzle.puzzle1_salaSecreta = true;
        Debug.Log("Puzzle resolvido!");
        Debug.Log(puzzle.puzzle1_salaSecreta);
        PuzzleProgressManager.Instance.MarkSolved("Puzzle1_SalaSecreta");
    }

    public void Dicas()
    {
        // Vai para o anúncio antes de mostrar dica
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("WatchedAd", 0);
        SceneManager.LoadScene("Ads");
    }

    public void FecharDicas()
    {
        dicas.SetActive(false);
    }

    // Para retrocompatibilidade se o botão antigo chamar isso
    public void DicasShow()
    {
        Dicas(); // Redireciona para o novo método
    }
}
