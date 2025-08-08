using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Registries.TextureRegistry;
using System;
using MonoGame.Extended;

public static class GridSystem
{
    public static List<Tile> Tiles = new List<Tile>();

    private static int _tileSize = 16;
    private static float _scaledTileSize = _tileSize * Settings.GlobalScale;

    private static float _previousScrollWheelValue = 1;

    private static float _currentTileRotation = 0;
    private static int _tileIndex = 0;

    public static void Start()
    {
        for (float i = 0; i < 50 * _scaledTileSize; i += _scaledTileSize)
        {
            PlaceTile(new Vector2(i, 300), TextureRegistry.Grass);
        }
    }

    public static void Update()
    {
        float scrollWheelValue = Mouse.GetState().ScrollWheelValue;
        Camera.ZoomAt(Input.GetMousePosition(), (scrollWheelValue - _previousScrollWheelValue) / (240 / Camera.Zoom * 2));

        if (Input.IsLeftMousePressed() || Input.IsLeftMouseDown() && Input.IsKeyDown(Keys.LeftShift))
        {
            PlaceTile(Input.GetMousePosition(), TextureRegistry.TileTextures[_tileIndex], _currentTileRotation);
        }

        if (Input.IsRightMousePressed() || Input.IsRightMouseDown() && Input.IsKeyDown(Keys.LeftShift))
        {
            RemoveTile(Input.GetMousePosition());
        }

        _previousScrollWheelValue = scrollWheelValue;

        if (Input.IsKeyPressed(Keys.Up))
        {
            if (_tileIndex < TextureRegistry.TileTextures.Count - 1)
            {
                _tileIndex += 1;
            }
        }

        if (Input.IsKeyPressed(Keys.Down))
        {
            if (_tileIndex > 0)
            {
                _tileIndex -= 1;
            }
        }
        
        if (Input.IsKeyPressed(Keys.R))
        {
            _currentTileRotation += 90 * ((float)Math.PI / 180);
        }
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        Vector2 mousePosition = Input.GetMousePosition();

        foreach (Tile tile in Tiles)
        {
            tile.Draw(spriteBatch);
        }

        spriteBatch.Draw(TextureRegistry.Selector, (GetGridPosition(mousePosition) - Camera.GetPosition()) * Camera.Zoom, null, Color.White, 0f, new Vector2(), Settings.GlobalScale * Camera.Zoom, SpriteEffects.None, 0f);
    }

    public static Vector2 GetGridPosition(Vector2 position)
    {
        Vector2 worldPos = Camera.GetPosition() + position / Camera.Zoom;
        return new Vector2(
            (float)Math.Floor(worldPos.X / _scaledTileSize) * _scaledTileSize,
            (float)Math.Floor(worldPos.Y / _scaledTileSize) * _scaledTileSize
        );
    }

    private static void PlaceTile(Vector2 screenPosition, Texture2D texture, float rotation = 0)
    {
        Vector2 gridPos = GetGridPosition(screenPosition);
        if (IsTileAtPosition(screenPosition)) { return; }

        Tiles.Add(new Tile(gridPos, texture, rotation));
    }

    private static void RemoveTile(Vector2 position)
    {
        int? index;
        index = GetIndexOfTileAtPosition(position);

        if (index != null)
        {
            Tiles.RemoveAt((int)index);
        }
    }

    private static int? GetIndexOfTileAtPosition(Vector2 position)
    {
        Vector2 gridPosition = GetGridPosition(position);
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (Tiles[i].Position == gridPosition)
            {
                return i;
            }
        }

        return null;
    }

    private static Tile GetTileAtPosition(Vector2 position)
    {
        Vector2 gridPosition = GetGridPosition(position);
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (Tiles[i].Position == gridPosition)
            {
                return Tiles[i];
            }
        }

        return null;
    }

    private static bool IsTileAtPosition(Vector2 position)
    {
        Vector2 gridPosition = GetGridPosition(position);
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (Tiles[i].Position == gridPosition)
            {
                return true;
            }
        }

        return false;
    }
}