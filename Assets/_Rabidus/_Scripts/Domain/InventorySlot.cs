public class InventorySlot
{
    private ItemStack _stack;

    public bool IsEmpty => _stack == null || _stack.Quantity <= 0;
    public ItemStack Stack => _stack;

    public void Set(ItemStack stack)
    {
        _stack = stack;
        if (_stack != null && _stack.Quantity <= 0)
            _stack = null;
    }

    public ItemStack Take()
    {
        var s = _stack;
        _stack = null;
        return s;
    }

    public void Clear() => _stack = null;
}