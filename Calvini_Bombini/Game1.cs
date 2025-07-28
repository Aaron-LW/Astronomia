using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Calvini_Bombini;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D textur;
    private Texture2D grass;
    private Vector2 position;

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
        textur = Content.Load<Texture2D>("mensch");
        grass = Content.Load<Texture2D>("Grass");

        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    MouseState previousMouseState;
    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        MouseState mouseState = Mouse.GetState();

        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (keyboardState.IsKeyDown(Keys.D)) 
        {
            position.X += 10;
        }

        if (keyboardState.IsKeyDown(Keys.A))
        {
            position.X -= 10;
        }

        if (keyboardState.IsKeyDown(Keys.W))
        {
            position.Y -= 10;
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            position.Y += 10;
        }

        if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
        {
            position.X -= 100;
        }

        if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
        { 
            position.X += 100;
        }

        previousMouseState = mouseState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //_spriteBatch.Draw(textur, new Vector2(0, 0), Color.White);
        _spriteBatch.Draw(textur, position, null, Color.White, 0f, new Vector2(), 5f, SpriteEffects.None, 0f);
        //_spriteBatch.Draw(grass, new Vector2(100, 100), null, Color.White, 90f, new Vector2(), 7f, SpriteEffects.None, 0f);

        //Draw(Textur, Position, Farbe)
        //Draw(Textur, Position, Rectangle, Farbe, Rotation, Origin, Skalierung, Spriteeffects, layerdepth)

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}