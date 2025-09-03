using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Astronomia;
using Microsoft.Xna.Framework;

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
        public static Texture2D Pickaxe;
        public static Texture2D ItemSlot;
        public static Texture2D GradientTexture;
        public static Texture2D KeyboardKey;

        public static void LoadTextures(Game1 game1)
        {
            Grass = game1.Content.Load<Texture2D>("Grass");
            Selector = game1.Content.Load<Texture2D>("selector");
            Mensch = game1.Content.Load<Texture2D>("mensch");
            GrassEdge = game1.Content.Load<Texture2D>("GrassErde");
            Dirt = game1.Content.Load<Texture2D>("Dirt");
            Pickaxe = game1.Content.Load<Texture2D>("Pickaxe");
            ItemSlot = game1.Content.Load<Texture2D>("ItemSlot");
            KeyboardKey = game1.Content.Load<Texture2D>("KeyboardKey");

            TileTextures.Add(Grass);
            TileTextures.Add(GrassEdge);
            TileTextures.Add(Dirt);

            GradientTexture = new Texture2D(game1.GraphicsDevice, 4, 1);

            Color[] colors = new Color[4];
            colors[0] = Color.White;
            colors[1] = Color.Transparent;
            colors[2] = Color.Transparent;
            colors[3] = Color.Transparent;

            GradientTexture.SetData(colors);
        }
    }
}