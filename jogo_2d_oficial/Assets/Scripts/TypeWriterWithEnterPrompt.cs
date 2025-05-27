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
    public AudioSource audioSource; // Fonte de áudio para digitação

    public GameObject enterPrompt; // Texto “Pressione ENTER para continuar”

    void Start()
    {
        enterPrompt.SetActive(false); // esconde o ENTER no início
        StartCoroutine(TypeText());
    }

    bool typingDone = false;

    void Update()
    {
        // Só permite apertar Enter depois que o texto terminou
        if (typingDone && Input.GetKeyDown(KeyCode.Return))
        {
            // Troca de cena
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
            if (!char.IsWhiteSpace(c) && typeSound != null)
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
