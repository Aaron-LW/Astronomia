using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Registries.TextureRegistry;

public static class EntitySystem
{
    public static List<Entity> Entities = new List<Entity>();
    public static Entity Player;

    public static void Start()
    {
        Player = CreateEntity(TextureRegistry.Mensch, [
            new PositionComponent(100, 100),
            new VelocityComponent(100, 0),
            new CharacterControllerComponent(200),
            new GravityComponent(),
            new ColliderComponent(default, true),
        ]);

    }

    public static void Update()
    {
        MovementSystem.Update();
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (Entity entity in Entities)
        {
            entity.Draw(spriteBatch);
        }

        MovementSystem.Draw(spriteBatch);
    }

    public static Entity CreateEntity(Texture2D texture, Component[] components)
    {
        Entities.Add(new Entity(texture, components));
        return Entities[^1];
    }
}