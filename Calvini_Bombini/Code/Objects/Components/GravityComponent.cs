public class GravityComponent : Component
{
    public float Acceleration;

    public GravityComponent(float acceleration = 2600f)
    {
        Acceleration = acceleration;
    }
}