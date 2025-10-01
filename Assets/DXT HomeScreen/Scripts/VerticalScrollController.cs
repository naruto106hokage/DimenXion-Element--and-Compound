using UnityEngine;
using UnityEngine.UI;

public class VerticalScrollController : MonoBehaviour
{
    [Header("References")]
    public ScrollRect scrollRect;
    public Button upButton;
    public Button downButton;
    public RectTransform content;
    public RectTransform viewport;
    public float scrollStep = 0.1f;

    void Start()
    {
        if (upButton != null)
            upButton.onClick.AddListener(ScrollUp);

        if (downButton != null)
            downButton.onClick.AddListener(ScrollDown);
    }

    void Update()
    {
        UpdateButtonStates();
    }

    void ScrollUp()
    {
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition + scrollStep);
    }

    void ScrollDown()
    {
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition - scrollStep);
    }

    void UpdateButtonStates()
    {
        float contentHeight = content.rect.height;
        float viewportHeight = viewport.rect.height;

        // Check if scrolling is necessary
        bool isScrollable = contentHeight > viewportHeight;

        if (upButton != null)
        {
            upButton.gameObject.SetActive(isScrollable);
            upButton.interactable = scrollRect.verticalNormalizedPosition < 0.99f;
        }

        if (downButton != null)
        {
            downButton.gameObject.SetActive(isScrollable);
            downButton.interactable = scrollRect.verticalNormalizedPosition > 0.01f;
        }
    }
}
