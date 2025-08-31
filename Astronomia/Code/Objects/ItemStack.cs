public class ItemStack
{
    public int Amount;
    public Item Item;

    public ItemStack(Item item, int amount)
    {
        Amount = amount;
        Item = item;
    }
}