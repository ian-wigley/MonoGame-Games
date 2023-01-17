using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod
{
    public class Bird : Enemy
    {

        public Bird(float xPos, float yPos, float speedX, Texture2D sprite, List<Rectangle> walls)
            : base(xPos, yPos, speedX, sprite, walls)
        {
            CurrentFrameX = 14;
            CurrentFrameY = 4;
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
                if (CurrentFrameX >= 14 + SheetSize)
                {
                    CurrentFrameX = 14;
                }
            }

            foreach (Rectangle wallRects in mWalls)
            {
                if (BirdRect.Intersects(wallRects))
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

        public Rectangle BirdRect
        {
            get
            {
                return new Rectangle((int)PositionX, (int)PositionY, 64, 69);
            }
        }
    }
}