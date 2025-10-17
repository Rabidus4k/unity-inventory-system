using System;
using UnityEngine;
public class Inventory : IInventory
{
    private readonly InventorySlot[] _slots;
    public event Action<int> SlotChanged;
    public int SlotCount => _slots.Length;

    public Inventory(int slotCount)
    {
        if (slotCount <= 0) throw new ArgumentOutOfRangeException(nameof(slotCount));

        _slots = new InventorySlot[slotCount];

        for (int i = 0; i < slotCount; i++) 
            _slots[i] = new InventorySlot();
    }

    public InventorySlot GetSlot(int index)
    {
        if (index < 0 || index >= _slots.Length) throw new ArgumentOutOfRangeException(nameof(index));

        return _slots[index];
    }

    public int Add(ItemDefinition def, int quantity, bool ignoreStackable = false)
    {
        if (def == null) throw new ArgumentNullException(nameof(def));
        if (quantity <= 0) return 0;

        int remaining = quantity;

        if (def.Stackable && !ignoreStackable)
        {
            for (int i = 0; i < _slots.Length && remaining > 0; i++)
            {
                var s = _slots[i];
                if (!s.IsEmpty && s.Stack.Definition == def)
                {
                    int canTake = def.MaxStack - s.Stack.Quantity;
                    if (canTake > 0)
                    {
                        int add = Mathf.Min(canTake, remaining);
                        s.Stack.Quantity += add;
                        remaining -= add;
                        SlotChanged?.Invoke(i);
                    }
                }
            }
        }

        for (int i = 0; i < _slots.Length && remaining > 0; i++)
        {
            var s = _slots[i];
            if (s.IsEmpty)
            {
                int put = def.Stackable ? Mathf.Min(def.MaxStack, remaining) : 1;
                s.Set(new ItemStack(def, put));
                remaining -= put;
                SlotChanged?.Invoke(i);
            }
        }

        return remaining;
    }

    public void MoveOrMerge(int fromIndex, int toIndex)
    {
        if (fromIndex == toIndex)
        {
            SlotChanged?.Invoke(fromIndex);
            return;
        }

        var from = GetSlot(fromIndex);
        var to = GetSlot(toIndex);
        if (from.IsEmpty) return;

        if (
            !to.IsEmpty &&
            to.Stack.Definition == from.Stack.Definition &&
            to.Stack.Definition.Stackable
            )
        {
            int canTake = to.Stack.Definition.MaxStack - to.Stack.Quantity;
            if (canTake > 0)
            {
                int move = Mathf.Min(canTake, from.Stack.Quantity);
                to.Stack.Quantity += move;
                from.Stack.Quantity -= move;
                if (from.Stack.Quantity <= 0) from.Clear();
                SlotChanged?.Invoke(fromIndex);
                SlotChanged?.Invoke(toIndex);
                return;
            }
        }

        var temp = to.Take();
        to.Set(from.Take());
        from.Set(temp);
        SlotChanged?.Invoke(fromIndex);
        SlotChanged?.Invoke(toIndex);
    }

    public void RemoveAt(int index, int quantity)
    {
        var slot = GetSlot(index);
        if (slot.IsEmpty || quantity <= 0) return;

        slot.Stack.Quantity -= quantity;
        if (slot.Stack.Quantity <= 0) slot.Clear();
        SlotChanged?.Invoke(index);
    }
}