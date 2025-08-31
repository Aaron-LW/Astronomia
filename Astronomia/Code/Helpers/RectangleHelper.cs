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

    public static void DrawRectangle(SpriteBatch spriteBatch, Vector2 start, float width, float height, Color? color = null, float thickness = 3f)
    {
        start = Camera.WorldToScreen(start);
        width = width * (Settings.GlobalScale * Camera.Zoom);
        height = height * (Settings.GlobalScale * Camera.Zoom);
        if (color == null)
        {
            color = Color.Yellow;
        }

        spriteBatch.DrawLine(start, start + new Vector2(width, 0), (Color)color, thickness);
        spriteBatch.DrawLine(start, start + new Vector2(0, height), (Color)color, thickness);
        spriteBatch.DrawLine(start + new Vector2(width, 0), start + new Vector2(width, height), (Color)color, thickness);
        spriteBatch.DrawLine(start + new Vector2(0, height), start + new Vector2(width, height), (Color)color, thickness);
    }

    public static void DrawRectangleScreen(SpriteBatch spriteBatch, Vector2 start, float width, float height, Color? color = null, float thickness = 3f)
    {
        if (color == null)
        {
            color = Color.Yellow;
        }

        spriteBatch.DrawLine(start, start + new Vector2(width, 0), (Color)color, thickness);
        spriteBatch.DrawLine(start, start + new Vector2(0, height), (Color)color, thickness);
        spriteBatch.DrawLine(start + new Vector2(width, 0), start + new Vector2(width, height), (Color)color, thickness);
        spriteBatch.DrawLine(start + new Vector2(0, height), start + new Vector2(width, height), (Color)color, thickness);
    }
}