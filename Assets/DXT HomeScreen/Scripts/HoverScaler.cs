using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float scaleFactor = 1.1f;
    public float scaleSpeed = 5f;
    [SerializeField] private Text text;
    [SerializeField] private Color color;

    private Color originalColor;
    private Vector3 targetScale;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        originalColor = text.color;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleFactor;
        text.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
        text.color = originalColor;
    }
}