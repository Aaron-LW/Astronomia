using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public static class Camera
{
    public static float X;
    public static float Y;

    public static Vector2 GetPosition()
    {
        return new Vector2(X, Y);
    }

    public static void Update()
    {
        if (Input.IsKeyDown(Keys.A))
        {
            X -= Settings.CameraSpeed;
        }

        if (Input.IsKeyDown(Keys.D))
        {
            X += Settings.CameraSpeed;
        }

        if (Input.IsKeyDown(Keys.W))
        {
            Y -= Settings.CameraSpeed;
        }

        if (Input.IsKeyDown(Keys.S))
        {
            Y += Settings.CameraSpeed;
        }
    }
}