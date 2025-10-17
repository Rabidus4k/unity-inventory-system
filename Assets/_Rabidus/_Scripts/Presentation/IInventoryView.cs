using System;

public interface IInventoryView
{
    void BuildSlots(int slotCount);
    void RenderSlot(int index, SlotRenderData data);
    void SetSelected(int index);
    void SetButtonsState(bool canUse, bool canDelete);

    event Action<int> OnBeginDrag;
    event Action<int, int> OnDropOnSlot;
    event Action<int> OnDropOutside;

    event Action<int> OnSlotClick;
    event Action OnUseClick;
    event Action OnDeleteClick;
}