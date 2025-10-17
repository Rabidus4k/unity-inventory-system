using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform itemRoot;
    [SerializeField] private Image selectedFrame;

    public int Index { get; private set; }
    public ItemView ItemView { get; private set; }

    private InventoryView _owner;

    public void Init(int index, InventoryView owner)
    {
        Index = index;
        _owner = owner;
    }

    public void BindItemView(ItemView itemView)
    {
        ItemView = itemView;
        if (itemView != null)
            itemView.transform.SetParent(itemRoot, false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        var item = eventData.pointerDrag != null ? eventData.pointerDrag.GetComponent<ItemView>() : null;
        if (item == null) return;
        item.MarkDroppedOnSlot();
        _owner.RaiseDropOnSlot(item.SlotIndex, Index);
    }

    public void OnPointerClick(PointerEventData _) => _owner.RaiseSlotClick(Index);

    public void SetSelected(bool value)
    {
        if (selectedFrame) selectedFrame.enabled = value;
    }
}