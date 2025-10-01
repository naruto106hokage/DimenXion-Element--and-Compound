using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FocusManager : MonoBehaviour
{
    [Header("Assign the parent container of the items")]
    public RectTransform parentContainer;

    [Header("Scroll Buttons")]
    public Button leftButton;
    public Button rightButton;

    [Range(0f, 1f)]
    public float unfocusedAlpha = 0.5f;

    private int currentIndex = 1; // start from second
    private RectTransform viewport;

    void Start()
    {
        if (parentContainer == null)
        {
            Debug.LogWarning("Parent container not assigned in FocusManager.");
            return;
        }

        viewport = parentContainer.parent.GetComponent<RectTransform>();

        // Apply alpha to all children
        int childCount = parentContainer.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var child = parentContainer.GetChild(i);
            CanvasGroup cg = child.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = child.gameObject.AddComponent<CanvasGroup>();
            cg.alpha = unfocusedAlpha;
        }

        // Hook buttons
        if (leftButton != null)
            leftButton.onClick.AddListener(OnLeftPressed);
        if (rightButton != null)
            rightButton.onClick.AddListener(OnRightPressed);

        if (childCount > 3)
            StartCoroutine(CenterChildAfterLayout(currentIndex));
    }

    private void OnLeftPressed()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            CenterChild(currentIndex);
        }
    }

    private void OnRightPressed()
    {
        if (currentIndex < parentContainer.childCount - 1)
        {
            currentIndex++;
            CenterChild(currentIndex);
        }
    }

    private IEnumerator CenterChildAfterLayout(int index)
    {
        yield return new WaitForEndOfFrame();
        CenterChild(index);
    }

    private void CenterChild(int index)
    {
        if (index < 0 || index >= parentContainer.childCount)
            return;

        RectTransform child = parentContainer.GetChild(index) as RectTransform;

        float childPos = child.localPosition.x;
        float childWidth = child.rect.width * child.localScale.x;
        float viewportWidth = viewport.rect.width;

        // Calculate offset to center this child
        float offsetX = -childPos - (childWidth / 2f) + (viewportWidth / 2f);

        // Apply offset to the container
        Vector2 newAnchoredPos = parentContainer.anchoredPosition;
        newAnchoredPos.x = offsetX;
        parentContainer.anchoredPosition = newAnchoredPos;
    }
}
