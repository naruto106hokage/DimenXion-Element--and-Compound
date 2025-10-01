using UnityEngine;
using UnityEngine.UI;

public class ScrollCenterDebugger : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;

    private RectTransform content;
    private RectTransform viewport;
    private GameObject lastCentered;

    private void Start()
    {
        if (scrollRect == null)
        {
            Debug.LogError("ScrollRect is not assigned.");
            enabled = false;
            return;
        }

        content = scrollRect.content;
        viewport = scrollRect.viewport;
    }

    private void Update()
    {
        GameObject centered = GetCenteredItem();
        if (centered != null && centered != lastCentered)
        {
            Debug.Log("Centered GameObject: " + centered.name);
            lastCentered = centered;
        }
    }

    private GameObject GetCenteredItem()
    {
        float closestDistance = float.MaxValue;
        GameObject closest = null;

        Vector3 viewportCenter = viewport.TransformPoint(new Vector3(viewport.rect.width / 2f, 0, 0));

        foreach (RectTransform child in content)
        {
            Vector3 childWorldPos = child.TransformPoint(child.rect.center);
            float distance = Mathf.Abs(viewportCenter.x - childWorldPos.x);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = child.gameObject;
            }
        }

        return closest;
    }
}
