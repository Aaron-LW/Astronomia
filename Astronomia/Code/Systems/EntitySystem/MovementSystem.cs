using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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

                    if (Input.IsKeyDown(Keys.Space) && colliderComponent.OnGround == true)
                    {
                        velocityComponent.Velocity.Y = -characterControllerComponent.JumpStrength;
                    }
                }

                if (gravityComponent != null)
                {
                    velocityComponent.Velocity.Y += gravityComponent.Acceleration * Time.DeltaTime;
                }

                //Handle the collision right before the velocity gets added so it doesn't get added lol
                colliderComponent.OnGround = false;
                //foreach (Tile tile in GridSystem.Tiles)
                //{
                    //if (colliderComponent.BoundingBox.WillCollideWithY(tile.BoundingBox, velocityComponent.Velocity) && colliderComponent.BoundingBox.WillCollideWithX(tile.BoundingBox, velocityComponent.Velocity))
                    //{
                    //    if (velocityComponent.Velocity.Y > 0f)
                    //    {
                    //        velocityComponent.Velocity.Y = 0f;
                    //    }
                    //}

                    //CollisionData collisionData = colliderComponent.BoundingBox.WillCollideWith(tile.BoundingBox, velocityComponent.Velocity);

                    //if (collisionData.CollideBottom)
                    //{
                    //    velocityComponent.Velocity.Y = 0f;
                    //    positionComponent.Y = tile.Position.Y - tile.Texture.Height + (entity.Texture.Height - (colliderComponent.BoundingBox.Height + colliderComponent.BoundingBox.YOffset));
                    //    colliderComponent.OnGround = true;
                    //}

                    //if (collisionData.CollideTop)
                    //{
                    //    velocityComponent.Velocity.Y = 0f;
                    //    positionComponent.Y = tile.Position.Y + tile.Texture.Height - colliderComponent.BoundingBox.YOffset;
                    //}

                    //if (collisionData.CollideLeft)
                    //{
                    //    if (velocityComponent.Velocity.X < 0)
                    //    {
                    //        velocityComponent.Velocity.X = 0f;
                    //        positionComponent.X = tile.Position.X + tile.Texture.Width - colliderComponent.BoundingBox.XOffset;
                    //    }
                    //}

                    //if (collisionData.CollideRight)
                    //{
                    //    if (velocityComponent.Velocity.X > 0)
                    //    {
                    //        velocityComponent.Velocity.X = 0f;
                    //        positionComponent.X = tile.Position.X - tile.Texture.Width + (entity.Texture.Width - (colliderComponent.BoundingBox.Width + colliderComponent.BoundingBox.XOffset));
                    //    }
                    //}
                //}

                positionComponent.AddPosition(velocityComponent.Velocity * Time.DeltaTime);
                velocityComponent.Velocity.X *= 1f - Settings.Drag * Time.DeltaTime;

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