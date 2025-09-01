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
            Vector2 mousePosition = Input.GetMousePosition();
            HeldItem.X = MathHelper.Lerp(HeldItem.X, mousePosition.X - HeldItem.Item.Texture.Width * (Settings.ItemSlotScale - HeldItem.ScaleModifier) / 2, 100f * Time.DeltaTime);
            HeldItem.Y = MathHelper.Lerp(HeldItem.Y, mousePosition.Y - HeldItem.Item.Texture.Height * (Settings.ItemSlotScale - HeldItem.ScaleModifier) / 2, 100f * Time.DeltaTime);
            spriteBatch.Draw(HeldItem.Item.Texture, HeldItem.Position, null, Color.White, 0f, new Vector2(), Settings.ItemSlotScale - HeldItem.ScaleModifier, SpriteEffects.None, 0f);

            if (HeldItem.Amount != 1 || HeldItem.Amount != 0)
            {
                Vector2 textPosition = HeldItem.Position + new Vector2(HeldItem.Item.Texture.Width * (Settings.ItemSlotScale - HeldItem.ScaleModifier) - Settings.Font.MeasureString(HeldItem.Amount.ToString()).X * 0.1f / 1.5f
                                                                       , HeldItem.Item.Texture.Height * (Settings.ItemSlotScale - HeldItem.ScaleModifier) - Settings.Font.MeasureString(HeldItem.Amount.ToString()).Y * 0.1f / 1.5f);
                spriteBatch.DrawString(Settings.Font, HeldItem.Amount.ToString(), textPosition, Color.White, 0f, new Vector2(), 0.1f, SpriteEffects.None, 0f);
            }
        }
    }
}