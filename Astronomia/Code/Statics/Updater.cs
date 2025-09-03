using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class Updater
{
    public static void Start(GraphicsDevice graphicsDevice)
    {
        ItemRegistry.Start();
        GridSystem.Start();
        EntitySystem.Start();
        Camera.Start(graphicsDevice);
        ViewportBounds.Update(graphicsDevice);
        DebugMenu.Start();
        EscapeMenu.Start();
        ControlsMenu.Start();
    }

    public static void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
    {
        Input.Update();
        Time.Update(gameTime);
        
        GridSystem.Update();
        EntitySystem.Update();
        EscapeMenu.Update();
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
        EscapeMenu.Draw(spriteBatch);
        ControlsMenu.Draw(spriteBatch);
    }
}