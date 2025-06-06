using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class AdManager : MonoBehaviour
{
    public TMP_Text countdownText;
    public float adDuration = 5f;

    private string previousScene;

    void Start()
    {
        previousScene = PlayerPrefs.GetString("PreviousScene", "MainScene");
        StartCoroutine(PlayFakeAd());
    }

    IEnumerator PlayFakeAd()
    {
        float timeLeft = adDuration;

        while (timeLeft > 0)
        {
            countdownText.text = "Anúncio termina em " + Mathf.CeilToInt(timeLeft) + " segundos...";
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        PlayerPrefs.SetInt("WatchedAd", 1);
        SceneManager.LoadScene(previousScene);
    }
}
