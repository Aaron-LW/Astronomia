using System;
using Microsoft.Xna.Framework;

public static class MovementSystem
{
    public static void Update()
    {
        foreach (Entity entity in EntitySystem.Entities)
        {
            PositionComponent positionComponent;
            VelocityComponent velocityComponent;

            entity.TryGetComponent<PositionComponent>(out positionComponent);
            entity.TryGetComponent<VelocityComponent>(out velocityComponent);

            if (positionComponent != null && velocityComponent != null)
            {
                positionComponent.AddPosition(velocityComponent.Velocity * Time.DeltaTime);
                velocityComponent.Velocity *= 1f - 7f * Time.DeltaTime;

                if (velocityComponent.Velocity.X < 10f)
                {
                    velocityComponent.Velocity.X = 0f;
                }
            }
            else
            {
                continue;
            }
        }
    }
}