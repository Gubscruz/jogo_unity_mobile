using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypewriterWithEnterPrompt3 : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    [TextArea]
    public string fullText;
    public float typingSpeed = 0.05f;
    public float startDelay = 1f;
    private string sceneToLoad; // Nome da cena a ser carregada

    public GameObject enterPrompt; // Texto “Pressione ENTER para continuar”

    private PuzzleSaver puzzle;
    public AudioClip typeSound; // Som da máquina de escrever
    public AudioSource audioSource; // Fonte de áudio para digitação


    void Start()
    {
        puzzle = PuzzleSaver.Instance;
        enterPrompt.SetActive(false); // esconde o ENTER no início
        StartCoroutine(TypeText());
    }

    bool typingDone = false;

    void Update()
    {
        // Só permite apertar Enter depois que o texto terminou
        if (typingDone && Input.GetKeyDown(KeyCode.Return))
        {
            if (puzzle.entrouSalaSecreta)
            {
                SceneManager.LoadScene("Fragmento IV");
            }
            else
            {
                SceneManager.LoadScene("Final Frag 4");
            }
        }
    }

    IEnumerator TypeText()
    {
        yield return new WaitForSeconds(startDelay);

        textMeshPro.text = "";

        foreach (char c in fullText)
        {
            textMeshPro.text += c;
            if (!char.IsWhiteSpace(c) && typeSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(typeSound);
            }
            yield return new WaitForSeconds(typingSpeed);
        }

        // Mostra o texto "Pressione ENTER"
        enterPrompt.SetActive(true);
        typingDone = true;
    }
}
