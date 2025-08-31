using Registries.TextureRegistry;

public static class ItemRegistry
{
    public static Item Pickaxe;
    public static Item Grass;
    public static Item Dirt;

    public static void Start()
    {
        Pickaxe = new Item("Pickaxe", TextureRegistry.Pickaxe);
        Grass = new Item("Grass", TextureRegistry.Grass);
        Dirt = new Item("Dirt", TextureRegistry.Dirt);
    }    
}