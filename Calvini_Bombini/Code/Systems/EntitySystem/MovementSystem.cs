using Microsoft.Xna.Framework.Input;

public static class MovementSystem
{
    public static void Update()
    {
        foreach (Entity entity in EntitySystem.Entities)
        {
            PositionComponent positionComponent;
            VelocityComponent velocityComponent;
            CharacterControllerComponent characterControllerComponent;
            GravityComponent gravityComponent;

            entity.TryGetComponent(out positionComponent);
            entity.TryGetComponent(out velocityComponent);
            entity.TryGetComponent(out characterControllerComponent);
            entity.TryGetComponent(out gravityComponent);

            if (positionComponent != null && velocityComponent != null)
            {
                if (characterControllerComponent != null)
                {
                    if (Input.IsKeyDown(Keys.D))
                    {
                        velocityComponent.Velocity.X = characterControllerComponent.MoveSpeed;
                    }

                    if (Input.IsKeyDown(Keys.A))
                    {
                        velocityComponent.Velocity.X = -characterControllerComponent.MoveSpeed;
                    }

                    if (Input.IsKeyPressed(Keys.Space) && positionComponent.Y == 0f)
                    {
                        velocityComponent.Velocity.Y = -characterControllerComponent.JumpStrength;
                    }
                }

                if (gravityComponent != null)
                {
                    velocityComponent.Velocity.Y += gravityComponent.Acceleration * Time.DeltaTime;
                }

                positionComponent.AddPosition(velocityComponent.Velocity * Time.DeltaTime);
                velocityComponent.Velocity.X *= 1f - 10f * Time.DeltaTime;

                if (velocityComponent.Velocity.X < 3f && velocityComponent.Velocity.X > -3f)
                {
                    velocityComponent.Velocity.X = 0f;
                }

                if (positionComponent.Position.Y > 0f)
                {
                    positionComponent.Y = 0f;
                }
            }
            else
            {
                continue;
            }
        }
    }
}