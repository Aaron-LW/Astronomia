using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

public static class RectangleHelper
{
    public static void DrawRectangle(SpriteBatch spriteBatch, Vector2 start, Vector2 end, float thickness = 3f)
    {
        start = (start - Camera.GetPosition()) * Camera.Zoom;
        end = (end - Camera.GetPosition()) * Camera.Zoom;
        spriteBatch.DrawLine(start, new Vector2(start.X + (end.X - start.X), start.Y), Color.Yellow, thickness);
        spriteBatch.DrawLine(start, new Vector2(start.X, start.Y + (end.Y - start.Y)), Color.Yellow, thickness);
        spriteBatch.DrawLine(start + new Vector2(end.X - start.X, 0f), end, Color.Yellow, thickness);
        spriteBatch.DrawLine(start + new Vector2(0, end.Y - start.Y), end, Color.Yellow, thickness);
    }
}