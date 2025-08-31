using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Text;
using MonoGame.Extended;

public static class Input
{
    private static KeyboardState PreviousKeyboardState;
    private static KeyboardState KeyboardState;

    private static MouseState PreviousMouseState;
    private static MouseState MouseState;

    public static bool IsKeyPressed(Keys Key)
    {
        if (KeyboardState.IsKeyDown(Key) && PreviousKeyboardState.IsKeyUp(Key))
        {
            return true;
        }

        return false;
    }

    public static bool IsKeyDown(Keys Key)
    {
        return KeyboardState.IsKeyDown(Key);
    }

    public static bool IsLeftMousePressed()
    {
        return MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
    }

    public static bool IsLeftMouseDown()
    {
        return MouseState.LeftButton == ButtonState.Pressed;
    }

    public static bool IsRightMousePressed()
    {
        return MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released;
    }

    public static bool IsRightMouseDown()
    {
        return MouseState.RightButton == ButtonState.Pressed;
    }

    public static Vector2 GetMousePosition()
    {
        Point MousePosition = MouseState.Position;

        return new Vector2(MousePosition.X, MousePosition.Y);
    }

    public static RectangleF GetMouseRectangle()
    {
        return new RectangleF(MouseState.Position.X, MouseState.Position.Y, 1, 1);
    }

    public static string GetPressedKeys(string startText = "")
    {
        Keys[] pressedKeys = KeyboardState.GetPressedKeys();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(startText);

        foreach (Keys key in pressedKeys)
        {
            if (KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key))
            {
                char? character = ConvertKeyToChar(key, KeyboardState.IsKeyDown(Keys.LeftShift) || KeyboardState.IsKeyDown(Keys.RightShift));
                if (character != null)
                {
                    stringBuilder.Append(character);
                }

                if (key == Keys.Back && stringBuilder.Length > 0)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                }
            }
        }

        return stringBuilder.ToString();
    }

    public static void Update()
    {
        KeyboardState = Keyboard.GetState();
        MouseState = Mouse.GetState();
    }

    public static void SwitchStates()
    {
        PreviousKeyboardState = KeyboardState;
        PreviousMouseState = MouseState;
    }


    private static char? ConvertKeyToChar(Keys key, bool shift)
    {
        if (key >= Keys.A && key <= Keys.Z)
        {
            char letter = (char)('a' + (key - Keys.A));
            return shift ? char.ToUpper(letter) : letter;
        }

        if (key >= Keys.D0 && key <= Keys.D9)
        {
            string normal = "0123456789";
            string shifted = ")!@#$%^&*(";
            int index = key - Keys.D0;
            return shift ? shifted[index] : normal[index];
        }

        if (key == Keys.Space)
            return ' ';

        if (key == Keys.OemPeriod) return '.';
        if (key == Keys.OemComma) return ',';

        return null;
    }
}