using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollSnap : MonoBehaviour
{
    [Header("References")]
    public ScrollRect scrollRect;
    public RectTransform content;
    public RectTransform viewport;
    public Button leftButton;
    public Button rightButton;
    public float scrollSpeed = 5f;

    private int currentIndex = 0;
    private Coroutine scrollCoroutine;

    void Start()
    {
        if (leftButton != null)
            leftButton.onClick.AddListener(OnLeftButtonClick);

        if (rightButton != null)
            rightButton.onClick.AddListener(OnRightButtonClick);

        if (content.childCount <= 1)
        {
            SetButtonState(leftButton, false);
            SetButtonState(rightButton, false);
        }

        // Focus on second item if available, otherwise first
        currentIndex = content.childCount > 1 ? 1 : 0;
        CenterOnItem(currentIndex);
    }

    void SetButtonState(Button button, bool state)
    {
        if (button == null) return;

        Text text = button.GetComponentInChildren<Text>();
        if (text != null)
            text.enabled = state;
    }

    public void OnRightButtonClick()
    {
        if (currentIndex < content.childCount - 1)
        {
            currentIndex++;
            CenterOnItem(currentIndex);
        }
    }

    public void OnLeftButtonClick()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            CenterOnItem(currentIndex);
        }
    }

    void CenterOnItem(int index)
    {
        if (scrollCoroutine != null)
            StopCoroutine(scrollCoroutine);

        scrollCoroutine = StartCoroutine(SmoothScrollTo(index));
        currentIndex = index;

        int reversedIndex = (content.childCount - 1) - index;

        if (content.childCount > reversedIndex)
        {
            Transform focused = content.GetChild(reversedIndex);
            Debug.Log("Currently Focused GameObject: " + focused.name);
        }

        int focusTargetIndex = reversedIndex;

        // If the first or last item is in focus, redirect focus to a neighbor
        if (reversedIndex == 0 && content.childCount > 1)
        {
            focusTargetIndex = 1;
        }
        else if (reversedIndex == content.childCount - 1 && content.childCount > 1)
        {
            focusTargetIndex = content.childCount - 2;
        }

        SetInteractionForFocusedOnly(focusTargetIndex);
        UpdateFade(focusTargetIndex);
    }

    IEnumerator SmoothScrollTo(int index)
    {
        if (content.childCount == 0) yield break;

        RectTransform target = content.GetChild(index) as RectTransform;

        float contentWidth = content.rect.width;
        float viewportWidth = viewport.rect.width;

        float targetCenterX = -target.localPosition.x - (target.rect.width / 2f);
        float targetNormalizedPos = Mathf.Clamp01(
            (targetCenterX + contentWidth / 2f - viewportWidth / 2f) / (contentWidth - viewportWidth)
        );

        float startPos = scrollRect.horizontalNormalizedPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * scrollSpeed;
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(startPos, targetNormalizedPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = targetNormalizedPos;
    }

    void UpdateFade(int focusedIndex)
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Transform item = content.GetChild(i);
            CanvasGroup cg = item.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = item.gameObject.AddComponent<CanvasGroup>();

            bool isFocused = (i == focusedIndex);
            cg.alpha = isFocused ? 1f : 0.8f;
        }
    }
    void SetInteractionForFocusedOnly(int focusedIndex)
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Transform child = content.GetChild(i);
            bool isFocused = (i == focusedIndex);

            // Enable/Disable EventTrigger
            var trigger = child.GetComponent<UnityEngine.EventSystems.EventTrigger>();
            if (trigger != null)
                trigger.enabled = isFocused;

            // Handle Button color and transition
            var button = child.GetComponent<Button>();
            if (button != null)
            {
                if (isFocused)
                {
                    // Restore default transition for focused button
                    button.transition = Selectable.Transition.ColorTint;
                    button.interactable = true;
                }
                else
                {
                    // Disable transitions for non-focused buttons
                    button.transition = Selectable.Transition.None;
                    button.interactable = false;

                    // Optionally fade the image slightly
                    var image = child.GetComponent<Image>();
                    if (image != null)
                    {
                        Color color = image.color;
                        color.a = 0.6f;
                        image.color = color;
                    }
                }
            }

            // Handle hover animation
            var hoverScaler = child.GetComponent<HoverScaler>();
            if (hoverScaler != null)
                hoverScaler.enabled = isFocused;
        }
    }

}
