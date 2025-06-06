using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    private Slot[] slots;
    public GameObject panel; // Referência ao painel onde os slots estão localizados

    public GameObject botaoAvancar; // Referência ao botão de fechar o puzzle

    public TextMeshProUGUI textoFeedback; // Referência ao texto de feedback

    public TextMeshProUGUI anoMorte; // Referência ao texto de instruções

    private PuzzleSaver puzzle;

    public AudioSource audioSource; // Referência ao AudioSource
    public AudioClip somErro; // Referência ao som de erro
    public AudioClip somAcerto; // Referência ao som de acerto
    private HudVidaController hudController;

    public DicasController dicasController; // Referência ao controlador de dicas

    public void Start()
    {
        puzzle = PuzzleSaver.Instance;
        hudController = HudVidaController.Instance;

        if (!puzzle.puzzle1_sala2)
        {
            botaoAvancar.gameObject.SetActive(false); // Desativa o botão de avançar no início
            textoFeedback.gameObject.SetActive(false); // Desativa o feedback de resposta incorreta
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
                hudController.PerderVida(); // Chama o método de perda
                audioSource.PlayOneShot(somErro); // Toca o som de erro
                Debug.Log($"Item {item.itemId} está no slot {slot.slotId} → incorreto");
                textoFeedback.text = "Não parece estar certo..."; // Atualiza o feedback de resposta incorreta
                textoFeedback.gameObject.SetActive(true); // Ativa o feedback de resposta incorreta
                return;
            }
        }

        audioSource.PlayOneShot(somAcerto); // Toca o som de acerto
        Debug.Log("Puzzle resolvido corretamente!");
        anoMorte.text = "Última anotação no bloco dos residentes permanentes. Encontrada sem sinais de violência nos aposentos superiores. - Data: 13/05/1895";
        textoFeedback.text = "Correto!"; // Atualiza o feedback de resposta correta
        textoFeedback.gameObject.SetActive(true); // Ativa o feedback de resposta correta
        botaoAvancar.gameObject.SetActive(true); // Ativa o botão de avançar
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Sala II"); // Volta para a cena inicial

    }

    public void Avancar()
    {
        puzzle.puzzle1_sala2 = true;
        SceneManager.LoadScene("Sala II"); // Volta para a cena inicial
        PuzzleProgressManager.Instance.MarkSolved("Puzzle1_Sala2");
        // Aqui você pode adicionar a lógica para avançar no jogo, como abrir uma porta ou trocar de cena
        Debug.Log("Avançar para a próxima parte do jogo!");
    }

    public void Dicas()
    {
        Debug.Log("Exibindo dicas para o puzzle atual");
        dicasController.ShowDica(); // Chama o método para exibir a dica
    }
}
