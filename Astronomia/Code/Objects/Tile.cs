using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

public class Tile
{
    public Texture2D Texture;
    public float Rotation;
    public BoundingBox BoundingBox;

    public Tile(Texture2D texture, float rotation = 0)
    {
        Texture = texture;
        Rotation = rotation;
        BoundingBox = new BoundingBox(16, 16, false);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(Texture, RotationHelper.GetRotatedPosition(position, new SizeF(Texture.Width, Texture.Height), Rotation, Camera.Zoom), null, Color.White, Rotation, new Vector2(), Settings.GlobalScale * Camera.Zoom, SpriteEffects.None, 0f);
        GridSystem.DrawnTiles++;

        if (BoundingBox.DrawBoundingBox)
        {
            BoundingBox.Draw(spriteBatch);
        }
    }
}