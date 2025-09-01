using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Registries.TextureRegistry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;

public static class EntitySystem
{
    public static List<Entity> Entities = new List<Entity>();
    public static Entity Player;

    public static void Start()
    {
        Player = CreateEntity(TextureRegistry.Mensch, [
            new PositionComponent(100, 100),
            new VelocityComponent(100, 0),
            new CharacterControllerComponent(PlayerSettings.MoveSpeed, PlayerSettings.JumpStrength),
            new GravityComponent(PlayerSettings.YAcceleration),
            new ColliderComponent(PlayerSettings.BoundingBox, PlayerSettings.DrawBoundingBox),
            new ContainerComponent(new Vector2(100, 200), 21, 7, 5, [new ItemStack(ItemRegistry.Pickaxe, 0), new ItemStack(ItemRegistry.Dirt, 10), new ItemStack(ItemRegistry.Grass, 23), new ItemStack(ItemRegistry.Dirt, 128)]),
        ]);

    }

    public static void Update()
    {
        if (Input.IsKeyPressed(Keys.E))
        {
            Player.TryGetComponent(out ContainerComponent containerComponent);
            if (containerComponent != null)
            {
                containerComponent.Opened = !containerComponent.Opened;
            }
        }

        if (!GridSystem.LevelEditor) { MovementSystem.Update(); }
        ContainerSystem.Update();
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (Entity entity in Entities)
        {
            entity.Draw(spriteBatch);
        }

        if (!GridSystem.LevelEditor) { MovementSystem.Draw(spriteBatch); }
        ContainerSystem.Draw(spriteBatch);
    }

    public static Entity CreateEntity(Texture2D texture, Component[] components)
    {
        Entities.Add(new Entity(texture, components));
        return Entities[^1];
    }
}