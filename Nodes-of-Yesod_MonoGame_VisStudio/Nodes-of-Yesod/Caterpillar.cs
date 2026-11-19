using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

public class Caterpillar : Enemy
{
    public Caterpillar() : base(0, 0, 0, null, null)
    {
    }

    public Caterpillar(float xPos, float yPos, float speedX, Texture2D sprite, List<Rectangle> walls)
        : base(xPos, yPos, speedX, sprite, walls)
    {
        CurrentFrameX = 0;
        CurrentFrameY = 7;
        SheetSize = 6;
        timeSinceLastFrame = 0;
        millisecondsPerFrame = 100;
    }

    public Rectangle CaterpillarRect => new((int)PositionX, (int)PositionY, 64, 69);

    public override void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        PositionX += mSpeed;

        if (timeSinceLastFrame > millisecondsPerFrame)
        {
            timeSinceLastFrame -= millisecondsPerFrame;
            ++CurrentFrameX;
            if (CurrentFrameX >= SheetSize) CurrentFrameX = 0;
        }

        foreach (var wallRects in mWalls)
            if (CaterpillarRect.Intersects(wallRects))
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

        flip = facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    }
}