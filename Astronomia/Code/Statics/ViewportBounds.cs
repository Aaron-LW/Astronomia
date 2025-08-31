using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

public static class ViewportBounds
{
    public static float Width;
    public static float Height;
    public static Vector2 Bounds { get { return new Vector2(Width, Height); } }

    public static void Update(GraphicsDevice graphicsDevice)
    {
        Width = graphicsDevice.Viewport.Width;
        Height = graphicsDevice.Viewport.Height;
    }
}