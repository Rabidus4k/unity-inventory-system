using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countLabel;

    private DragCursor _dragCursor;
    private InventoryView _owner;
    private CanvasGroup _cg;
    public int SlotIndex { get; private set; }
    public bool HasItem { get; private set; }

    private bool _droppedOnSlot;

    public void Init(int slotIndex, InventoryView owner, DragCursor dragCursor)
    {
        SlotIndex = slotIndex;
        _owner = owner;
        _dragCursor = dragCursor;
        _cg = GetComponent<CanvasGroup>();

        Render(new SlotRenderData());
    }

    public void Render(SlotRenderData data)
    {
        HasItem = data.HasItem;
        icon.enabled = data.HasItem && data.Icon != null;
        icon.sprite = data.Icon;
        countLabel.text = data.HasItem && data.IsStackable ? data.Count.ToString() : string.Empty;
        _cg.alpha = 1;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!HasItem) return;
        _droppedOnSlot = false;
        _owner.RaiseBeginDrag(SlotIndex);
        _dragCursor.Show(icon.sprite, string.IsNullOrEmpty(countLabel.text) ? 1 : int.Parse(countLabel.text));
        _dragCursor.SetScreenPosition(eventData.position);
        _cg.alpha = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!HasItem) return;
        _dragCursor.SetScreenPosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragCursor.Hide();
        
        if (!HasItem) return;

        if (!_droppedOnSlot)
            _owner.RaiseDropOutside(SlotIndex);
    }

    public void MarkDroppedOnSlot()
    {
        _droppedOnSlot = true;
    }
}