using Microsoft.Xna.Framework.Graphics;

public class Item
{
    public string Name;
    public Texture2D Texture;
    public int MaxStackSize;

    public Item(string name, Texture2D texture, int maxStackSize = 99)
    {
        Name = name;
        Texture = texture;
        MaxStackSize = maxStackSize;
    }
}