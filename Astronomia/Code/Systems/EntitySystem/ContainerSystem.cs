using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public static class ContainerSystem
{
    public static ItemStack HeldItem = new ItemStack(ItemRegistry.Blank, 0);

    public static void Update()
    {
        foreach (Entity entity in EntitySystem.Entities)
        {
            ContainerComponent containerComponent;

            entity.TryGetComponent(out containerComponent);

            if (containerComponent != null)
            {
                containerComponent.Update();
            }
        }
    }

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

        if (HeldItem.Item.Texture != null)
        {
            //To-Do: shmoovie machen
            spriteBatch.Draw(HeldItem.Item.Texture, Input.GetMousePosition() - new Vector2(HeldItem.Item.Texture.Width / 2 * (Settings.ItemSlotScale - Settings.ItemStackDefaultScaleModifier), HeldItem.Item.Texture.Height / 2 * (Settings.ItemSlotScale - HeldItem.ScaleModifier)), null, Color.White, 0f, new Vector2(), Settings.ItemSlotScale - HeldItem.ScaleModifier, SpriteEffects.None, 0f);
        }
    }
}