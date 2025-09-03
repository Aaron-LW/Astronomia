using System;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Registries.TextureRegistry;
using System.Collections.Generic;
using System.Linq;

public static class ControlsMenu
{
    public static float X;
    public static float Y;
    public static Vector2 Position => new Vector2(X, Y);

    private static float _endX;
    private static float _endY;
    private static Vector2 _endPosition => new Vector2(_endX, _endY);

    private static float _startX;
    private static float _startY;
    private static Vector2 _startPosition => new Vector2(_startX, _startY);

    public static float Width;
    public static float Height;
    public static Vector2 Bounds => new Vector2(Width, Height);

    private static Color _backgroundColor;
    private static bool _active = false;

    private static Dictionary<string, string> _controls = new Dictionary<string, string>();

    public static void Start()
    {
        Width = 700 * (1920 / ViewportBounds.Width);
        Height = 950 * (1920 / ViewportBounds.Width);
        _endX = ViewportBounds.Width / 2;
        _endY = ViewportBounds.Height / 2;
        _startX = ViewportBounds.Width / 2;
        _startY = ViewportBounds.Height + Height;
        X = _startX;
        Y = _startY;
        _backgroundColor = new Color(31, 32, 34);

        _controls.Add("W", "(Leveleditor) Move upwards");
        _controls.Add("S", "(Leveleditor) Move downwards");
        _controls.Add("A", "Move leftwards");
        _controls.Add("D", "Move rightwards");
        _controls.Add("Space", "      Jump");
        _controls.Add("F", "Switch to leveleditor/normal mode");
        _controls.Add("LMB", "(Leveleditor) Place tile");
        _controls.Add("RMB", "(Leveleditor) Remove tile");
        _controls.Add("R", "(Leveleditor) Rotate tile");
        _controls.Add("H", "Debug menu");
        _controls.Add("Tab", "(Leveleditor) Select tile");
        _controls.Add("Shift + LMB", "                      (Leveleditor) Fast place tiles");
        _controls.Add("Shift + RMB", "                      (Leveleditor) Fast remove tiles");
        _controls.Add("CTRL + LMB", "                       (Leveleditor) Mass place tiles");
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        if (!_active)
        {
            Y = MathHelper.Lerp(Y, _startY, 20 * Time.DeltaTime);
        }
        else
        {
            Y = MathHelper.Lerp(Y, _endY, 20 * Time.DeltaTime);
        }

        spriteBatch.FillRectangle(new RectangleF(X - Width / 2, Y - Height / 2, Width, Height), _backgroundColor);

        for (int i = 0; i < _controls.Count; i++)
        {
            float scale = 4;
            Vector2 margin = new Vector2(30, 30);
            Vector2 position = new Vector2(X - Width / 2 + margin.X, Y + (TextureRegistry.KeyboardKey.Height * scale * i) - Height / 2 + margin.Y);

            spriteBatch.Draw(TextureRegistry.KeyboardKey, position, null, Color.White, 0f, new Vector2(), scale, SpriteEffects.None, 0f);

            Vector2 textPosition = position + new Vector2(TextureRegistry.KeyboardKey.Width * scale / 2, TextureRegistry.KeyboardKey.Height * scale / 2) - (Settings.Font.MeasureString(_controls.ElementAt(i).Key) / 2 * 0.15f);
            spriteBatch.DrawString(Settings.Font, _controls.ElementAt(i).Key, textPosition, Color.White, 0f, new Vector2(), 0.15f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Settings.Font, "=>", textPosition + new Vector2(TextureRegistry.KeyboardKey.Width * scale, Settings.Font.MeasureString("=>").Y / 4 * 0.1f), Color.White, 0f, new Vector2(), 0.1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Settings.Font, _controls.ElementAt(i).Value, textPosition + new Vector2(TextureRegistry.KeyboardKey.Width * scale * 1.8f, Settings.Font.MeasureString(_controls.ElementAt(i).Value).Y / 4 * 0.1f), Color.White, 0f, new Vector2(), 0.1f, SpriteEffects.None, 0f);
        }
    }

    public static void Open()
    {
        if (EscapeMenu.OpenedMenu != String.Empty) { return; }
        _active = true;
        EscapeMenu.OpenedMenu = "ControlsMenu";
    }

    public static void Close()
    {
        _active = false;
    }
}