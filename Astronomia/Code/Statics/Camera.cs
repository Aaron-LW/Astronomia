using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public static class Camera
{
    public static float X;
    public static float Y;
    public static float Zoom = 1f;
    public static Entity FocusedEntity;

    public static void Start()
    {
        FocusedEntity = EntitySystem.Player;
    }

    public static void Update()
    {
        if (FocusedEntity == null)
        {
            float moveSpeed = Settings.CameraSpeed * Time.DeltaTime;

            if (Input.IsKeyDown(Keys.A))
            {
                X -= moveSpeed;
            }

            if (Input.IsKeyDown(Keys.D))
            {
                X += moveSpeed;
            }

            if (Input.IsKeyDown(Keys.W))
            {
                Y -= moveSpeed;
            }

            if (Input.IsKeyDown(Keys.S))
            {
                Y += moveSpeed;
            }
        }
        else
        {
            PositionComponent positionComponent;
            FocusedEntity.TryGetComponent(out positionComponent);

            if (positionComponent != null)
            {
                SetPosition(positionComponent.Position + (new Vector2(FocusedEntity.Texture.Width / 2, FocusedEntity.Texture.Height / 2) * Settings.GlobalScale) - (ViewportBounds.Bounds / (2f * Zoom)));
            }
        }
    }

    public static Vector2 GetPosition()
    {
        return new Vector2(X, Y);
    }

    public static void SetPosition(Vector2 position)
    {
        X = position.X;
        Y = position.Y;
    }

    public static void ZoomAt(Vector2 zoomCenterScreenPos, float addedZoom)
    {
        Vector2 beforeZoomWorldPos = GetPosition() + zoomCenterScreenPos / Zoom;

        Zoom += addedZoom;
        if (Zoom < 0.1)
        {
            Zoom = 0.1f;
        }

        Vector2 afterZoomWorldPos = GetPosition() + zoomCenterScreenPos / Zoom;

        Vector2 zoomDelta = afterZoomWorldPos - beforeZoomWorldPos;

        X = (GetPosition() - zoomDelta).X;
        Y = (GetPosition() - zoomDelta).Y;
    }

}