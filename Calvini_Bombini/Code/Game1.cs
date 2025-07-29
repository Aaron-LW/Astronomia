using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Registries.TextureRegistry;

namespace Calvini_Bombini;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        TextureRegistry.LoadTextures(this);
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Input.Update();
        GridSystem.Update();

        Input.SwitchStates();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //_spriteBatch.Draw(textur, new Vector2(0, 0), Color.White);
        //_spriteBatch.Draw(grass, new Vector2(100, 100), null, Color.White, 90f, new Vector2(), 7f, SpriteEffects.None, 0f);

        //Draw(Textur, Position, Farbe)
        //Draw(Textur, Position, Rectangle, Farbe, Rotation, Origin, Skalierung, Spriteeffects, layerdepth)

        GridSystem.Draw(_spriteBatch);
    

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}