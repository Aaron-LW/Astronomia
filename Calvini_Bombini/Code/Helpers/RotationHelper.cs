using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

public static class RotationHelper
{
    //[membermodifizierer (public/private), [returntype (void/Vector2/int/objekte)], [Name], [argumente in ()], {}]]
    public static float DegreesToRadians(float degrees)
    {
        return degrees * ((float)Math.PI / 180);
    }

    public static Vector2 GetRotatedPosition(Vector2 position, SizeF bounds, float rotation) 
    {
        float centerX = position.X + (bounds.Width / 2f) * (Settings.GlobalScale);
        float centerY = position.Y + (bounds.Height / 2f) * (Settings.GlobalScale);

        float newX = position.X - centerX;
        float newY = position.Y - centerY;
        
        float rotatedX = newX * (float)Math.Cos(rotation) - newY * (float)Math.Sin(rotation);
        float rotatedY = newX * (float)Math.Sin(rotation) + newY * (float)Math.Cos(rotation);
        
        return new Vector2(centerX + rotatedX, centerY + rotatedY); 
    }
}

