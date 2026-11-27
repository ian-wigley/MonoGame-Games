using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

public class GreenMeanie : Enemy
{
    public GreenMeanie() : base(0, 0, 0, null, null)
    {
    }

    public GreenMeanie(float xPos, float yPos, float speedX, Texture2D sprite, List<Rectangle> walls)
        : base(xPos, yPos, speedX, sprite, walls)
    {
        CurrentFrameX = 0;
        CurrentFrameY = 4;
        SheetSize = 4;
        timeSinceLastFrame = 0;
        millisecondsPerFrame = 100;
        facingLeft = false;
    }

    public Rectangle GreenMeanieRect => new((int)PositionX, (int)PositionY, 64, 69);

    public override void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        PositionX += SpeedX;

        if (timeSinceLastFrame > millisecondsPerFrame)
        {
            timeSinceLastFrame -= millisecondsPerFrame;

            ++CurrentFrameX;
            if (CurrentFrameX >= SheetSize) CurrentFrameX = 0;
        }

        if (PositionX > 750)
        {
            SpeedX *= -1;
            facingLeft = true;
        }

        foreach (var wallRects in walls)
            if (GreenMeanieRect.Intersects(wallRects))
            {
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
            }

        flip = facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    }
}