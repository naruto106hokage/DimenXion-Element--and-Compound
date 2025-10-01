using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollSnapFade : MonoBehaviour
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
        // Start from second element if exists
        currentIndex = content.childCount > 1 ? 1 : 0;

        leftButton.onClick.AddListener(OnLeftButtonClick);
        rightButton.onClick.AddListener(OnRightButtonClick);

        CenterAndFade(currentIndex);
    }

    public void OnRightButtonClick()
    {
        if (currentIndex < content.childCount - 1)
        {
            currentIndex++;
            CenterAndFade(currentIndex);
        }
    }

    public void OnLeftButtonClick()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            CenterAndFade(currentIndex);
        }
    }

    void CenterAndFade(int index)
    {
        if (scrollCoroutine != null)
            StopCoroutine(scrollCoroutine);

        scrollCoroutine = StartCoroutine(SmoothScrollTo(index));
        UpdateFading(index);
    }

    IEnumerator SmoothScrollTo(int index)
    {
        RectTransform target = content.GetChild(index) as RectTransform;

        float contentWidth = content.rect.width;
        float viewportWidth = viewport.rect.width;

        float targetCenterX = -target.localPosition.x - (target.rect.width / 2f);
        float targetNormalizedPos = Mathf.Clamp01(
            (targetCenterX + contentWidth / 2 - viewportWidth / 2) / (contentWidth - viewportWidth)
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

    void UpdateFading(int focusedIndex)
    {
        for (int i = 0; i < content.childCount; i++)
        {
            GameObject item = content.GetChild(i).gameObject;

            CanvasGroup cg = item.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = item.AddComponent<CanvasGroup>();

            bool isFocused = (i == focusedIndex);
            cg.alpha = isFocused ? 1f : 0.3f;
            cg.blocksRaycasts = isFocused;
            cg.interactable = isFocused;
        }
    }
}
