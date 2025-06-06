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

    public Button botaoAvancar;
    public TextMeshProUGUI textoFeedback;

    private PuzzleSaver puzzle;
    private HudVidaController hudController;

    public AudioSource audioSource;
    public AudioClip somErro;
    public AudioClip somAcerto;

    public DicasController dicasController;

    void Start()
    {
        puzzle = PuzzleSaver.Instance;
        hudController = HudVidaController.Instance;

        if (!puzzle.puzzle2_sala4)
        {
            textoFeedback.gameObject.SetActive(false);
            botaoAvancar.gameObject.SetActive(false);
            ApagarTodasAsLinhas();
        }

        // Verifica se voltou de anúncio para exibir dica
        if (PlayerPrefs.GetInt("WatchedAd", 0) == 1)
        {
            PlayerPrefs.SetInt("WatchedAd", 0);
            dicasController.ShowDica();
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

        if (Input.GetMouseButtonDown(1))
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
        if (linhaAtual == null) return;

        Vector2 screenStart = Camera.main.WorldToScreenPoint(worldStart);
        Vector2 screenEnd = Camera.main.WorldToScreenPoint(worldEnd);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenStart, Camera.main, out Vector2 localStart);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenEnd, Camera.main, out Vector2 localEnd);

        Vector2 direction = localEnd - localStart;
        float distance = direction.magnitude;

        linhaAtual.sizeDelta = new Vector2(distance, 4f);
        linhaAtual.anchoredPosition = localStart + direction * 0.5f;
        linhaAtual.localRotation = Quaternion.FromToRotation(Vector3.right, direction);
    }

    public void ApagarTodasAsLinhas()
    {
        foreach (var linha in linhasCriadas)
            Destroy(linha);

        linhasCriadas.Clear();
        ligacoesFeitas.Clear();
    }

    public void ApagarUltimaLinha()
    {
        if (linhasCriadas.Count > 0)
        {
            Destroy(linhasCriadas[^1]);
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
            hudController.PerderVida();
            audioSource.PlayOneShot(somErro);
            textoFeedback.text = "Não parece estar certo...";
            textoFeedback.gameObject.SetActive(true);
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
            audioSource.PlayOneShot(somAcerto);
            textoFeedback.text = "Correto!";
            textoFeedback.gameObject.SetActive(true);
            botaoAvancar.gameObject.SetActive(true);
        }
        else
        {
            hudController.PerderVida();
            audioSource.PlayOneShot(somErro);
            textoFeedback.text = "Não parece estar certo...";
            textoFeedback.gameObject.SetActive(true);
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
        SceneManager.LoadScene("Sala IV");
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Sala IV");
    }

    public void Dicas()
    {
        // Redireciona para anúncio antes de exibir dica
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("WatchedAd", 0);
        SceneManager.LoadScene("Ads");
    }
}
