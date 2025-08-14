public class CollisionData
{
    public bool CollideTop = false;
    public bool CollideBottom = false;
    public bool CollideLeft = false;
    public bool CollideRight = false;

    public CollisionData(bool collideTop, bool collideBottom, bool collideLeft, bool collideRight)
    {
        CollideTop = collideTop;
        CollideBottom = collideBottom;
        CollideLeft = collideLeft;
        CollideRight = collideRight;
    }

    public CollisionData() { }
}