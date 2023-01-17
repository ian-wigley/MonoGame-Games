using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nodes_of_Yesod
{
    class RedSpaceman : Enemy
    {
        private int mRedSpacemanX = 0;
        private int mRedSpacemanY = 0;
        private int mRedSpacemanFrame = 17;

        public RedSpaceman(float xPos, float yPos, float speedX, float speedY, Texture2D sprite, List<Rectangle> walls)
            : base(xPos, yPos, speedX, speedY, sprite, walls)
        {
            mRedSpacemanX = 2;
            mRedSpacemanY = 9;
            SheetSize = 3;
            timeSinceLastFrame = 0;
        }

        public override void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > 0.6)
            {
                timeSinceLastFrame = 0;
                mRedSpacemanFrame++;
            }
            if (mRedSpacemanFrame >= 6)
            {
                mRedSpacemanFrame = 0;
            }
        }

        new public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mSprite, new Vector2(mRedSpacemanX, mRedSpacemanY), RedSpacemanRect, Color.White);
        }

        public Rectangle RedSpacemanRect
        {
            get
            {
                return new Rectangle(mRedSpacemanFrame * mSpriteWidth, 9 * mSpriteHeight, mSpriteWidth, mSpriteHeight);
            }
        }
    }
}
