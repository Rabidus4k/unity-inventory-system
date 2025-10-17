using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ContextMenuManager : Singleton<ContextMenuManager>
{
    [SerializeField] private RectTransform _root;
    [SerializeField] private UIContextMenu _menu;

    private CanvasGroup _cg;

    protected override void Awake()
    {
        base.Awake();
        _cg = GetComponent<CanvasGroup>();
    }

    public void ShowContextMenu(ItemDefinition item)
    {
        _cg.alpha = 1;

        _menu.Initialize(item);

        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _root,
            Input.mousePosition,                  
            null,
            out localPoint);

        _menu.Root.anchoredPosition = localPoint;
    }

    public void HideContextMenu()
    {
        _cg.alpha = 0;
    }
}
