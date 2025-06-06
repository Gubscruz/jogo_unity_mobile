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


    void Start()
    {
        puzzle = PuzzleSaver.Instance;
        enterPrompt.SetActive(false); // esconde o ENTER no início
        StartCoroutine(TypeText());
    }

    bool typingDone = false;

    void Update()
    {
        if (!typingDone) return;

        bool enterKey = Input.GetKeyDown(KeyCode.Return);
        bool clickOrTouch = Input.GetMouseButtonDown(0)
                         || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

        if (enterKey || clickOrTouch)
        {
            if (puzzle.entrouSalaSecreta)
                SceneManager.LoadScene("Fragmento IV");
            else
                SceneManager.LoadScene("Final Frag 4");
        }
    }

    IEnumerator TypeText()
    {
        yield return new WaitForSeconds(startDelay);

        textMeshPro.text = "";

        foreach (char c in fullText)
        {
            textMeshPro.text += c;
            if (!char.IsWhiteSpace(c) && typeSound != null && SfxManager.Instance != null)
            {
                SfxManager.Instance.Play(typeSound);
            }
            yield return new WaitForSeconds(typingSpeed);
        }

        // Mostra o texto "Pressione ENTER"
        enterPrompt.SetActive(true);
        typingDone = true;
    }
}
