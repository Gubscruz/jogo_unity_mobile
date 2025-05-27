using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;
    private void Start()
    {
        originalScale = transform.localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * 0.9f;
    }
    public void OnPointerUp(PointerEventData eventData) {
        transform.localScale = originalScale;      
    }
}
