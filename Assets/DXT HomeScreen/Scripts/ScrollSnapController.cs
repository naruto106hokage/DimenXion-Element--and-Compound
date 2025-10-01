using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrollSnapController : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform contentPanel;
    public Button leftButton;
    public Button rightButton;

    [Header("Fade Settings")]
    public float fadedAlpha = 0.3f;
    public float focusedAlpha = 1f;

    private List<RectTransform> items = new List<RectTransform>();
    private int currentIndex = 0;

    void Start()
    {
        // Collect all children as items
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            RectTransform child = contentPanel.GetChild(i) as RectTransform;
            if (child != null)
                items.Add(child);
        }

        if (items.Count == 0)
        {
            Debug.LogWarning("No items found under contentPanel.");
            return;
        }

        // Set default index (second item or first if only one)
        currentIndex = items.Count > 1 ? 1 : 0;

        // Attach button listeners
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);

        UpdateFocus();
    }

    void MoveLeft()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateFocus();
        }
    }

    void MoveRight()
    {
        if (currentIndex < items.Count - 1)
        {
            currentIndex++;
            UpdateFocus();
        }
    }

    void UpdateFocus()
    {
        for (int i = 0; i < items.Count; i++)
        {
            CanvasGroup cg = items[i].GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = items[i].gameObject.AddComponent<CanvasGroup>();
            }

            bool isFocused = (i == currentIndex);
            cg.alpha = isFocused ? focusedAlpha : fadedAlpha;
            cg.interactable = isFocused;
            cg.blocksRaycasts = isFocused;
        }

        CenterOnItem(items[currentIndex]);
    }

    void CenterOnItem(RectTransform target)
    {
        Vector2 targetPos = contentPanel.anchoredPosition;
        float panelWidth = contentPanel.rect.width;
        float itemWidth = target.rect.width;

        // Move the panel so the target is centered
        float targetX = -target.localPosition.x + (panelWidth / 2) - (itemWidth / 2);
        contentPanel.anchoredPosition = new Vector2(targetX, contentPanel.anchoredPosition.y);
    }
}
