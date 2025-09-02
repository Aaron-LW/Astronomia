using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Registries.TextureRegistry;

public class AnimatedText
{
    public Vector2 StartPosition;
    public Vector2 EndPosition
    {
        get
        {
            return new Vector2(_endX, _endY) + _endPositionHoverModifier;
        }
        set
        {
            _endX = value.X;
            _endY = value.Y;
        }
    }
    public Vector2 Position;
    public string Text;
    public bool Active;
    public float Speed;
    public float ScaleModifier;
    public Action OnClickAction;

    private Vector2 _endPositionHoverModifier;
    private float _endX;
    private float _endY;
    private Vector2 _textBounds => Settings.Font.MeasureString(Text) * (0.1f + ScaleModifier);

    public AnimatedText(string text, Vector2 startPosition, Vector2 endPosition, float speed, Action onClickAction, float scalemodifer = 0, bool active = false)
    {
        Text = text;
        StartPosition = startPosition;
        EndPosition = endPosition;
        Active = active;
        Speed = speed;
        ScaleModifier = scalemodifer;
        OnClickAction = onClickAction;
    }

    public void AnimationStep()
    {
        if (Active)
        {
            if (Position != EndPosition)
            {
                Position.X = MathHelper.Lerp(Position.X, EndPosition.X, Speed * Time.DeltaTime);
                Position.Y = MathHelper.Lerp(Position.Y, EndPosition.Y, Speed * Time.DeltaTime);
            }

            RectangleF rect = new RectangleF(StartPosition.X, StartPosition.Y, EndPosition.X - StartPosition.X + _textBounds.X, EndPosition.Y - StartPosition.Y + _textBounds.Y);
            if (rect.Intersects(Input.GetMouseRectangle()))
            {
                _endPositionHoverModifier = new Vector2(30, 0);
                if (Input.IsLeftMousePressed())
                {
                    OnClickAction();
                }
            }
            else
            {
                _endPositionHoverModifier = Vector2.Zero;
            }
        }
        else
        {
            if (Position != StartPosition)
            {
                Position.X = MathHelper.Lerp(Position.X, StartPosition.X, Speed * Time.DeltaTime);
                Position.Y = MathHelper.Lerp(Position.Y, StartPosition.Y, Speed * Time.DeltaTime);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);
        spriteBatch.Draw(TextureRegistry.GradientTexture, new Rectangle((int)0, (int)EndPosition.Y - (int)_endPositionHoverModifier.Y, (int)EndPosition.X + (int)_textBounds.X - ((int)EndPosition.X - (int)Position.X), (int)EndPosition.Y - (int)StartPosition.Y + (int)_textBounds.Y), Color.White);
        spriteBatch.DrawString(Settings.Font, Text, Position, Color.White, 0f, new Vector2(), 0.1f + ScaleModifier, SpriteEffects.None, 0f);
        spriteBatch.End();

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //I now that I am probably commiting a lot of war crimes right here but I don't care. I just want a gradient
    }
}