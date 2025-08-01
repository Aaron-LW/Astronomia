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
        if (Input.IsLeftMousePressed() || Input.IsLeftMouseDown() && Input.IsKeyDown(Keys.LeftShift))
        {
            Vector2 mousePosition = Input.GetMousePosition();
            Tiles.Add(new Tile(GetGridPosition(mousePosition), TextureRegistry.Grass));
        }
    }

    //[Membermodifizierer (public/private) [returntype (void)] [name]()]
    public static void Draw(SpriteBatch spriteBatch)
    {
        Vector2 mousePosition = Input.GetMousePosition();

        foreach (Tile tile in Tiles)
        {
            //spriteBatch.Draw(tile.Texture, new Vector2(tile.Position.X, tile.Position.Y), Color.White);
            spriteBatch.Draw(tile.Texture, tile.Position - Camera.GetPosition(), null, Color.White, 0f, Vector2.Zero, Settings.GlobalScale, SpriteEffects.None, 0f);
        }
        spriteBatch.Draw(TextureRegistry.Selector, GetGridPosition(mousePosition) - Camera.GetPosition(), null, Color.White, 0f, new Vector2(), Settings.GlobalScale, SpriteEffects.None, 0f);
    }


    public static Vector2 GetGridPosition(Vector2 position)
    {
        //Calvin Lösung
        int x = (int)(position.X / _scaledTileSize) * _tileSize;
        int y = (int)(position.Y / _scaledTileSize) * _tileSize;

        //richtige Lösung
        float xx = (int)((position.X + Camera.X) / _scaledTileSize) * _scaledTileSize;
        float yy = (int)((position.Y + Camera.Y) / _scaledTileSize) * _scaledTileSize;

        //cleane lösung
        Vector2 worldPos = new Vector2(position.X, position.Y) + Camera.GetPosition();
            return new Vector2(
                (float)Math.Floor(worldPos.X / _scaledTileSize) * _scaledTileSize,
                (float)Math.Floor(worldPos.Y / _scaledTileSize) * _scaledTileSize
            );
    }
}