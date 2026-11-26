using Microsoft.Xna.Framework;

namespace JetPacReloaded;

public class Camera
{
    private Matrix _position;
    public Matrix Transform { get; private set; }

    public Vector2 CameraPos => new(Transform.Translation.X, Transform.Translation.Y);

    public void Follow(JetMan target)
    {
        var x = 400.0f;
        var y = 300.0f;
        x = target.JetManPosition.X switch
        {
            >= 380 and <= 2160 => -target.JetManPosition.X - target.JetManRect.Width / 2,
            < 380 => -380 - target.JetManRect.Width / 2,
            > 2160 => -2160 - target.JetManRect.Width / 2,
            _ => x
        };

        y = target.JetManPosition.Y switch
        {
            > 270 and < 620 => -target.JetManPosition.Y - target.JetManRect.Height / 2,
            <= 270 => -270 - target.JetManRect.Height / 2,
            >= 620 => -620 - target.JetManRect.Height / 2,
            _ => y
        };

        _position = Matrix.CreateTranslation(x, y, 0);

        // The offset value is used to move the camera to centred around the player
        var offset = Matrix.CreateTranslation(400, 300, 0);
        Transform = _position * offset;
    }
}