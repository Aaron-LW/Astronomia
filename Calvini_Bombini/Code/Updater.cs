using Microsoft.Xna.Framework.Graphics;

public static class Updater
{
    public static void Start()
    {

    }

    public static void Update()
    {
        Input.Update();
        GridSystem.Update();

        Input.SwitchStates();
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        GridSystem.Draw(spriteBatch);
    }
}