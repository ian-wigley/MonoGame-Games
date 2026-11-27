using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

public class WoodLouse : Enemy
{
    public WoodLouse() : base(0, 0, 0, null, null)
    {
    }

    public WoodLouse(float xPos, float yPos, float speedX, Texture2D sprite, List<Rectangle> walls)
        : base(xPos, yPos, speedX, sprite, walls)
    {
        CurrentFrameX = 0;
        CurrentFrameY = 5;
        SheetSize = 4;
        timeSinceLastFrame = 0;
        millisecondsPerFrame = 100;
    }

    public Rectangle WoodLouseRect => new((int)PositionX, (int)PositionY, 64, 69);

    public override void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        PositionX += speed;

        if (timeSinceLastFrame > millisecondsPerFrame)
        {
            timeSinceLastFrame -= millisecondsPerFrame;

            ++CurrentFrameX;
            if (CurrentFrameX >= CurrentFrameX + SheetSize) CurrentFrameX = 0;
        }

        foreach (var _ in walls.Where(wallRects => WoodLouseRect.Intersects(wallRects)))
            if (speed > 0)
            {
                PositionX -= 5;
                speed *= -1;
                facingLeft = true;
            }
            else
            {
                PositionX += 5;
                speed *= -1;
                facingLeft = false;
            }

        flip = facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    }
}