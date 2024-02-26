using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Animation
{
    private readonly float _frameTime;
    private readonly List<Rectangle> _sourceRectangles = [];
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private bool _active = true;
    private int _frame;
    private float _frameTimeLeft;

    public Animation(SpriteBatch spriteBatch, Texture2D texture, int framesX, int framesY, float frameTime, int row = 1)
    {
        _texture = texture;
        _frameTime = frameTime;
        _frameTimeLeft = _frameTime;
        _spriteBatch = spriteBatch;
        var frameWidth = _texture.Width / framesX;
        var frameHeight = _texture.Height / framesY;
        for (var lines = 0; lines < row; lines++)
        for (var i = 0; i < framesX; i++)
            _sourceRectangles.Add(new Rectangle(i * frameWidth, lines * frameHeight, frameWidth, frameHeight));
    }

    private void Stop()
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
        if (!_active) return;
        if (_sourceRectangles.Count <= _frame + 1) Stop();

        _frameTimeLeft -= 3;

        if (!(_frameTimeLeft <= 0)) return;
        _frameTimeLeft += _frameTime;
        _frame++;
    }

    public void Draw(Vector2 pos)
    {
        if (!_active) return;
        _spriteBatch.Draw(_texture, pos, _sourceRectangles[_frame], Color.White, 0, Vector2.Zero, Vector2.One,
            SpriteEffects.None, 1);
    }
}