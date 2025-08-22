using Microsoft.Xna.Framework;

public class VelocityComponent : Component
{
    public Vector2 Velocity;

    public VelocityComponent(float xVelocity = 0f, float yVelocity = 0f)
    {
        Velocity = new Vector2(xVelocity, yVelocity);
    }
}