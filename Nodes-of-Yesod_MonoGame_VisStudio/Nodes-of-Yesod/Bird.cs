using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

public class Bird : Enemy
{
    public Bird() : base(0, 0, 0, null, null)
    {
    }

    public Bird(float xPos, float yPos, float speedX, Texture2D sprite, List<Rectangle> walls)
        : base(xPos, yPos, speedX, sprite, walls)
    {
        CurrentFrameX = 14;
        CurrentFrameY = 4;
        SheetSize = 4;
        timeSinceLastFrame = 0;
        millisecondsPerFrame = 100;
    }

    public Rectangle BirdRect => new((int)PositionX, (int)PositionY, 64, 69);

    public override void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        PositionX += speed;

        if (timeSinceLastFrame > millisecondsPerFrame)
        {
            timeSinceLastFrame -= millisecondsPerFrame;

            ++CurrentFrameX;
            if (CurrentFrameX >= 14 + SheetSize) CurrentFrameX = 14;
        }

        foreach (var wallRects in walls)
            if (BirdRect.Intersects(wallRects))
            {
                if (speed > 0)
                {
                    PositionX -= 5;
                    speed *= -1;
                    facingLeft = true;
                }
                else
                {
                    PositionX += 5;
                    speed *= -1;
                    facingLeft = false;
                }
            }

        flip = facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    }
}