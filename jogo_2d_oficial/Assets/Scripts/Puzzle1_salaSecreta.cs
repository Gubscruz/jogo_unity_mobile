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

    public GameObject dicas; // Referência ao painel onde os slots estão localizados

    public GameObject botaoAvancar; // Referência ao botão de fechar o puzzle

    public TextMeshProUGUI textoFeedback; // Referência ao texto de feedback

    private bool personResolved = false; // Flag para verificar se o puzzle de pessoas foi resolvido
    private bool symbolResolved = false; // Flag para verificar se o puzzle de símbolos foi resolvido

    private PuzzleSaver puzzle;
    
    private HudVidaController hudController;

    public AudioSource audioSource;
    public AudioClip somErro;
    public AudioClip somAcerto;


    public void Start()

    {
        hudController = HudVidaController.Instance;
        puzzle = PuzzleSaver.Instance; // Obtém a instância do PuzzleSaver
        if (puzzle.puzzle1_salaSecreta)
        {
            textoFeedback.gameObject.SetActive(false); // Desativa o feedback de resposta incorreta
            botaoAvancar.gameObject.SetActive(false); // Desativa o botão de avançar no início
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
            if (slot.currentItem == null)
            {
                Debug.Log("Slot vazio!");
                return;
            }

            ItemDragHandle2 item = slot.currentItem.GetComponent<ItemDragHandle2>();
            if (item.itemId != slot.slotId)
            {
                
                Debug.Log($"Puzzle de pessoas incorreto! Item {item.itemId} está no slot {slot.slotId}");
                return;
            }
        }

        Debug.Log("Puzzle de pessoas resolvido corretamente!");
        personResolved = true; // Marca o puzzle de pessoas como resolvido

    }


    public void checkSymbol()
    {
        foreach (Slot slot in slotsSymbol)
        {
            if (slot.currentItem == null)
            {
                Debug.Log("Slot vazio!");
                return;
            }

            ItemDragHandle2 item = slot.currentItem.GetComponent<ItemDragHandle2>();
            if (item.itemId != slot.slotId)
            {
                Debug.Log($"Puzzle de símbolos incorreto! Item {item.itemId} está no slot {slot.slotId}");
                return;
            }
        }

        Debug.Log("Puzzle de símbolos resolvido corretamente!");
        symbolResolved = true; // Marca o puzzle de símbolos como resolvido
    }

    public void Verificar()
    {
        checkPerson(); // Verifica o puzzle de pessoas
        checkSymbol(); // Verifica o puzzle de símbolos
        // Verifica se ambos os puzzles foram resolvidos
        if (personResolved && symbolResolved)
        {
            audioSource.PlayOneShot(somAcerto); // Toca o som de acerto
            Debug.Log("Ambos os puzzles foram resolvidos corretamente!");
            textoFeedback.text = "Correto!"; // Atualiza o feedback de resposta correta
            textoFeedback.gameObject.SetActive(true); // Ativa o feedback de resposta correta
            botaoAvancar.gameObject.SetActive(true); // Ativa o botão de avançar
        }
        else if (personResolved)
        {
            audioSource.PlayOneShot(somErro); // Toca o som de erro
            hudController.PerderVida();
            textoFeedback.text = "Lembre-se de ordenar os simbolos tambem"; // Atualiza o feedback de resposta incorreta
            textoFeedback.gameObject.SetActive(true); // Ativa o feedback de resposta incorreta
        }
        else if (symbolResolved)
        {
            audioSource.PlayOneShot(somErro); // Toca o som de erro
            hudController.PerderVida();
            textoFeedback.text = "Lembre-se de ordenar as pessoas tambem"; // Atualiza o feedback de resposta incorreta
            textoFeedback.gameObject.SetActive(true); // Ativa o feedback de resposta incorreta
        }
        else
        {
            audioSource.PlayOneShot(somErro); // Toca o som de erro
            hudController.PerderVida();
            textoFeedback.text = "Isso não parece estar certo... Lembre-se tudo na vida tem uma ordem!"; // Atualiza o feedback de resposta incorreta
            textoFeedback.gameObject.SetActive(true); // Ativa o feedback de resposta incorreta
        }
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Sala Secreta"); // Volta para a sala 2
    }

    public void Avancar()
    {
        SceneManager.LoadScene("Sala Secreta"); // Volta para a sala 2
        puzzle.puzzle1_salaSecreta = true; // Marca o puzzle como resolvido
        Debug.Log("Puzzle resolvido!"); // Log para depuração
        Debug.Log(puzzle.puzzle1_salaSecreta); // Log para depuração
        PuzzleProgressManager.Instance.MarkSolved("Puzzle1_SalaSecreta"); // Marca o puzzle como resolvido no gerenciador de progresso
    }

    public void Dicas()
    {
        dicas.SetActive(true); // Ativa o painel de dicas
    }

    public void FecharDicas()
    {
        dicas.SetActive(false); // Desativa o painel de dicas
    }
}
