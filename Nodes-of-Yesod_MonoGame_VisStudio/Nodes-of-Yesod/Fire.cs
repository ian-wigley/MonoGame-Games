using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod
{
    public class Fire : Enemy
    {

        public Fire(float xPos, float yPos, float speedX, Texture2D sprite, List<Rectangle> walls)
            : base(xPos, yPos, speedX, sprite, walls)
        {
            CurrentFrameX = 10;
            CurrentFrameY = 5;
            SheetSize = 4;
            timeSinceLastFrame = 0;
            millisecondsPerFrame = 100;
        }

        public override void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            PositionX += mSpeed;

            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;

                ++CurrentFrameX;
                if (CurrentFrameX >= 10 + SheetSize)
                {
                    CurrentFrameX = 10;
                }
            }

            foreach (Rectangle wallRects in mWalls)
            {
                if (FireRect.Intersects(wallRects))
                {
                    if (mSpeed > 0)
                    {
                        PositionX -= 5;
                        mSpeed *= -1;
                    }
                    else
                    {
                        PositionX += 5;
                        mSpeed *= -1;
                    }
                }
            }
        }

        public Rectangle FireRect
        {
            get
            {
                return new Rectangle((int)PositionX, (int)PositionY, 64, 69);
            }
        }
    }
}
