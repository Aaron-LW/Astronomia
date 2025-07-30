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
    
    private int _frameCounter;
    private TimeSpan _elapsedTime = TimeSpan.Zero;
    private int _fps;

    private SpriteFont _font;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 180.0);
        _graphics.SynchronizeWithVerticalRetrace = false;
        IsFixedTimeStep = false;
        _graphics.IsFullScreen = true;

        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        Updater.Start();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        TextureRegistry.LoadTextures(this);
        _font = Content.Load<SpriteFont>("font");
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Updater.Update();

        _elapsedTime += gameTime.ElapsedGameTime;
        _frameCounter++;

        if (_elapsedTime >= TimeSpan.FromSeconds(1))
        {
            _fps = _frameCounter;
            _frameCounter = 0;
            _elapsedTime -= TimeSpan.FromSeconds(1);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Updater.Draw(_spriteBatch);
        //_spriteBatch.Draw(textur, new Vector2(0, 0), Color.White);
        //_spriteBatch.Draw(grass, new Vector2(100, 100), null, Color.White, 90f, new Vector2(), 7f, SpriteEffects.None, 0f);

        //Draw(Textur, Position, Farbe)
        //Draw(Textur, Position, Rectangle, Farbe, Rotation, Origin, Skalierung, Spriteeffects, layerdepth)
        _spriteBatch.DrawString(_font, $"FPS: {_fps}", new Vector2(10, 10), Color.White, 0f, new Vector2(), 0.1f, SpriteEffects.None, 0f);

        //Vertestung von github
        //Nächster test weil der andere hat angeblich nicht funktioniert
        //Noch ein test weil hoffnung

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}