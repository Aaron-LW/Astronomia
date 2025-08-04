using Microsoft.Xna.Framework;

public class VelocityComponent : Component
{
    public VelocityComponent(float xVelocity = 0f, float yVelocity = 0f)
    {
        Velocity = new Vector2(xVelocity, yVelocity);
    }

    public Vector2 Velocity;
}