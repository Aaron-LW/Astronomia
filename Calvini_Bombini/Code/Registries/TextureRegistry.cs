using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Calvini_Bombini;

namespace Registries.TextureRegistry
{
    public static class TextureRegistry
    {
        public static List<Texture2D> TileTextures = new List<Texture2D>();

        public static Texture2D Grass;

        public static void LoadTextures(Game1 game1)
        {
            Grass = game1.Content.Load<Texture2D>("Grass");

            TileTextures.Add(Grass);
        }
    }
}