using System;

public interface IInventory
{
    int SlotCount { get; }
    InventorySlot GetSlot(int index);

    int Add(ItemDefinition def, int quantity, bool ignoreStackable = false);
    void MoveOrMerge(int fromIndex, int toIndex);
    void RemoveAt(int index, int quantity);

    event Action<int> SlotChanged;
}