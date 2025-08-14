using System;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
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
            ColliderComponent colliderComponent;

            entity.TryGetComponent(out positionComponent);
            entity.TryGetComponent(out velocityComponent);
            entity.TryGetComponent(out characterControllerComponent);
            entity.TryGetComponent(out gravityComponent);
            entity.TryGetComponent(out colliderComponent);

            if (positionComponent != null && velocityComponent != null)
            {
                if (colliderComponent != null)
                {
                    colliderComponent.BoundingBox.UpdatePosition(positionComponent.Position);
                }

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

                    if (Input.IsKeyPressed(Keys.Space))
                    {
                        velocityComponent.Velocity.Y = -characterControllerComponent.JumpStrength;
                    }
                }

                if (gravityComponent != null)
                {
                    velocityComponent.Velocity.Y += gravityComponent.Acceleration * Time.DeltaTime;
                }

                //Handle the collision right before the velocity gets added so it doesn't get added lol
                foreach (Tile tile in GridSystem.Tiles)
                {
                    //if (colliderComponent.BoundingBox.WillCollideWithY(tile.BoundingBox, velocityComponent.Velocity) && colliderComponent.BoundingBox.WillCollideWithX(tile.BoundingBox, velocityComponent.Velocity))
                    //{
                    //    if (velocityComponent.Velocity.Y > 0f)
                    //    {
                    //        velocityComponent.Velocity.Y = 0f;
                    //    }
                    //}

                    CollisionData collisionData = colliderComponent.BoundingBox.WillCollideWith(tile.BoundingBox, velocityComponent.Velocity);

                    if (collisionData.CollideLeft)
                    {
                        if (velocityComponent.Velocity.X < 0)
                        {
                            velocityComponent.Velocity.X = 0f;
                            positionComponent.X = tile.Position.X + tile.Texture.Width;
                            Console.WriteLine("Left");
                        }
                    }

                    if (collisionData.CollideRight)
                    {
                        if (velocityComponent.Velocity.X > 0)
                        {
                            velocityComponent.Velocity.X = 0f;
                            positionComponent.X = tile.Position.X - colliderComponent.BoundingBox.Width;
                            Console.WriteLine("Right");
                        }
                    }

                    if (collisionData.CollideBottom)
                    {
                        velocityComponent.Velocity.Y = 0f;
                        positionComponent.Y = tile.Position.Y - colliderComponent.BoundingBox.Height;
                        Console.WriteLine("Bottom");
                    }
                }

                positionComponent.AddPosition(velocityComponent.Velocity * Time.DeltaTime);
                velocityComponent.Velocity.X *= 1f - 10f * Time.DeltaTime;

                if (velocityComponent.Velocity.X < 3f && velocityComponent.Velocity.X > -3f)
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

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (Entity entity in EntitySystem.Entities)
        {
            ColliderComponent colliderComponent;

            entity.TryGetComponent(out colliderComponent);

            if (colliderComponent != null)
            {
                if (colliderComponent.BoundingBox.DrawBoundingBox)
                {
                    colliderComponent.BoundingBox.Draw(spriteBatch);
                }
            }
        }
    }
}