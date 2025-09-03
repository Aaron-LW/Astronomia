using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Input;

public static class DebugMenu
{
    public static bool PlayerHitbox = false;
    public static bool ViewportEdges = false;
    public static bool CameraTileSamplePoints = false;
    public static bool InfiniteZoom = false;
    public static bool ShowCollisionCheckArea = false;

    private static Vector2 _buttonStartPos = new Vector2(50, 50);
    private static Vector2 _buttonBounds = new Vector2(450, 100);
    private static float _buttonSpacing = 30;
    private static bool _debugMenu = false;

    private static float _counter;

    public static void Start()
    {
        EntitySystem.Player.TryGetComponent<ColliderComponent>(out var collider);
        collider.BoundingBox.DrawBoundingBox = PlayerHitbox;
    }

    public static void Update()
    {
        if (Input.IsKeyPressed(Keys.H))
        {
            _debugMenu = !_debugMenu;
        }

        //Settings.Font.Spacing += MathF.Sin(_counter) / 2;
        //_counter += Time.DeltaTime * 8;

        if (new RectangleF(_buttonStartPos.X, _buttonStartPos.Y, _buttonBounds.X, _buttonBounds.Y).Contains(Input.GetMousePosition()) && Input.IsLeftMousePressed() && _debugMenu)
        {
            PlayerHitbox = !PlayerHitbox;
            EntitySystem.Player.TryGetComponent<ColliderComponent>(out var collider);
            collider.BoundingBox.DrawBoundingBox = PlayerHitbox;
        }

        if (new RectangleF(_buttonStartPos.X, _buttonStartPos.Y + _buttonBounds.Y + _buttonSpacing, _buttonBounds.X, _buttonBounds.Y).Contains(Input.GetMousePosition()) && Input.IsLeftMousePressed() && _debugMenu)
        {
            ViewportEdges = !ViewportEdges;
        }

        if (new RectangleF(_buttonStartPos.X, _buttonStartPos.Y + (_buttonBounds.Y + _buttonSpacing) * 2, _buttonBounds.X, _buttonBounds.Y).Contains(Input.GetMousePosition()) && Input.IsLeftMousePressed() && _debugMenu)
        {
            CameraTileSamplePoints = !CameraTileSamplePoints;
        }

        if (new RectangleF(_buttonStartPos.X, _buttonStartPos.Y + (_buttonBounds.Y + _buttonSpacing) * 3, _buttonBounds.X, _buttonBounds.Y).Contains(Input.GetMousePosition()) && Input.IsLeftMousePressed() && _debugMenu)
        {
            InfiniteZoom = !InfiniteZoom;
        }

        if (new RectangleF(_buttonStartPos.X, _buttonStartPos.Y + (_buttonBounds.Y + _buttonSpacing) * 4, _buttonBounds.X, _buttonBounds.Y).Contains(Input.GetMousePosition()) && Input.IsLeftMousePressed() && _debugMenu)
        {
            ShowCollisionCheckArea = !ShowCollisionCheckArea;
        }
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        if (!_debugMenu) { return; }

        spriteBatch.FillRectangle(new RectangleF(_buttonStartPos.X, _buttonStartPos.Y, _buttonBounds.X, _buttonBounds.Y), Color.Gray);
        spriteBatch.DrawString(Settings.Font, "PlayerHitbox: " + PlayerHitbox.ToString(), _buttonStartPos + _buttonBounds / 2 - Settings.Font.MeasureString("PlayerHitbox: " + PlayerHitbox.ToString()) * 0.1f / 2, Color.White, 0f, new Vector2(), 0.1f, SpriteEffects.None, 0f);

        spriteBatch.FillRectangle(new RectangleF(_buttonStartPos.X, _buttonStartPos.Y + _buttonBounds.Y + _buttonSpacing, _buttonBounds.X, _buttonBounds.Y), Color.Gray);
        spriteBatch.DrawString(Settings.Font, "ViewportEdges: " + ViewportEdges.ToString(), _buttonStartPos + _buttonBounds / 2 + new Vector2(0, _buttonBounds.Y + _buttonSpacing) - Settings.Font.MeasureString("ViewportEdges: " + ViewportEdges.ToString()) * 0.1f / 2, Color.White, 0f, new Vector2(), 0.1f, SpriteEffects.None, 0f);

        spriteBatch.FillRectangle(new RectangleF(_buttonStartPos.X, _buttonStartPos.Y + (_buttonBounds.Y + _buttonSpacing) * 2, _buttonBounds.X, _buttonBounds.Y), Color.Gray);
        spriteBatch.DrawString(Settings.Font, "CameraTileSamplePoints: " + CameraTileSamplePoints.ToString(), _buttonStartPos + _buttonBounds / 2 + new Vector2(0, _buttonBounds.Y + _buttonSpacing) * 2 - Settings.Font.MeasureString("CameraTileSamplePoints: " + CameraTileSamplePoints.ToString()) * 0.1f / 2, Color.White, 0f, new Vector2(), 0.1f, SpriteEffects.None, 0f);

        spriteBatch.FillRectangle(new RectangleF(_buttonStartPos.X, _buttonStartPos.Y + (_buttonBounds.Y + _buttonSpacing) * 3, _buttonBounds.X, _buttonBounds.Y), Color.Gray);
        spriteBatch.DrawString(Settings.Font, "InfiniteZoom: " + InfiniteZoom.ToString(), _buttonStartPos + _buttonBounds / 2 + new Vector2(0, _buttonBounds.Y + _buttonSpacing) * 3 - Settings.Font.MeasureString("InfiniteZoom: " + InfiniteZoom.ToString()) * 0.1f / 2, Color.White, 0f, new Vector2(), 0.1f, SpriteEffects.None, 0f);

        spriteBatch.FillRectangle(new RectangleF(_buttonStartPos.X, _buttonStartPos.Y + (_buttonBounds.Y + _buttonSpacing) * 4, _buttonBounds.X, _buttonBounds.Y), Color.Gray);
        spriteBatch.DrawString(Settings.Font, "ShowCollisionCheckArea: " + ShowCollisionCheckArea.ToString(), _buttonStartPos + _buttonBounds / 2 + new Vector2(0, _buttonBounds.Y + _buttonSpacing) * 4 - Settings.Font.MeasureString("ShowCollisionCheckArea: " + ShowCollisionCheckArea.ToString()) * 0.1f / 2, Color.White, 0f, new Vector2(), 0.1f, SpriteEffects.None, 0f);
    }
}