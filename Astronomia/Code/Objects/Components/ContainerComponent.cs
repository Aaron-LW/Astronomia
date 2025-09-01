using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Registries.TextureRegistry;
using MonoGame.Extended;

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

        for (int i = 0; i < _slots; i++)
        {
            Items.Add(new ItemStack(ItemRegistry.Blank, 0));
        }

        if (items != null)
        {
            for (int i = 0; i < items.Length; i++)
            {
                AddItem(items[i]);
            }
        }

    }

    public void Update()
    {
        Dictionary<int, RectangleF> slotBoxes = GetItemSlotBoxesAndIndices();
        foreach (KeyValuePair<int, RectangleF> keyValuePair in slotBoxes)
        {
            if (keyValuePair.Value.Intersects(Input.GetMouseRectangle()) && Input.IsLeftMousePressed())
            {
                if (ContainerSystem.HeldItem == null)
                {
                    ContainerSystem.HeldItem = Items[keyValuePair.Key];
                    Items[keyValuePair.Key] = new ItemStack(ItemRegistry.Blank, 0);
                }
                else
                {
                    ItemStack tempStack = Items[keyValuePair.Key];
                    Items[keyValuePair.Key] = ContainerSystem.HeldItem;
                    ContainerSystem.HeldItem = tempStack;
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int x = 0;
        int y = 0;

        for (int i = 0; i < Items.Count; i++)
        {
            Vector2 slotPosition = new Vector2(X + x * (16 * Settings.ItemSlotScale) + x * _spacing, Y + y * (16 * Settings.ItemSlotScale) + y * _spacing);
            Vector2 itemTexturePosition = new Vector2();
            if (Items[i].Item != null)
            {
                if (Items[i].Item.Texture != null)
                {
                    itemTexturePosition = slotPosition + new Vector2(TextureRegistry.ItemSlot.Width * Settings.ItemSlotScale / 2 - Items[i].Item.Texture.Width * (Settings.ItemSlotScale - Items[i].ScaleModifier) / 2, TextureRegistry.ItemSlot.Height * Settings.ItemSlotScale / 2 - Items[i].Item.Texture.Height * (Settings.ItemSlotScale - Items[i].ScaleModifier) / 2);
                    if (Items[i].Position != itemTexturePosition)
                    {
                        Items[i].X = MathHelper.Lerp(Items[i].X, itemTexturePosition.X, 100f * Time.DeltaTime);
                        Items[i].Y = MathHelper.Lerp(Items[i].Y, itemTexturePosition.Y, 100f * Time.DeltaTime);
                    }

                    RectangleF rectangle = new RectangleF(itemTexturePosition.X, itemTexturePosition.Y, Items[i].Item.Texture.Width * (Settings.ItemSlotScale - Items[i].ScaleModifier), Items[i].Item.Texture.Height * (Settings.ItemSlotScale - Items[i].ScaleModifier));
                    if (rectangle.Intersects(Input.GetMouseRectangle()))
                    {
                        Items[i].ScaleModifier = MathHelper.Lerp(Items[i].ScaleModifier, 0.5f, 100f * Time.DeltaTime);
                    }
                    else
                    {
                        Items[i].ScaleModifier = MathHelper.Lerp(Items[i].ScaleModifier, Settings.ItemStackDefaultScaleModifier, 100f * Time.DeltaTime);
                    }
                }
            }


            spriteBatch.Draw(TextureRegistry.ItemSlot, slotPosition, null, Color.White, 0f, new Vector2(), Settings.ItemSlotScale, SpriteEffects.None, 0f);
            if (Items[i].Item.Texture != null)
            {
                spriteBatch.Draw(Items[i].Item.Texture, Items[i].Position, null, Color.White, 0f, new Vector2(), Settings.ItemSlotScale - Items[i].ScaleModifier, SpriteEffects.None, 0f);
            }

            if (Items[i].Amount != 1 || Items[i].Amount != 0)
            {
                if (Items[i].Item.Texture != null)
                {
                    Vector2 textPosition = itemTexturePosition + new Vector2(Items[i].Item.Texture.Width * (Settings.ItemSlotScale - Items[i].ScaleModifier) - Settings.Font.MeasureString(Items[i].Amount.ToString()).X * 0.1f / 1.5f
                                                                           , Items[i].Item.Texture.Height * (Settings.ItemSlotScale - Items[i].ScaleModifier) - Settings.Font.MeasureString(Items[i].Amount.ToString()).Y * 0.1f / 1.5f);
                    spriteBatch.DrawString(Settings.Font, Items[i].Amount.ToString(), textPosition, Color.White, 0f, new Vector2(), 0.1f, SpriteEffects.None, 0f);
                }
            }

            x++;
            if (x >= _itemsPerRow)
            {
                x = 0;
                y++;
            }
        }
    }

    public void AddItem(ItemStack itemStack)
    {
        if (itemStack.Amount <= 0)
        {
            Console.WriteLine("Can't add 0 of item " + itemStack.Item.Name);
            return;
        }

        foreach (ItemStack item in Items)
        {
            if (item.Item == itemStack.Item && item.Amount < item.Item.MaxStackSize || item.Item == ItemRegistry.Blank)
            {
                if (item.Item == ItemRegistry.Blank)
                {
                    if (itemStack.Amount > itemStack.Item.MaxStackSize)
                    {
                        item.Item = itemStack.Item;
                        item.Amount = itemStack.Item.MaxStackSize;
                        itemStack.Amount -= itemStack.Item.MaxStackSize;
                    }
                    else
                    {
                        item.Item = itemStack.Item;
                        item.Amount = itemStack.Amount;
                        return;
                    }
                }
                else
                {
                    if (item.Amount + itemStack.Amount > item.Item.MaxStackSize)
                    {
                        itemStack.Amount -= item.Item.MaxStackSize - item.Amount;
                        item.Amount = item.Item.MaxStackSize;
                    }
                    else
                    {
                        item.Amount += itemStack.Amount;
                        return;
                    }
                }
            }
        }

        Console.WriteLine("Not enough space to put Item " + itemStack.Item.Name);
    }

    private Dictionary<int, RectangleF> GetItemSlotBoxesAndIndices()
    {
        int x = 0;
        int y = 0;
        Dictionary<int, RectangleF> dict = new Dictionary<int, RectangleF>();

        for (int i = 0; i < Items.Count; i++)
        {
            Vector2 slotPosition = new Vector2(X + x * (16 * Settings.ItemSlotScale) + x * _spacing, Y + y * (16 * Settings.ItemSlotScale) + y * _spacing);
            RectangleF rectangle = new RectangleF(slotPosition.X, slotPosition.Y, TextureRegistry.ItemSlot.Width * Settings.ItemSlotScale, TextureRegistry.ItemSlot.Height * Settings.ItemSlotScale);

            dict[i] = rectangle;

            x++;
            if (x >= _itemsPerRow)
            {
                x = 0;
                y++;
            }
        }

        return dict;
    }
}