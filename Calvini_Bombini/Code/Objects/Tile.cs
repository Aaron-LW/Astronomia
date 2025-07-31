using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Tile
{
    public Vector2 Position;
    public Texture2D Texture;

    public Tile(Vector2 position, Texture2D texture)
    {
        Position = position;
        Texture = texture;
    }
}