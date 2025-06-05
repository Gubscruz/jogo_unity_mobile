using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Puzzle2_sala4 : MonoBehaviour
{
    public RectTransform canvasRect;
    public GameObject linePrefab;

    private List<GameObject> linhasCriadas = new List<GameObject>();
    private List<(RectTransform, RectTransform)> ligacoesFeitas = new List<(RectTransform, RectTransform)>();

    public RectTransform quadro1;
    public RectTransform quadro2;
    public RectTransform quadro3;
    public RectTransform quadro4;

    private RectTransform primeiroQuadro = null;
    private RectTransform linhaAtual = null;

    public Button botaoAvancar; // Referência ao botão de avançar

    public TextMeshProUGUI textoFeedback; // Referência ao texto de feedback

    private PuzzleSaver puzzle;
    public AudioSource audioSource; // Referência ao AudioSource
    public AudioClip somErro; // Referência ao som de erro
    public AudioClip somAcerto; // Referência ao som de acerto

    private HudVidaController hudController;

    public DicasController dicasController; // Referência ao controlador de dicas

    void Start()
    {
        puzzle = PuzzleSaver.Instance;
        hudController = HudVidaController.Instance;
        if (!puzzle.puzzle2_sala4)
        {
            textoFeedback.gameObject.SetActive(false); // Desativa o feedback de resposta incorreta
            botaoAvancar.gameObject.SetActive(false); // Desativa o botão de avançar no início
            ApagarTodasAsLinhas(); // Limpa todas as linhas criadas
        }


    }

    void Update()
    {
        if (linhaAtual != null && primeiroQuadro != null)
        {
            Vector3 startWorld = primeiroQuadro.position;
            Vector3 mouseScreen = Input.mousePosition;
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            mouseWorld.z = startWorld.z;

            AtualizarLinha(startWorld, mouseWorld);
        }

        if (Input.GetMouseButtonDown(1)) // botão direito
        {
            if (linhaAtual != null)
            {
                Destroy(linhaAtual.gameObject);
                linhaAtual = null;
                primeiroQuadro = null;
            }
        }
    }

    public void QuadroClicado(RectTransform clicado)
    {
        if (primeiroQuadro == null)
        {
            primeiroQuadro = clicado;
            GameObject linhaGO = Instantiate(linePrefab, canvasRect);
            linhaAtual = linhaGO.GetComponent<RectTransform>();
            linhasCriadas.Add(linhaGO);

        }
        else
        {
            AtualizarLinha(primeiroQuadro.position, clicado.position);
            ligacoesFeitas.Add((primeiroQuadro, clicado));
            primeiroQuadro = null;
            linhaAtual = null;
        }


    }

    private void AtualizarLinha(Vector3 worldStart, Vector3 worldEnd)
    {
        Vector2 localStart;
        Vector2 localEnd;

        Vector2 screenStart = Camera.main.WorldToScreenPoint(worldStart);
        Vector2 screenEnd = Camera.main.WorldToScreenPoint(worldEnd);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenStart, Camera.main, out localStart);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenEnd, Camera.main, out localEnd);

        Vector2 direction = localEnd - localStart;
        float distance = direction.magnitude;

        if (linhaAtual == null) return;

        linhaAtual.sizeDelta = new Vector2(distance, 4f); // altura fixa da linha
        linhaAtual.anchoredPosition = localStart + direction * 0.5f;
        linhaAtual.localRotation = Quaternion.FromToRotation(Vector3.right, direction);
    }

    public void ApagarTodasAsLinhas()
    {
        foreach (var linha in linhasCriadas)
        {
            Destroy(linha);

        }
        for (int i = 0; i < ligacoesFeitas.Count; i++)
        {
            ligacoesFeitas.RemoveAt(i);
            i--;
        }
        linhasCriadas.Clear();
        ligacoesFeitas.Clear();
    }


    public void ApagarUltimaLinha()
    {
        if (linhasCriadas.Count > 0)
        {
            var ultima = linhasCriadas[linhasCriadas.Count - 1];
            Destroy(ultima);
            linhasCriadas.RemoveAt(linhasCriadas.Count - 1);


        }
        if (ligacoesFeitas.Count > 0)
        {
            ligacoesFeitas.RemoveAt(ligacoesFeitas.Count - 1);
        }
    }


    public void VerificarLigacoes(RectTransform quadro1, RectTransform quadro2, RectTransform quadro3, RectTransform quadro4)
    {
        var ligacoesCorretas = new List<(RectTransform, RectTransform)>
        {
            (quadro1, quadro2),
            (quadro2, quadro4),
            (quadro3, quadro4)
        };

        int corretas = 0;

        if (ligacoesFeitas.Count != ligacoesCorretas.Count)
        {
            hudController.PerderVida(); // Remove uma vida
            Debug.Log($"incorreto qunatidade");
            Debug.Log($"ligacoesFeitas: {ligacoesFeitas.Count}");
            Debug.Log($"ligacoesCorretas: {ligacoesCorretas.Count}");
            audioSource.PlayOneShot(somErro); // Toca o som de erro
            textoFeedback.text = "Não parece estar certo..."; // Atualiza o feedback de resposta incorreta
            textoFeedback.gameObject.SetActive(true); // Ativa o feedback de resposta incorreta
            return;
        }

        foreach (var correta in ligacoesCorretas)
        {
            foreach (var feita in ligacoesFeitas)
            {
                if ((feita.Item1 == correta.Item1 && feita.Item2 == correta.Item2) ||
                    (feita.Item1 == correta.Item2 && feita.Item2 == correta.Item1))
                {
                    corretas++;
                    break;
                }
            }
        }
        if (corretas == ligacoesCorretas.Count)
        {
            Debug.Log($"correto");
            audioSource.PlayOneShot(somAcerto); // Toca o som de acerto
            textoFeedback.text = "Correto!"; // Atualiza o feedback de resposta correta
            textoFeedback.gameObject.SetActive(true); // Ativa o feedback de resposta correta
            botaoAvancar.gameObject.SetActive(true); // Ativa o botão de avançar

        }
        else
        {
            hudController.PerderVida(); // Remove uma vida
            Debug.Log($"incorreto");
            audioSource.PlayOneShot(somErro); // Toca o som de erro
            textoFeedback.text = "Não parece estar certo..."; // Atualiza o feedback de resposta incorreta
            textoFeedback.gameObject.SetActive(true); // Ativa o feedback de resposta incorreta
        }
    }

    public void Verificar()
    {
        VerificarLigacoes(quadro1, quadro2, quadro3, quadro4);
    }

    public void Avancar()
    {
        puzzle.puzzle2_sala4 = true;
        PuzzleProgressManager.Instance.MarkSolved("Puzzle2_Sala4");
        SceneManager.LoadScene("Sala IV"); // Avança para a próxima sala
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Sala IV"); // Volta para a cena inicial
        // Aqui você pode adicionar a lógica para voltar ao jogo, como fechar o painel do puzzle
        Debug.Log("Voltar para a parte anterior do jogo!");
    }

    public void Dicas()
    {
        dicasController.ShowDica(); 
    }


}
