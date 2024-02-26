using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Animation
{
    private readonly Texture2D _texture;
    private readonly List<Rectangle> _sourceRectangles = [];
    private readonly int _frames;
    private int _frame;
    private readonly float _frameTime;
    private float _frameTimeLeft;
    private bool _active = true;
    private SpriteBatch _spriteBatch;
    public bool repeat = false;

    public Animation(SpriteBatch spriteBatch, Texture2D texture, int framesX, int framesY, float frameTime, int row = 1)
    {
        _texture = texture;
        _frameTime = frameTime;
        _frameTimeLeft = _frameTime;
        _frames = framesX;
        _spriteBatch = spriteBatch;
        var frameWidth = _texture.Width / framesX;
        var frameHeight = _texture.Height / framesY;
        for (int lines = 0; lines < row; lines++) {
            for (int i = 0; i < _frames; i++)
            {
                _sourceRectangles.Add(new(i * frameWidth, lines * frameHeight, frameWidth, frameHeight));
            }
        }
    }

    public void Stop()
    {
        _active = false;
    }

    public void Start()
    {
        _active = true;
    }

    public void Reset()
    {
        _frame = 0;
        _frameTimeLeft = _frameTime;
    }

    public void Update(GameTime gameTime)
    {
        if (repeat && !_active) {
            _active = true;
            Reset();
        }
        if (!_active) return;
        if (_sourceRectangles.Count <= _frame + 1) Stop();

        _frameTimeLeft -= 3;

        if (_frameTimeLeft <= 0)
        {
            _frameTimeLeft += _frameTime;
            _frame++;
        }
    }

    public void Draw(Vector2 pos)
    {
        if (!_active) return;
        _spriteBatch.Draw(_texture, pos, _sourceRectangles[_frame], Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);
    }
}