using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIButtonSound2 : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverSound;
    public AudioClip clickSound;


    void Start()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && SfxManager.Instance != null)
            SfxManager.Instance.Play(hoverSound);
    }

    public void PlayClickSound()
    {
        if (clickSound != null && SfxManager.Instance != null)
            SfxManager.Instance.Play(clickSound);

    }

}
