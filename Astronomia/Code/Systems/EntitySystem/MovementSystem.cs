using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public static class MovementSystem
{
    private static float d = 1.7f;

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
                // Make collision here
                Tile[] tiles = GridSystem.GetTilesInArea(positionComponent.Position + (velocityComponent.Velocity * Time.DeltaTime) - new Vector2(EntitySystem.Player.Texture.Width, EntitySystem.Player.Texture.Height) / 2 * d + new Vector2(EntitySystem.Player.Texture.Width, EntitySystem.Player.Texture.Height) / 2, positionComponent.Position + new Vector2(EntitySystem.Player.Texture.Width, EntitySystem.Player.Texture.Height) * d);
                foreach (Tile tile in tiles)
                {
                    CollisionData collisionData = colliderComponent.BoundingBox.WillCollideWith(tile.BoundingBox, velocityComponent.Velocity);

                    if (collisionData.CollideBottom)
                    {
                        velocityComponent.Velocity.Y = 0f;
                        positionComponent.Y = collisionData.Position.Y - tile.Texture.Height + (entity.Texture.Height - (colliderComponent.BoundingBox.Height + colliderComponent.BoundingBox.YOffset));
                        colliderComponent.OnGround = true;
                    }

                    if (collisionData.CollideLeft)
                    {
                        if (velocityComponent.Velocity.X < 0)
                        {
                            velocityComponent.Velocity.X = 0f;
                            positionComponent.X = collisionData.Position.X + tile.Texture.Width - colliderComponent.BoundingBox.XOffset;
                        }
                    }

                    if (collisionData.CollideRight)
                    {
                        if (velocityComponent.Velocity.X > 0)
                        {
                            velocityComponent.Velocity.X = 0f;
                            positionComponent.X = collisionData.Position.X - tile.Texture.Width + (entity.Texture.Width - (colliderComponent.BoundingBox.Width + colliderComponent.BoundingBox.XOffset));
                        }
                    }

                    if (collisionData.CollideTop)
                    {
                        velocityComponent.Velocity.Y = 0f;
                        positionComponent.Y = collisionData.Position.Y + tile.Texture.Height - colliderComponent.BoundingBox.YOffset;
                    }
                }

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
            PositionComponent positionComponent = null;
            VelocityComponent velocityComponent = null;

            entity.TryGetComponent(out colliderComponent);
            if (DebugMenu.ShowCollisionCheckArea)
            {
                entity.TryGetComponent(out positionComponent);
                entity.TryGetComponent(out velocityComponent);
            }

            if (colliderComponent != null)
            {
                if (colliderComponent.BoundingBox.DrawBoundingBox)
                {
                    colliderComponent.BoundingBox.Draw(spriteBatch);
                }
            }

            if (DebugMenu.ShowCollisionCheckArea)
            {
                GridSystem.GetTilesInArea(positionComponent.Position + (velocityComponent.Velocity * Time.DeltaTime) - new Vector2(EntitySystem.Player.Texture.Width, EntitySystem.Player.Texture.Height) / 2 * d + new Vector2(EntitySystem.Player.Texture.Width, EntitySystem.Player.Texture.Height) / 2, positionComponent.Position + new Vector2(EntitySystem.Player.Texture.Width, EntitySystem.Player.Texture.Height) * d, spriteBatch);
            }
        }
    }
}