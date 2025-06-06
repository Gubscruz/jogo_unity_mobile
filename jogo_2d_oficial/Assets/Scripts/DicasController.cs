using UnityEngine;
using UnityEngine.UI;
using TMPro; // Ensure you have the TextMeshPro package installed

public class DicasController : MonoBehaviour
{
    public TextMeshProUGUI dicasText;

    public GameObject panel; // Referência ao painel de dicas


    public string puzzle = "puzzle1_sala1"; // Nome do puzzle atual
    public string[] dicas = {
        "Dica 1: Tente forma uma palavra com as letras.",
        "Dica 2: Perceba que cada letra tem um valor diferente.",
        "Dica 3: A palavra secreta é relacionada a botânica.",

    };


    public void ShowDica()
    {
        panel.SetActive(true); // Ativa o painel de dicas
        DicaInfo dicasInfo = Dicas.dicasPorPuzzle[puzzle];
        Debug.Log($"Dicas para {puzzle}: {dicasInfo.dicasVistas}/{dicasInfo.maxDicas}");
        if (dicasInfo.dicasVistas < dicasInfo.maxDicas)
        {
            Debug.Log("exibindo dica");
            dicasText.text = dicas[dicasInfo.dicasVistas];
            dicasInfo.dicasVistas++;
            Dicas.dicasPorPuzzle[puzzle] = dicasInfo; // Atualiza o dicionário com as novas informações
            dicasText.gameObject.SetActive(true); // Certifique-se de que o GameObject está ativo
        }
        else
        {
            Debug.Log("Todas as dicas já foram vistas");
            dicasText.text = "Todas as dicas já foram vistas.";
            dicasText.gameObject.SetActive(true); // Desativa o GameObject se todas as dicas foram vistas
        }
    }
    

    public void HideDica()
    {
        panel.SetActive(false); // Desativa o painel de dicas
        dicasText.gameObject.SetActive(false); // Desativa o GameObject de dicas
        dicasText.text = ""; // Limpa o texto da dica
    }
}
