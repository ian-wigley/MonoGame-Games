using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetPacReloaded;

public class Particle : BaseObject
{
    private const int LifeSpan = 20;

    private readonly Color[] _colors =
    [
        Color.White,
        Color.Yellow,
        Color.Orange,
        Color.Brown,
        Color.Red,
        Color.Brown,
        Color.Brown,
        Color.Black
    ];

    private int _count;
    private int _length;
    private int _particleX;
    private int _particleY;
    private float _scale = 1.0f;

    public Particle(Texture2D image)
    {
        Image = image;
    }

    public void Update(int x, int y, bool facingLeft, bool showParticles)
    {
        if (showParticles)
        {
            if (_particleY < _length) _particleY++;
            if (_particleY >= _length)
            {
                if (facingLeft)
                    _particleX = x + 20 + Rand.Next(0, 10);
                else
                    _particleX = x + Rand.Next(0, 10);

                _length = y + 40 + LifeSpan;
                _particleY = y + 40 + Rand.Next(0, 10);
            }

            if (_particleY == _length - 20)
            {
                _count = 0;
                _scale = 1.0f;
            }

            if (_particleY == _length - 15)
            {
                _count = 1;
                _scale = 0.9f;
            }

            if (_particleY == _length - 10)
            {
                _count = 2;
                _scale = 0.8f;
            }

            if (_particleY == _length - 5)
            {
                _count = 3;
                _scale = 0.7f;
            }

            if (_particleY == _length)
            {
                _count = 4;
                _scale = 0.45f;
            }
        }

        // Hide the particles when not in use
        if (showParticles) return;
        _count = 7;
        _particleX = 0;
        _particleY = 0;
        _length = 0;
    }

    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: camera.Transform);
        var particleLocation = new Vector2(_particleX, _particleY);
        spriteBatch.Draw(Image, particleLocation, null, _colors[_count], 0, new Vector2(0, 9), _scale,
            SpriteEffects.None, 0);
        spriteBatch.End();
    }
}