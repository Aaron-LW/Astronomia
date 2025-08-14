using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

public class Tile
{
    public Vector2 Position;
    public Texture2D Texture;
    public float Rotation;
    public BoundingBox BoundingBox;

    public Tile(Vector2 position, Texture2D texture, float rotation = 0)
    {
        Position = position;
        Texture = texture;
        Rotation = rotation;
        BoundingBox = new BoundingBox(16, 16, true);
        BoundingBox.UpdatePosition(position);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, (RotationHelper.GetRotatedPosition(Position, new SizeF(Texture.Width, Texture.Height), Rotation) - Camera.GetPosition()) * Camera.Zoom, null, Color.White, Rotation, new Vector2(), Settings.GlobalScale * Camera.Zoom, SpriteEffects.None, 0f);
        BoundingBox.Draw(spriteBatch);
    }
}