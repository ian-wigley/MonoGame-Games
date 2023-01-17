using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nodes_of_Yesod
{
    public class WoodLouse : Enemy
    {
        public WoodLouse(float xPos, float yPos, float speedX, Texture2D sprite, List<Rectangle> walls)
            : base(xPos, yPos, speedX, sprite, walls)
        {
            CurrentFrameX = 0;
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
                if (CurrentFrameX >= CurrentFrameX + SheetSize)
                {
                    CurrentFrameX = 0;
                }
            }

            foreach (Rectangle wallRects in mWalls)
            {
                if (WoodLouseRect.Intersects(wallRects))
                {
                    if (mSpeed > 0)
                    {
                        PositionX -= 5;
                        mSpeed *= -1;
                        facingLeft = true;
                    }
                    else
                    {
                        PositionX += 5;
                        mSpeed *= -1;
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

        public Rectangle WoodLouseRect
        {
            get
            {
                return new Rectangle((int)PositionX, (int)PositionY, 64, 69);
            }
        }
    }
}
