using Microsoft.Xna.Framework;

public static class CoordinateHelper
{
    public static Vector2 ScreenToWorldPosition(Vector2 screenPosition)
    {
        return Camera.GetPosition() + screenPosition / Camera.Zoom;
    }
}