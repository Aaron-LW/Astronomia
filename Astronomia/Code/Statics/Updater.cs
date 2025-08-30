using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class Updater
{
    public static void Start(GraphicsDevice graphicsDevice)
    {
        GridSystem.Start();
        EntitySystem.Start();
        Camera.Start(graphicsDevice);
        DebugMenu.Start();
    }

    public static void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
    {
        Input.Update();
        Time.Update(gameTime);
        
        GridSystem.Update();
        if (!GridSystem.LevelEditor) { EntitySystem.Update(); }
        Camera.Update();
        ViewportBounds.Update(graphicsDevice);
        DebugMenu.Update();

        Input.SwitchStates();
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        GridSystem.Draw(spriteBatch);
        EntitySystem.Draw(spriteBatch);
        DebugMenu.Draw(spriteBatch);
    }
}