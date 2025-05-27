using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypewriterWithEnterPrompt : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    [TextArea]
    public string fullText;
    public float typingSpeed = 0.05f;
    public float startDelay = 1f;
    public string sceneToLoad; // Nome da cena a ser carregada
    public AudioClip typeSound; // Som da máquina de escrever

    public GameObject enterPrompt; // Texto “Pressione ENTER para continuar”

    void Start()
    {
        enterPrompt.SetActive(false); // esconde o ENTER no início
        StartCoroutine(TypeText());
    }

    bool typingDone = false;

    void Update()
    {
        if (!typingDone) return;
        // Só permite apertar Enter depois que o texto terminou
        bool enterKey = Input.GetKeyDown(KeyCode.Return);
        bool clickOrTouch = Input.GetMouseButtonDown(0)
                         || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

        if (enterKey || clickOrTouch)
        {
            SceneManager.LoadScene(sceneToLoad);
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
