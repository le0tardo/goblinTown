using UnityEngine;
using TMPro;

public class HoverUI : MonoBehaviour
{
    [SerializeField] HoverManager hover;
    [SerializeField] TextMeshProUGUI text;

    RectTransform rectTransform;
    Canvas canvas;

    public Vector2 offset = new Vector2(20f, 0f); // pixels to the right
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        text.text = hover.hoverString;
        TrackMouse();
    }

    void TrackMouse()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out mousePos
        );

        rectTransform.localPosition = mousePos+offset;
    }
}
