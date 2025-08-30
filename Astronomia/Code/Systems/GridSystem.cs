using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Registries.TextureRegistry;
using System;
using MonoGame.Extended;

public static class GridSystem
{
    public static Dictionary<Vector2, Tile> Tiles = new Dictionary<Vector2, Tile>();
    public static int DrawnTiles = 0;
    public static bool LevelEditor = true;

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
    private static Queue<(Vector2 position, Texture2D texture, float rotation)> _tileQueue = new Queue<(Vector2, Texture2D, float)>();
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
            if (LevelEditor)
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

        }
        else if (LevelEditor)
        {
            if (_massPlace)
            {
                MassPlaceStepWise();
                _massPlace = false;
            }
        }

        if (Input.IsRightMousePressed() || Input.IsRightMouseDown() && Input.IsKeyDown(Keys.LeftShift))
        {
            if (LevelEditor)
            {
                RemoveTile(Input.GetMousePosition());
            }
        }

        _previousScrollWheelValue = scrollWheelValue;

        if (Input.IsKeyPressed(Keys.R) && LevelEditor)
        {
            _currentTileRotation += 90 * ((float)Math.PI / 180);
        }

        if (Input.IsKeyPressed(Keys.Tab) && LevelEditor)
        {
            _showRectangle = !_showRectangle;
            _rectanglePosition = Input.GetMousePosition();
        }

        if (Input.IsLeftMousePressed() && LevelEditor)
        {
            int? index = GetRectangleIndexAtMousePosition();
            if (index != null)
            {
                _tileIndex = (int)index;
                _showRectangle = false;
            }
        }

        if (Input.IsKeyPressed(Keys.F))
        {
            LevelEditor = !LevelEditor;
        }
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        Vector2 mousePosition = Input.GetMousePosition();

        Vector2 start = new Vector2(0, 0);
        Vector2 end = new Vector2(Camera.Viewport.Width, Camera.Viewport.Height) + (new Vector2(_scaledTileSize, _scaledTileSize) * Camera.Zoom);

        if (DebugMenu.ViewportEdges)
        {
            spriteBatch.DrawPoint(start, Color.Green, 30f);
            spriteBatch.DrawPoint(end - new Vector2(_scaledTileSize, _scaledTileSize) * Camera.Zoom, Color.Red, 30f);
        }

        DrawnTiles = 0;
        for (float x = start.X; x <= end.X; x += _scaledTileSize * Camera.Zoom) 
        {
            for (float y = start.Y; y <= end.Y; y += _scaledTileSize * Camera.Zoom)
            {
                if (DebugMenu.CameraTileSamplePoints)
                {
                    spriteBatch.DrawPoint((IndexToPosition(GetTileIndex(new Vector2(x, y))) - Camera.Position) * Camera.Zoom, Color.Yellow, 3f);
                }

                if (Tiles.TryGetValue(GetTileIndex(new Vector2(x, y)), out Tile tile))
                {
                    tile.Draw(spriteBatch, (IndexToPosition(GetTileIndex(new Vector2(x, y))) - Camera.Position) * Camera.Zoom);
                }
            }
        }

        spriteBatch.DrawString(Settings.Font, "Drawn tiles: " + DrawnTiles.ToString(), new Vector2(8, 100), Color.White, 0f, new Vector2(), 0.1f, SpriteEffects.None, 0f);

        if (LevelEditor)
        {
            spriteBatch.Draw(TextureRegistry.TileTextures[_tileIndex], RotationHelper.GetRotatedPosition(mousePosition + new Vector2(15, 20), new SizeF(TextureRegistry.TileTextures[_tileIndex].Width, TextureRegistry.TileTextures[_tileIndex].Height), _currentTileRotation, 2f), null, Color.White, _currentTileRotation, new Vector2(), 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(TextureRegistry.Selector, (GetGridPosition(mousePosition) - Camera.GetPosition()) * Camera.Zoom, null, Color.White, 0f, new Vector2(), Settings.GlobalScale * Camera.Zoom, SpriteEffects.None, 0f);
        }

        if (_showRectangle && LevelEditor)
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

        if (_massPlace && LevelEditor)
        {
            RectangleHelper.DrawRectangle(spriteBatch, GetGridPosition(_massPlaceStartPostition, GetOppositeTileEdgeFromMousePosition(_massPlaceCenterPosition), true), GetGridPosition(Input.GetMousePosition(), GetTileEdgeFromMousePosition(_massPlaceCenterPosition)));
        }

        spriteBatch.DrawString(Settings.Font, "Tiles: " + Tiles.Count, new Vector2(8, 50), Color.White, 0f, new Vector2(), 0.1f, SpriteEffects.None, 0f);
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

    public static Tile[] GetTilesInArea(Vector2 start, Vector2 end, SpriteBatch spriteBatch = null)
    {
        List<Tile> tiles = new List<Tile>();
        if (spriteBatch != null && DebugMenu.ShowCollisionCheckArea) { RectangleHelper.DrawRectangle(spriteBatch, start, end, 3f); }
        start = (start - Camera.GetPosition()) * Camera.Zoom;
        end = (end - Camera.GetPosition()) * Camera.Zoom;

        if (spriteBatch != null && DebugMenu.ShowCollisionCheckArea)
        {
            spriteBatch.DrawPoint(start, Color.Red, 10f);
            spriteBatch.DrawPoint(end, Color.Red, 10f);
        }

        for (float x = start.X; x <= end.X; x += _scaledTileSize * Camera.Zoom)
        {
            for (float y = start.Y; y <= end.Y; y += _scaledTileSize * Camera.Zoom)
            {
                if (Tiles.TryGetValue(GetTileIndex(new Vector2(x, y)), out Tile tile))
                {
                    tiles.Add(tile);
                    tile.BoundingBox.UpdatePosition(IndexToPosition(GetTileIndex(new Vector2(x, y))));
                }
            }
        }

        return tiles.ToArray();
    }

    private static void PlaceTile(Vector2 screenPosition, Texture2D texture, float rotation = 0)
    {
        Vector2 index = GetTileIndex(screenPosition);
        Tiles[index] = new Tile(texture, rotation);
    }

    private static void PlaceTileInWorld(Vector2 worldPosition, Texture2D texture, float rotation = 0)
    {
        Vector2 index = GetTileIndexWorld(worldPosition);
        Tiles[index] = new Tile(texture, rotation);
    }

    private static void RemoveTile(Vector2 screenPosition)
    {
        Vector2 index = GetTileIndex(screenPosition);
        if (Tiles.TryGetValue(index, out Tile tile)) { Tiles.Remove(index); }
    }

    private static Vector2 IndexToPosition(Vector2 index)
    {
        return new Vector2(index.X * _tileSize, index.Y * _tileSize);
    }

    private static Vector2 GetTileIndex(Vector2 screenPosition)
    {
        Vector2 worldPos = Camera.ScreenToWorld(screenPosition);

        return new Vector2((int)Math.Floor(worldPos.X / _scaledTileSize), (int)Math.Floor(worldPos.Y / _scaledTileSize));
    }

    private static Vector2 GetTileIndexWorld(Vector2 worldPosition)
    {
        return new Vector2((int)Math.Floor(worldPosition.X / _scaledTileSize), (int)Math.Floor(worldPosition.Y / _scaledTileSize));
    }

    //private static int? GetIndexOfTileAtPosition(Vector2 position)
    //{
    //    Vector2 gridPosition = GetGridPosition(position);
    //    for (int i = 0; i < Tiles.Count; i++)
    //    {
    //        if (Tiles[i].Position == gridPosition)
    //        {
    //            return i;
    //        }
    //    }
    //
    //    return null;
    //}

    //(private static Tile GetTileAtPosition(Vector2 position)
    //{
        //Vector2 gridPosition = GetGridPosition(position);
        //for (int i = 0; i < Tiles.Count; i++)
        //{
        //    if (Tiles[i].Position == gridPosition)
        //    {
        //        return Tiles[i];
        //    }
        //}

        //return null;
    //}

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
                    _tileQueue.Enqueue((new Vector2((float)Math.Round(x), (float)Math.Round(y)), TextureRegistry.TileTextures[_tileIndex], _currentTileRotation));

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
                    _tileQueue.Enqueue((new Vector2((float)Math.Round(x), (float)Math.Round(y)), TextureRegistry.TileTextures[_tileIndex], _currentTileRotation));

                    x -= _scaledTileSize;
                    if (x < endX)
                    {
                        x = _massPlaceStartPostition.X + 1;
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
                    _tileQueue.Enqueue((new Vector2((float)Math.Round(x), (float)Math.Round(y)), TextureRegistry.TileTextures[_tileIndex], _currentTileRotation));

                    x -= _scaledTileSize;
                    if (x < endX)
                    {
                        x = _massPlaceStartPostition.X + 1;
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
                    _tileQueue.Enqueue((new Vector2((float)Math.Round(x), (float)Math.Round(y)), TextureRegistry.TileTextures[_tileIndex], _currentTileRotation));

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
            if (_tileQueue.Count == 0) { return; }
            var (position, texture, rotation) = _tileQueue.Dequeue();
            PlaceTileInWorld(position, texture, rotation);
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