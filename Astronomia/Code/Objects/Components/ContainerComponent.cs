using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Registries.TextureRegistry;

public class ContainerComponent : Component
{
    public List<ItemStack> Items = new List<ItemStack>();
    public float X;
    public float Y;
    public Vector2 Position
    {
        get
        {
            return new Vector2(X, Y);
        }

        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    private int _slots;
    private int _itemsPerRow;
    private float _spacing;

    public ContainerComponent(Vector2 position, int slots, int itemsPerRow, float spacing, ItemStack[] items = null)
    {
        _slots = slots;
        _itemsPerRow = itemsPerRow;
        Position = position;
        _spacing = spacing;
        if (items != null)
        {
            Items.AddRange(items);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int x = 0;
        int y = 0;

        for (int i = 0; i < Items.Count; i++)
        {
            Vector2 position = new Vector2(X + x * (16 * Settings.ItemSlotScale) + x * _spacing, Y + y * (16 * Settings.ItemSlotScale) + y * _spacing);
            spriteBatch.Draw(TextureRegistry.ItemSlot, position, null, Color.White, 0f, new Vector2(), Settings.ItemSlotScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(Items[i].Item.Texture, position + new Vector2(TextureRegistry.ItemSlot.Width * Settings.ItemSlotScale / 2 - Items[i].Item.Texture.Width * (Settings.ItemSlotScale - 1.2f) / 2, TextureRegistry.ItemSlot.Height * Settings.ItemSlotScale / 2 - Items[i].Item.Texture.Height * (Settings.ItemSlotScale - 1.2f) / 2), null, Color.White, 0f, new Vector2(), Settings.ItemSlotScale - 1.2f, SpriteEffects.None, 0f);

            x++;
            if (x >= _itemsPerRow)
            {
                x = 0;
                y++;
            }
        }
    }
}