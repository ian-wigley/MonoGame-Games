using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

public class Fire : Enemy
{
    public Fire() : base(0, 0, 0, null, null)
    {
    }

    public Fire(float xPos, float yPos, float speedX, Texture2D sprite, List<Rectangle> walls)
        : base(xPos, yPos, speedX, sprite, walls)
    {
        CurrentFrameX = 10;
        CurrentFrameY = 5;
        SheetSize = 4;
        timeSinceLastFrame = 0;
        millisecondsPerFrame = 100;
        rect = new Rectangle((int)PositionX, (int)PositionY, 64, 69);
    }
    
    public override void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        PositionX += speed;

        if (timeSinceLastFrame > millisecondsPerFrame)
        {
            timeSinceLastFrame -= millisecondsPerFrame;

            ++CurrentFrameX;
            if (CurrentFrameX >= 10 + SheetSize) CurrentFrameX = 10;
        }

        foreach (var _ in walls.Where(wallRects => rect.Intersects(wallRects)))
            if (speed > 0)
            {
                PositionX -= 5;
                speed *= -1;
            }
            else
            {
                PositionX += 5;
                speed *= -1;
            }
    }
}