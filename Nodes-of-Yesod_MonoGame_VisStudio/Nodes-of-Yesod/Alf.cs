using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

public class Alf : Enemy
{
    private bool turning;

    public Alf() : base(0, 0, 0, null, null)
    {
    }

    public Alf(float xPos, float yPos, float speedX, Texture2D sprite, List<Rectangle> walls)
        : base(xPos, yPos, speedX, sprite, walls)
    {
        CurrentFrameX = 0;
        CurrentFrameY = 3;
        SheetSize = 8;
        timeSinceLastFrame = 0;
        millisecondsPerFrame = 100;
    }

    public Rectangle AlfRect => new((int)PositionX, (int)PositionY, 64, 69);

    public override void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        PositionX += speed;

        if (timeSinceLastFrame > millisecondsPerFrame)
        {
            timeSinceLastFrame -= millisecondsPerFrame;

            ++CurrentFrameX;
            if ((CurrentFrameX >= SheetSize && !turning) || (turning && CurrentFrameX >= SheetSize + 3))
                CurrentFrameX = 0;
        }

        foreach (var wallRects in walls.Where(wallRects => AlfRect.Intersects(wallRects)))
            if (speed > 0)
            {
                Turning();
                turning = false;
                PositionX -= 5;
                speed = -1;
                facingLeft = true;
            }
            else
            {
                Turning();
                turning = false;
                PositionX += 5;
                speed = 1;
                facingLeft = false;
            }

        flip = facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    }

    private void Turning()
    {
        turning = true;
        speed = 0;
        CurrentFrameX = 9;

        while (CurrentFrameX < SheetSize + 3) CurrentFrameX++;
    }
}