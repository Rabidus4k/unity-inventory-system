using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragCursor : MonoBehaviour
{
    [SerializeField] private Canvas rootCanvas;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countLabel;

    private RectTransform _rt;

    private void Awake()
    {
        _rt = (RectTransform)transform;
        Hide();
    }

    public void Show(Sprite sprite, int count)
    {
        icon.sprite = sprite;
        icon.enabled = sprite != null;
        countLabel.text = count > 1 ? count.ToString() : string.Empty;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetScreenPosition(Vector2 screenPosition)
    {
        if (rootCanvas == null || _rt == null) return;
        Vector2 pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                (RectTransform)rootCanvas.transform,
                screenPosition,
                rootCanvas.worldCamera,
                out pos
            );

        _rt.anchoredPosition = pos;
    }
}