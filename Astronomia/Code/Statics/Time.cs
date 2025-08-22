using Microsoft.Xna.Framework;

public static class Time
{
    public static float DeltaTime;

    public static void Update(GameTime gameTime)
    {
        DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}
