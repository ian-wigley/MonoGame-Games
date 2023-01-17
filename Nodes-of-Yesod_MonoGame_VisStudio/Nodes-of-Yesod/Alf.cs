using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nodes_of_Yesod
{
    public class Alf : Enemy
    {
        private bool turning = false;

        public Alf(float xPos, float yPos, float speedX, Texture2D sprite, List<Rectangle> walls)
            : base(xPos, yPos, speedX, sprite, walls)
        {
            CurrentFrameX = 0;
            CurrentFrameY = 3;
            SheetSize = 8;
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
                if (CurrentFrameX >= SheetSize && !turning)
                {
                    CurrentFrameX = 0;
                }
                else if (turning && CurrentFrameX >= SheetSize + 3)
                {
                        CurrentFrameX = 0;
                }
            }

            foreach (Rectangle wallRects in mWalls)
            {
                if (AlfRect.Intersects(wallRects))
                {
                    if (mSpeed > 0)
                    {
                        Turning();
                        turning = false;
                        PositionX -= 5;
                        mSpeed = -1;
                        facingLeft = true;
                    }
                    else
                    {
                        Turning();
                        turning = false;
                        PositionX += 5;
                        mSpeed = 1;
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

        private void Turning()
        {
            turning = true;
            mSpeed = 0;
            CurrentFrameX = 9;

            while (CurrentFrameX < SheetSize + 3)
            {
                CurrentFrameX++;
            }
        }

        public Rectangle AlfRect
        {
            get
            {
                return new Rectangle((int)PositionX, (int)PositionY, 64, 69);
            }
        }
    }
}