using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Registries.TextureRegistry;
using System;
public static class GridSystem
{
    public static List<Tile> Tiles = new List<Tile>();

    private static int _tileSize = 16;
    private static float _scaledTileSize = _tileSize * Settings.GlobalScale;

    public static void Update()
    {
        if (Input.IsLeftMousePressed())
        {
            Vector2 mousePosition = Input.GetMousePosition();
            Tiles.Add(new Tile(GetGridPosition(mousePosition), TextureRegistry.Grass));
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


    public static Vector2 GetGridPosition(Vector2 position)
    {
        //Calvin Lösung
        int x = (int)(position.X / _scaledTileSize) * _tileSize;
        int y = (int)(position.Y / _scaledTileSize) * _tileSize;

        //richtige Lösung
        float xx = (int)(position.X / _scaledTileSize) * _scaledTileSize;
        float yy = (int)(position.Y / _scaledTileSize) * _scaledTileSize;

        //cleane lösung
        return new Vector2(
            (int)(position.X / _scaledTileSize) * _scaledTileSize,
            (int)(position.Y / _scaledTileSize) * _scaledTileSize
        );
    }
}