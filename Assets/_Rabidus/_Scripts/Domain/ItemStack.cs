public class ItemStack
{
    public ItemDefinition Definition { get; private set; }
    public int Quantity { get; set; }

    public ItemStack(ItemDefinition def, int quantity)
    {
        Definition = def;
        Quantity = quantity;
    }
}