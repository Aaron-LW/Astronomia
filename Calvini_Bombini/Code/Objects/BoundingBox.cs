using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

public class BoundingBox
{
    public float X;
    public float Y;
    public float Width;
    public float ScaledWidth => Width * (Settings.GlobalScale * Camera.Zoom);
    public float Height;
    public float ScaledHeight => Height * (Settings.GlobalScale + Camera.Zoom);
    public Vector2 Position => new Vector2(X, Y);
    public bool DrawBoundingBox;

    public BoundingBox(float width, float height, bool drawBoundingBox = false)
    {
        Width = width;
        Height = height;
        DrawBoundingBox = drawBoundingBox;
    }

    public void UpdatePosition(Vector2 newPosition)
    {
        X = newPosition.X;
        Y = newPosition.Y;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawRectangle(new RectangleF((X - Camera.GetPosition().X) * Camera.Zoom, (Y - Camera.GetPosition().Y) * Camera.Zoom, Width * (Settings.GlobalScale * Camera.Zoom), Height * (Settings.GlobalScale * Camera.Zoom)),
                                    Color.Red, 3f * Camera.Zoom / 8);
    }

    public bool IsCollidingWith(BoundingBox other)
    {
        return X < other.X + other.Width &&
                X + Width > other.X &&
                Y < other.Y + other.Height &&
                Y + Height > other.Y;
    }

    public CollisionData WillCollideWith(BoundingBox other, Vector2 velocity)
    {
        float vX = X + (velocity.X * Time.DeltaTime);
        float vY = Y + (velocity.Y * Time.DeltaTime);
        CollisionData data = new CollisionData();

        if (vX < other.X + other.Width &&
            vX + Width > other.X &&
            vY < other.Y + other.Height &&
            vY + Height > other.Y)
        {
            if (Y + Height <= other.Y && X + Width != other.X && X != other.X + other.Width)
            {
                data.CollideBottom = true;
            }
            
            if (X + Width <= other.X && other.Y != Y + Height)
            {
                data.CollideRight = true;
            }

            if (X >= other.X + other.Width && other.Y != Y + Height)
            {
                data.CollideLeft = true;
            }

            if (Y >= other.Y + other.Height)
            {
                data.CollideTop = true;
            }
        }

        return data;
    }
}