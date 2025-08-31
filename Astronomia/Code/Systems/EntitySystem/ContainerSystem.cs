using Microsoft.Xna.Framework.Graphics;

public static class ContainerSystem
{
    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (Entity entity in EntitySystem.Entities)
        {
            ContainerComponent containerComponent;

            entity.TryGetComponent(out containerComponent);

            if (containerComponent != null)
            {
                containerComponent.Draw(spriteBatch);
            }
        }
    }
}