using Microsoft.Xna.Framework;

public class CollisionData
{
    public bool CollideTop = false;
    public bool CollideBottom = false;
    public bool CollideLeft = false;
    public bool CollideRight = false;
    public bool IsColliding = false;
    public Vector2 Position;

    public CollisionData(bool collideTop, bool collideBottom, bool collideLeft, bool collideRight, bool isColliding)
    {
        CollideTop = collideTop;
        CollideBottom = collideBottom;
        CollideLeft = collideLeft;
        CollideRight = collideRight;
        IsColliding = isColliding;
    }

    public CollisionData() { }
}