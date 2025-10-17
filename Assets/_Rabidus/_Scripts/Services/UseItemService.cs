public class UseItemService : IItemUseService
{
    public bool CanUse(ItemStack stack)
    {
        if (stack == null || stack.Definition == null) return false;

        return stack.Definition.ItemType == ItemType.Armor;
    }
}