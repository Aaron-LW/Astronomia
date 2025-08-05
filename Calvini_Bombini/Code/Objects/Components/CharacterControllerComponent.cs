public class CharacterControllerComponent : Component
{
    public float MoveSpeed;
    public float JumpStrength;

    public CharacterControllerComponent(float moveSpeed = 500, float jumpStrength = 800)
    {
        MoveSpeed = moveSpeed;
        JumpStrength = jumpStrength;
    }
}