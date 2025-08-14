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
    public float ScaledHeight =>  Height * (Settings.GlobalScale + Camera.Zoom);
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
        if (!DrawBoundingBox) { return; }

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

        // Obere Seite
        data.CollideTop = 
            vY < other.Y + other.Height &&
            vY >= other.Y &&
            vX + Width > other.X &&
            vX < other.X + other.Width;

        // Untere Seite
        data.CollideBottom =
            vY + Height > other.Y &&
            vY + Height <= other.Y + other.Height &&
            vX + Width > other.X &&
            vX < other.X + other.Width;

        // Linke Seite
        data.CollideLeft =
            vX < other.X + other.Width &&
            vX >= other.X &&
            vY + Height > other.Y &&
            vY < other.Y + other.Height;

        // Rechte Seite
        data.CollideRight =
            vX + Width > other.X &&
            vX + Width <= other.X + other.Width &&
            vY + Height > other.Y &&
            vY < other.Y + other.Height;


        return data;
    }
}