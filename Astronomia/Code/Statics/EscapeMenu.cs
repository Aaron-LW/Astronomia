using System;
using System.Collections.Generic;
using Astronomia;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public static class EscapeMenu
{
    private static List<AnimatedText> _animatedTexts = new List<AnimatedText>();
    private static float _counter = 0;
    private static float _counterBase = 0.06f;
    private static int _textIndex = 0;
    private static bool _active = false;

    public static void Start()
    {
        _animatedTexts.Add(new AnimatedText("Exit", new Vector2(-150, 200), new Vector2(75, 200), 30, Game1.CloseGame, 0.1f));
        _animatedTexts.Add(new AnimatedText("Controls", new Vector2(-300, 290), new Vector2(75, 290), 30, Game1.CloseGame, 0.1f));
    }

    public static void Update()
    {
        for (int i = 0; i < _animatedTexts.Count; i++)
        {
            _animatedTexts[i].AnimationStep();
        }

        if (Input.IsKeyPressed(Keys.Escape))
        {
            _active = !_active;
            if (!_active)
            {
                foreach (AnimatedText animatedText in _animatedTexts)
                {
                    animatedText.Active = false;
                }
            }
        }

        if (_active)
        {
            _counter -= Time.DeltaTime;
        }

        if (_counter <= 0 && _textIndex < _animatedTexts.Count)
        {
            _counter = _counterBase;
            _textIndex++;
        }

        if (!_active)
        {
            _counter = 0;
            _textIndex = 0;
        }
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < _animatedTexts.Count; i++)
        {
            _animatedTexts[i].Draw(spriteBatch);
            if (i < _textIndex)
            {
                _animatedTexts[i].Active = true;
            }
        }
    }
}