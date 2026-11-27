using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

public class Fish : Enemy
{
    public Fish() : base(0, 0, 0, null, null)
    {
    }

    public Fish(float xPos, float yPos, float speedX, float speedY, Texture2D sprite, List<Rectangle> walls)
        : base(xPos, yPos, speedX, speedY, sprite, walls)
    {
        CurrentFrameX = 7;
        CurrentFrameY = 5;
        SheetSize = 6;
        timeSinceLastFrame = 0;
        millisecondsPerFrame = 100;
    }

    public Rectangle FishRect => new((int)PositionX, (int)PositionY, 64, 69);

    public override void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        PositionX += SpeedX;
        PositionY += SpeedY;

        if (timeSinceLastFrame > millisecondsPerFrame)
        {
            timeSinceLastFrame -= millisecondsPerFrame;
            ++CurrentFrameX;
            if (CurrentFrameX == 9 && CurrentFrameY == 5)
            {
                CurrentFrameX = 8;
                CurrentFrameY = 4;
            }
            else if (CurrentFrameX >= 8 + SheetSize)
            {
                CurrentFrameX = 8;
            }
        }

        if (PositionY > 400 ||
            PositionY < 0)
            SpeedY *= -1;

        foreach (var _ in walls.Where(wallRects => FishRect.Intersects(wallRects)))
            if (SpeedX > 0)
            {
                PositionX -= 5;
                SpeedX *= -1;
                facingLeft = true;
            }
            else
            {
                PositionX += 5;
                SpeedX *= -1;
                facingLeft = false;
            }

        flip = facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    }
}