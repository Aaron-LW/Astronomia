using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class Entity
{
    public List<Component> Components = new List<Component>();
    public Texture2D Texture { get; }

    public Entity(Texture2D texture, Component[] components = null)
    {
        Texture = texture;

        foreach (Component component in components)
        {
            AddComponent(component);
        }
    }

    public void AddComponent(Component component)
    {
        if (!HasComponent(component))
        {
            Components.Add(component);
        }
        else
        {
            Console.WriteLine("Entity already has Component " + component.GetType());
        }
    }

    public void RemoveComponent(Component component)
    {
        for (int i = 0; i < Components.Count; i++)
        {
            if (Components[i] == component)
            {
                Components.RemoveAt(i);
            }
        }
    }

    public Component GetComponent<Component>()
    {
        for (int i = 0; i < Components.Count; i++)
        {
            if (Components[i] is Component component)
            {
                return component;
            }
        }

        return default;
    }

    public bool TryGetComponent<T>(out T component) where T : Component
    {
        for (int i = 0; i < Components.Count; i++)
        {
            if (Components[i] is T matched)
            {
                component = matched;
                return true;
            }
        }

        component = null;
        return false;
    }

    public bool HasComponent(Component component)
    {
        for (int i = 0; i < Components.Count; i++)
        {
            Type type = component.GetType();
            if (Components[i].GetType() == type)
            {
                return true;
            }
        }

        return false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 position = new Vector2();

        if (TryGetComponent<PositionComponent>(out var positionComponent)) { position = positionComponent.Position; }

        spriteBatch.Draw(Texture, (position - Camera.GetPosition()) * Camera.Zoom, null, Color.White, 0f, new Vector2(), Settings.GlobalScale * Camera.Zoom, SpriteEffects.None, 0f);
    }
}