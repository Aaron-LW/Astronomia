using Registries.TextureRegistry;
using Microsoft.Xna.Framework.Graphics;

public static class ItemRegistry
{
    public static Item Blank;
    public static Item Pickaxe;
    public static Item Grass;
    public static Item Dirt;

    public static void Start()
    {
        Blank = new Item("", null);
        Pickaxe = new Item("Pickaxe", TextureRegistry.Pickaxe);
        Grass = new Item("Grass", TextureRegistry.Grass);
        Dirt = new Item("Dirt", TextureRegistry.Dirt);
    }    
}