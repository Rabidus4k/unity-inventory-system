public class DeteleItemService : IItemUseService
{
    public bool CanUse(ItemStack stack)
    {
        if (stack == null || stack.Definition == null) return false;
        return true;
    }
}