using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nodes_of_Yesod
{
    public class BlueThingy : Enemy
    {

        public BlueThingy(float xPos, float yPos, float speedX, float speedY, Texture2D sprite,
            List<Rectangle> walls, List<Rectangle> platforms)
            : base(xPos, yPos, speedX, speedY, sprite, walls)
        {
            mPlatforms = platforms;
            CurrentFrameX = 7;
            CurrentFrameY = 5;
            SheetSize = 7;
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
                    CurrentFrameX = 12;
                    CurrentFrameY = 3;
                }
                else if (CurrentFrameX >= SheetSize + 12)
                {
                    CurrentFrameX = 12;
                }
            }

            if (PositionY > 366 ||
                    PositionY < 0)
            {
                SpeedY *= -1;
            }

            if (PositionX > 750 || PositionX < 0)
            {
                SpeedX *= -1;
            }

            //if (mWalls.Count > 0)
            //{
            //    foreach (Rectangle wallRects in mWalls)
            //    {
            //        if (wallRects.X == 62 && PositionX <= 100)
            //        {
            //            SpeedX *= -1;
            //            PositionX += 5;
            //        }

            //        if (wallRects.X == 744 && PositionX >= 700)
            //        {
            //            SpeedX *= -1;
            //            PositionX -= 5;
            //        }
            //    }
            //}

            foreach (Rectangle platform in mPlatforms)
            {
                if (BlueThingyRect.Intersects(platform))
                {
                    //if (BlueThingyRect.Top >= platform.Bottom + 5 ||
                    //    BlueThingyRect.Top <= platform.Bottom + 5)
                    //{
                    //    PositionY += 5;
                    //    SpeedY *= -1;
                    //}
                    //if (BlueThingyRect.Right >= platform.Left + 3 ||
                    //    BlueThingyRect.Right <= platform.Left + 3)
                    //{
                    //    PositionX -= 5;
                    //    SpeedX *= -1;
                    //}

                    //if (BlueThingyRect.Left >= platform.Right + 3 ||
                    //    BlueThingyRect.Left <= platform.Right + 3)
                    //{
                    //    SpeedX *= -1;
                    //}

                    // # <- *
                    if (BlueThingyRect.Left == platform.Right - 3 ||
                        BlueThingyRect.Left == platform.Right - 2 ||
                        BlueThingyRect.Left == platform.Right - 1) 
                    {
                        PositionX += 5;
                        SpeedX *= -1;
                        break;
                    }

                    // * -> #
                    if (BlueThingyRect.Right == platform.Left - 3)
                    {
                        SpeedX *= -1;
                    }

                    if(BlueThingyRect.Top == platform.Bottom + 3)
                    {
                        SpeedY *= -1;
                    }

                    if (BlueThingyRect.Bottom == platform.Top - 3)
                    {
                        SpeedY *= -1;
                    }

                }
            }
        }
        public Rectangle BlueThingyRect
        {
            get
            {
                return new Rectangle((int)PositionX + 20, (int)PositionY + 40, 20, 30);
            }
        }
    }
}
