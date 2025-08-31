using Microsoft.Xna.Framework.Graphics;

public class Item
{
    public string Name;
    public Texture2D Texture;

    public Item(string name, Texture2D texture)
    {
        Name = name;
        Texture = texture;
    }
}