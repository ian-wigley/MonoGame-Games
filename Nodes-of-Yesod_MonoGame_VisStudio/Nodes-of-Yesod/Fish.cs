using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nodes_of_Yesod
{
    public class Fish : Enemy
    {

        public Fish(float xPos, float yPos, float speedX, float speedY, Texture2D sprite, List<Rectangle> walls)
            : base(xPos, yPos, speedX, speedY, sprite, walls)
        {
            CurrentFrameX = 7;
            CurrentFrameY = 5;
            SheetSize = 6;
            timeSinceLastFrame = 0;
            millisecondsPerFrame = 100;
        }

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
                else if(CurrentFrameX >= 8 + SheetSize)
                {
                    CurrentFrameX = 8;
                }
            }

            if (PositionY > 400 ||
                    PositionY < 0)
            {
                SpeedY *= -1;
            }

            foreach (Rectangle wallRects in mWalls)
            {
                if (FishRect.Intersects(wallRects))
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
            }

            if (facingLeft)
            {
                flip = SpriteEffects.FlipHorizontally;
            }
            else
            {
                flip = SpriteEffects.None;
            }
        }

        public Rectangle FishRect
        {
            get
            {
                return new Rectangle((int)PositionX, (int)PositionY, 64, 69);
            }
        }
    }
}
