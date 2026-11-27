using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

internal class RedSpaceman : Enemy
{
    private int mRedSpacemanFrame = 17;
    private readonly int mRedSpacemanX;
    private readonly int mRedSpacemanY;

    public RedSpaceman() : base(0, 0, 0, null, null)
    {
    }

    public RedSpaceman(float xPos, float yPos, float speedX, float speedY, Texture2D sprite, List<Rectangle> walls)
        : base(xPos, yPos, speedX, speedY, sprite, walls)
    {
        mRedSpacemanX = 2;
        mRedSpacemanY = 9;
        SheetSize = 3;
        timeSinceLastFrame = 0;
    }

    public Rectangle RedSpacemanRect =>
        new(mRedSpacemanFrame * SpriteWidth, 9 * SpriteHeight, SpriteWidth, SpriteHeight);

    public override void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        if (timeSinceLastFrame > 0.6)
        {
            timeSinceLastFrame = 0;
            mRedSpacemanFrame++;
        }

        if (mRedSpacemanFrame >= 6) mRedSpacemanFrame = 0;
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, new Vector2(mRedSpacemanX, mRedSpacemanY), RedSpacemanRect, Color.White);
    }
}