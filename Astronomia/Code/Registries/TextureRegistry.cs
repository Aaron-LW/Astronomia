using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Astronomia;

namespace Registries.TextureRegistry
{
    public static class TextureRegistry
    {
        public static List<Texture2D> TileTextures = new List<Texture2D>();

        public static Texture2D Grass;
        public static Texture2D GrassEdge;
        public static Texture2D Selector;
        public static Texture2D Mensch;
        public static Texture2D Dirt;

        public static void LoadTextures(Game1 game1)
        {
            Grass = game1.Content.Load<Texture2D>("Grass");
            Selector = game1.Content.Load<Texture2D>("selector");
            Mensch = game1.Content.Load<Texture2D>("mensch");
            GrassEdge = game1.Content.Load<Texture2D>("GrassErde");
            Dirt = game1.Content.Load<Texture2D>("Dirt");

            TileTextures.Add(Grass);
            TileTextures.Add(GrassEdge);
            TileTextures.Add(Dirt);
        }
    }
}