using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

public class ColliderComponent : Component
{
    public BoundingBox BoundingBox;
    public bool OnGround = false;

    public ColliderComponent(BoundingBox boundingBox = null, bool drawBoundingBox = false)
    {
        if (boundingBox == null)
        {
            BoundingBox = new BoundingBox(16, 16);
        }
        else
        {
            BoundingBox = boundingBox;
        }

        BoundingBox.DrawBoundingBox = drawBoundingBox;
    }
}