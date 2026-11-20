using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

internal class WhirlWind : Enemy
{
    private int mWhirlWindFrame = 17;
    private readonly int mWhirlWindX;
    private readonly int mWhirlWindY;

    public WhirlWind() : base(0, 0, 0, null, null)
    {
    }

    public WhirlWind(float xPos, float yPos, float speedX, float speedY, Texture2D sprite, List<Rectangle> walls)
        : base(xPos, yPos, speedX, speedY, sprite, walls)
    {
        mWhirlWindX = 2;
        mWhirlWindY = 9;
        SheetSize = 3;
        timeSinceLastFrame = 0;
    }

    public Rectangle WhirlWindRect =>
        new(mWhirlWindFrame * MSpriteWidth, 9 * MSpriteHeight, MSpriteWidth, MSpriteHeight);

    public override void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        if (timeSinceLastFrame > 0.6)
        {
            timeSinceLastFrame = 0;
            mWhirlWindFrame++;
        }

        if (mWhirlWindFrame >= 6) mWhirlWindFrame = 0;
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(mSprite, new Vector2(mWhirlWindX, mWhirlWindY), WhirlWindRect, Color.White);
    }
}