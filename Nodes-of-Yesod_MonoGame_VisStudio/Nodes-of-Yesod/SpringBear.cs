using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

public class SpringBear : Enemy
{
    public SpringBear() : base(0, 0, 0, null, null)
    {
    }


    public SpringBear(float posX, float posY, float speedX, float speedY, Texture2D sprite,
        List<Rectangle> walls, List<Rectangle> platforms)
        : base(posX, posY, speedX, speedY, sprite, walls)
    {
        mPlatforms = platforms;
        mFrameSize = new Point(64, 69);
        CurrentFrameX = 4;
        CurrentFrameY = 4;
        SheetSize = 4;
        timeSinceLastFrame = 0;
        millisecondsPerFrame = 100;
    }

    public Rectangle enemyRect => new((int)PositionX, (int)PositionY, 64, 69);

    public override void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        PositionX += SpeedX;
        PositionY += SpeedY;

        if (PositionY > 366 ||
            PositionY < 0)
            SpeedY *= -1;

        if (PositionX is > 750 or < 0) SpeedX *= -1;

        if (timeSinceLastFrame > millisecondsPerFrame)
        {
            timeSinceLastFrame -= millisecondsPerFrame;

            ++CurrentFrameX;
            if (CurrentFrameX == 9 && CurrentFrameY == 5)
            {
                CurrentFrameX = 6;
                CurrentFrameY = 4;
            }
            else if (CurrentFrameX >= SheetSize + 4)
            {
                CurrentFrameX = 4;
            }
        }

        // Wall Collision check
        if (mWalls.Count > 0)
            foreach (var wallRects in mWalls)
            {
                if (wallRects.X == 62 && PositionX <= 100)
                {
                    SpeedX *= 1;
                    PositionX += 5;
                }

                if (wallRects.X == 744 && PositionX >= 700)
                {
                    SpeedX *= -1;
                    PositionX -= 5;
                }
            }

        // Platform Collision check
        foreach (var platform in mPlatforms.Where(platform => enemyRect.Intersects(platform)))
            if (SpeedX > 0)
            {
                PositionX -= 5;
                SpeedX *= -1;
            }
            else
            {
                PositionX += 5;
                SpeedX *= 1;
            }
    }
}