using Microsoft.Xna.Framework;

public class PositionComponent : Component
{
    public PositionComponent(float x = 0, float y = 0)
    {
        X = x;
        Y = y;
    }

    public float X;
    public float Y;
    public Vector2 Position
    {
        get
        {
            return new Vector2(X, Y);
        }
    }

    public void SetPosition(Vector2 position)
    {
        X = position.X;
        Y = position.Y;
    }

    public void AddPosition(Vector2 position)
    {
        X += position.X;
        Y += position.Y;
    }
}