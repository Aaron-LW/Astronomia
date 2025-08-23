using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Registries.TextureRegistry;
using System;
using MonoGame.Extended;
using System.Threading.Tasks;

public static class GridSystem
{
    public static List<Tile> Tiles = new List<Tile>();

    private static int _tileSize = 16;
    private static float _scaledTileSize = _tileSize * Settings.GlobalScale;
    private static float _previousScrollWheelValue = 1;
    private static float _currentTileRotation = 0;
    private static int _tileIndex = 0;
    private static bool _showRectangle;
    private static Vector2 _rectangleBounds = new Vector2(100, 100);
    private static Vector2 _rectanglePosition;
    private static int _rectangleTilesPerRow = 2;
    private static int _rectangleTileScale = 2;
    private static int _rectangleTileSize = _tileSize * _rectangleTileScale;
    private static int _rectangleTilePadding = 3;
    private static bool _massPlace = false;
    private static Vector2 _massPlaceStartPostition;
    private static Vector2 _massPlaceCenterPosition;
    private static Queue<(Vector2 position, Texture2D texture)> _tileQueue = new Queue<(Vector2, Texture2D)>();
    private static int _tilesToProcessPerFrame = 100;

    public enum TileEdge
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }


    public static void Start()
    {
        for (float i = 0; i < 50 * _scaledTileSize; i += _scaledTileSize)
        {
            PlaceTile(new Vector2(i, 300), TextureRegistry.Grass);
        }
    }

    public static void Update()
    {
        ProcessTileQueue();
        float scrollWheelValue = Mouse.GetState().ScrollWheelValue;
        Camera.ZoomAt(Input.GetMousePosition(), (scrollWheelValue - _previousScrollWheelValue) / (240 / Camera.Zoom * 2));

        if (Input.IsLeftMousePressed() || Input.IsLeftMouseDown() && Input.IsKeyDown(Keys.LeftShift) || Input.IsLeftMouseDown() && Input.IsKeyDown(Keys.LeftControl))
        {
            if (Input.IsKeyDown(Keys.LeftControl))
            {
                if (_massPlace == false)
                {
                    _massPlaceCenterPosition = GetGridPosition(Input.GetMousePosition()) + new Vector2(_scaledTileSize / 2, _scaledTileSize / 2);
                    _massPlaceStartPostition = GetGridPosition(Input.GetMousePosition());
                }
                _massPlace = true;
            }
            else
            {
                if (_massPlace)
                {
                    MassPlaceStepWise();
                    _massPlace = false;
                }
                PlaceTile(Input.GetMousePosition(), TextureRegistry.TileTextures[_tileIndex], _currentTileRotation);
            }

        }
        else
        {
            if (_massPlace)
            {
                MassPlaceStepWise();
                _massPlace = false;
            }
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

        if (Input.IsKeyPressed(Keys.Tab))
        {
            _showRectangle = !_showRectangle;
            _rectanglePosition = Input.GetMousePosition();
        }

        if (Input.IsLeftMousePressed())
        {
            int? index = GetRectangleIndexAtMousePosition();
            if (index != null)
            {
                _tileIndex = (int)index;
                _showRectangle = false;
            }
        }
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        Vector2 mousePosition = Input.GetMousePosition();

        foreach (Tile tile in Tiles)
        {
            tile.Draw(spriteBatch);
        }

        spriteBatch.Draw(TextureRegistry.TileTextures[_tileIndex], RotationHelper.GetRotatedPosition(mousePosition + new Vector2(15, 20), new SizeF(TextureRegistry.TileTextures[_tileIndex].Width, TextureRegistry.TileTextures[_tileIndex].Height), _currentTileRotation, 2f), null, Color.White, _currentTileRotation, new Vector2(), 2f, SpriteEffects.None, 0f);
        spriteBatch.Draw(TextureRegistry.Selector, (GetGridPosition(mousePosition) - Camera.GetPosition()) * Camera.Zoom, null, Color.White, 0f, new Vector2(), Settings.GlobalScale * Camera.Zoom, SpriteEffects.None, 0f);

        if (_showRectangle)
        {
            _rectangleBounds.X = _rectangleTileSize * _rectangleTilesPerRow;
            _rectangleBounds.Y = (TextureRegistry.TileTextures.Count / _rectangleTilesPerRow + 1) * _rectangleTileSize;
            spriteBatch.FillRectangle(new RectangleF(_rectanglePosition.X - _rectangleBounds.X / 2 - _rectangleTilePadding, _rectanglePosition.Y - _rectangleBounds.Y / 2 - _rectangleTilePadding, _rectangleBounds.X + _rectangleTilePadding * 3, _rectangleBounds.Y + _rectangleTilePadding * 3), Color.GhostWhite, 0);

            int x = 0;
            int y = 0;

            for (int i = 0; i < TextureRegistry.TileTextures.Count; i++)
            {
                spriteBatch.Draw(TextureRegistry.TileTextures[i], _rectanglePosition - _rectangleBounds / 2 + new Vector2((_rectangleTileSize + _rectangleTilePadding) * x, (_rectangleTileSize + _rectangleTilePadding) * y), null, Color.White, 0, new Vector2(), _rectangleTileScale, SpriteEffects.None, 0f);

                x++;
                if (x >= _rectangleTilesPerRow)
                {
                    x = 0;
                    y++;
                }
            }
        }

        if (_massPlace)
        {
            RectangleHelper.DrawRectangle(spriteBatch, GetGridPosition(_massPlaceStartPostition, GetOppositeTileEdgeFromMousePosition(_massPlaceCenterPosition), true), GetGridPosition(Input.GetMousePosition(), GetTileEdgeFromMousePosition(_massPlaceCenterPosition)));
        }
    }



    public static Vector2 GetGridPosition(Vector2 position, TileEdge edge = TileEdge.TopLeft, bool worldPosition = false)
    {
        Vector2 worldPos = position;
        if (!worldPosition)
        {
            worldPos = Camera.GetPosition() + position / Camera.Zoom;
        }

        switch (edge)
        {
            case TileEdge.TopLeft:
                return new Vector2(
                    (float)Math.Floor(worldPos.X / _scaledTileSize) * _scaledTileSize,
                    (float)Math.Floor(worldPos.Y / _scaledTileSize) * _scaledTileSize
                );

            case TileEdge.TopRight:
                return new Vector2(
                    (float)Math.Floor(worldPos.X / _scaledTileSize) * _scaledTileSize + _scaledTileSize,
                    (float)Math.Floor(worldPos.Y / _scaledTileSize) * _scaledTileSize
                );

            case TileEdge.BottomLeft:
                return new Vector2(
                    (float)Math.Floor(worldPos.X / _scaledTileSize) * _scaledTileSize,
                    (float)Math.Floor(worldPos.Y / _scaledTileSize) * _scaledTileSize + _scaledTileSize
                );

            case TileEdge.BottomRight:
                return new Vector2(
                    (float)Math.Floor(worldPos.X / _scaledTileSize) * _scaledTileSize + _scaledTileSize,
                    (float)Math.Floor(worldPos.Y / _scaledTileSize) * _scaledTileSize + _scaledTileSize
                );
        }

        //if this returns we are in big trouble because it normally should never
        return new Vector2(0, 0);
    }

    private static void PlaceTile(Vector2 screenPosition, Texture2D texture, float rotation = 0)
    {
        Vector2 gridPos = GetGridPosition(screenPosition);
        if (IsTileAtPosition(screenPosition)) { return; }

        Tiles.Add(new Tile(gridPos, texture, rotation));
    }

    private static void PlaceTileInWorld(Vector2 worldPosition, Texture2D texture, float rotation = 0)
    {
        if (IsTileAtPosition(worldPosition)) { return; }

        Tiles.Add(new Tile(worldPosition, texture, rotation));
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

    private static void DrawRectangleBoxes(SpriteBatch spriteBatch)
    {
        int x = 0;
        int y = 0;

        for (int i = 0; i < TextureRegistry.TileTextures.Count; i++)
        {
            Vector2 position = _rectanglePosition - _rectangleBounds / 2 + new Vector2((_rectangleTileSize + _rectangleTilePadding) * x, (_rectangleTileSize + _rectangleTilePadding) * y);
            spriteBatch.DrawRectangle(new RectangleF(position.X, position.Y, _rectangleTileSize, _rectangleTileSize), Color.Red, 3f);

            x++;
            if (x >= _rectangleTilesPerRow)
            {
                x = 0;
                y++;
            }
        }
    }

    private static int? GetRectangleIndexAtMousePosition()
    {
        int x = 0;
        int y = 0;
        int index = 0;

        for (int i = 0; i < TextureRegistry.TileTextures.Count; i++)
        {
            Vector2 position = _rectanglePosition - _rectangleBounds / 2 + new Vector2((_rectangleTileSize + _rectangleTilePadding) * x, (_rectangleTileSize + _rectangleTilePadding) * y);
            RectangleF rect = new RectangleF(position.X, position.Y, _rectangleTileSize, _rectangleTileSize);
            if (rect.Intersects(Input.GetMouseRectangle()))
            {
                return index;
            }

            x++;
            index++;
            if (x >= _rectangleTilesPerRow)
            {
                x = 0;
                y++;
            }
        }

        return null;
    }

    private static void MassPlaceStepWise()
    {
        bool finished = false;
        float x = _massPlaceStartPostition.X + 1;
        float y = _massPlaceStartPostition.Y + 1;

        float endX = GetGridPosition(Input.GetMousePosition()).X + 1;
        float endY = GetGridPosition(Input.GetMousePosition()).Y + 1;

        TileEdge orientation = GetTileEdgeFromMousePosition(_massPlaceCenterPosition);

        switch (orientation)
        {
            case TileEdge.BottomRight:
                while (!finished)
                {
                    _tileQueue.Enqueue((new Vector2((float)Math.Round(x), (float)Math.Round(y)), TextureRegistry.TileTextures[_tileIndex]));

                    x += _scaledTileSize;
                    if (x > endX)
                    {
                        x = _massPlaceStartPostition.X;
                        y += _scaledTileSize;
                    }

                    if (y > endY)
                    {
                        finished = true;
                    }

                }
                break;

            case TileEdge.TopLeft:
                while (!finished)
                {
                    _tileQueue.Enqueue((new Vector2((float)Math.Round(x), (float)Math.Round(y)), TextureRegistry.TileTextures[_tileIndex]));

                    x -= _scaledTileSize;
                    if (x < endX)
                    {
                        x = _massPlaceStartPostition.X;
                        y -= _scaledTileSize;
                    }

                    if (y < endY)
                    {
                        finished = true;
                    }

                }
                break;

            case TileEdge.BottomLeft:
                while (!finished)
                {
                    _tileQueue.Enqueue((new Vector2((float)Math.Round(x), (float)Math.Round(y)), TextureRegistry.TileTextures[_tileIndex]));

                    x -= _scaledTileSize;
                    if (x < endX)
                    {
                        x = _massPlaceStartPostition.X;
                        y += _scaledTileSize;
                    }

                    if (y > endY)
                    {
                        finished = true;
                    }

                }
                break;

            case TileEdge.TopRight:
                while (!finished)
                {
                    _tileQueue.Enqueue((new Vector2((float)Math.Round(x), (float)Math.Round(y)), TextureRegistry.TileTextures[_tileIndex]));

                    x += _scaledTileSize;
                    if (x > endX)
                    {
                        x = _massPlaceStartPostition.X;
                        y -= _scaledTileSize;
                    }

                    if (y < endY)
                    {
                        finished = true;
                    }

                }
                break;
        }
    }

    private static void ProcessTileQueue()
    {
        for (int i = 0; i < _tilesToProcessPerFrame; i++)
        {
            if(_tileQueue.Count == 0) { return; }
            var (position, texture) = _tileQueue.Dequeue();
            PlaceTileInWorld(position, texture);
        }
    }

    private static TileEdge GetOppositeTileEdgeFromMousePosition(Vector2 centerPosition)
    {
        Vector2 mousePosition = Input.GetMousePosition();
        mousePosition = CoordinateHelper.ScreenToWorldPosition(mousePosition);
        //spriteBatch.DrawPoint((mousePosition - Camera.GetPosition()) * Camera.Zoom, Color.Red, 30f);

        //Bottom right
        if (mousePosition.X > centerPosition.X && mousePosition.Y > centerPosition.Y)
        {
            return TileEdge.TopLeft;
        }

        //Bottom left
        if (mousePosition.X < centerPosition.X && mousePosition.Y > centerPosition.Y)
        {
            return TileEdge.TopRight;
        }

        //Top right
        if (mousePosition.X > centerPosition.X && mousePosition.Y < centerPosition.Y)
        {
            return TileEdge.BottomLeft;
        }

        //Top left
        if (mousePosition.X < centerPosition.X && mousePosition.Y < centerPosition.Y)
        {
            return TileEdge.BottomRight;
        }

        //should never happen hopefully but compiler doesn't compile without it 
        return TileEdge.TopLeft;
    }

    private static TileEdge GetTileEdgeFromMousePosition(Vector2 centerPosition)
    {
        Vector2 mousePosition = Input.GetMousePosition();
        mousePosition = CoordinateHelper.ScreenToWorldPosition(mousePosition);

        //Bottom right
        if (mousePosition.X > centerPosition.X && mousePosition.Y > centerPosition.Y)
        {
            return TileEdge.BottomRight;
        }

        //Bottom left
        if (mousePosition.X < centerPosition.X && mousePosition.Y > centerPosition.Y)
        {
            return TileEdge.BottomLeft;
        }

        //Top left
        if (mousePosition.X < centerPosition.X && mousePosition.Y < centerPosition.Y)
        {
            return TileEdge.TopLeft;
        }

        //Top right
        if (mousePosition.X > centerPosition.X && mousePosition.Y < centerPosition.Y)
        {
            return TileEdge.TopRight;
        }

        return TileEdge.TopLeft;
    }
}