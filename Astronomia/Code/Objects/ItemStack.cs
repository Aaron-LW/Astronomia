using Microsoft.Xna.Framework;

public class ItemStack
{
    public int Amount;
    public Item Item;
    public float ScaleModifier = Settings.ItemStackDefaultScaleModifier;
    public float X;
    public float Y;
    public Vector2 Position
    {
        get
        {
            return new Vector2(X, Y);
        }
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    public ItemStack(Item item, int amount)
    {
        Amount = amount;
        Item = item;
    }
}