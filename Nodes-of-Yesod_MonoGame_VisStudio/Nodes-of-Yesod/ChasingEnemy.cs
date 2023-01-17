using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nodes_of_Yesod
{
    public class ChasingEnemy : Enemy
    {

        public ChasingEnemy(float xPos, float yPos, float speedX, float speedY, Texture2D sprite, List<Rectangle> walls)
            : base(xPos, yPos, speedX, speedY, sprite, walls)
        {
            CurrentFrameX = 4;
            CurrentFrameY = 5;
            SheetSize = 3;
            timeSinceLastFrame = 0;
            millisecondsPerFrame = 100;
        }

        public override void Update(GameTime gameTime) { }

        public new void Update(GameTime gameTime, Vector2 charliePos)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (charliePos.X < PositionX)
            {
                PositionX -= SpeedX;
            }
            else if (charliePos.X > PositionX)
            {
                PositionX += SpeedX;
            }

            if (charliePos.Y < PositionY)
            {
                PositionY -= SpeedY;
            }
            else if (charliePos.Y > PositionY)
            {
                PositionY += SpeedY;
            }

            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;

                ++CurrentFrameX;
                if (CurrentFrameX >= 4 + SheetSize)
                {
                    CurrentFrameX = 4;
                }
            }
        }
    }
}
