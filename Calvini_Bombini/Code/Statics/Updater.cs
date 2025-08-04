using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class Updater
{
    public static void Start()
    {
        GridSystem.Start();
        EntitySystem.Start();
    }

    public static void Update(GameTime gameTime)
    {
        Input.Update();
        Time.Update(gameTime);
        
        GridSystem.Update();
        EntitySystem.Update();
        Camera.Update();

        Input.SwitchStates();
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        GridSystem.Draw(spriteBatch);
        EntitySystem.Draw(spriteBatch);
    }
}