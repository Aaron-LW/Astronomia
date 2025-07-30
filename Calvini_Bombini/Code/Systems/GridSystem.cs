using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Registries.TextureRegistry;
using System;
public static class GridSystem
{
    public static List<Tile> Tiles = new List<Tile>();

    public static void Update()
    {
        if (Input.IsLeftMousePressed())
        {
            Tiles.Add(new Tile(Input.GetMousePosition(), TextureRegistry.Grass));
        }
    }

    //[Membermodifizierer (public/private) [returntype (void)] [name]()]
    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (Tile tile in Tiles)
        {
            //spriteBatch.Draw(tile.Texture, new Vector2(tile.Position.X, tile.Position.Y), Color.White);
            spriteBatch.Draw(tile.Texture, tile.Position, null, Color.White, 0f, Vector2.Zero, Settings.GlobalScale, SpriteEffects.None, 0f);
        }
    }
}

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