using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod
{
    public class Plant : Enemy
    {
        private int mPlantX;
        private int mPlantY;
        private int mPlantFrame = 17;

        public Plant(float xPos, float yPos, float speedX, float speedY, Texture2D sprite, List<Rectangle> walls)
            : base(xPos, yPos, speedX, speedY, sprite, walls)
        {
            mSprite = sprite;
            mPlantX = 2;
            mPlantY = 9;
            SheetSize = 3;
            timeSinceLastFrame = 0;
        }

        public override void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > 0.6)
            {
                timeSinceLastFrame = 0;
                mPlantFrame++;
            }
            if (mPlantFrame >= 6)
            {
                mPlantFrame = 0;
            }
        }

        new public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mSprite, new Vector2(mPlantX, mPlantY), PlantRect, Color.White);
        }

        public Rectangle PlantRect
        {
            get
            {
                return new Rectangle(mPlantFrame * mSpriteWidth, 9 * mSpriteHeight, mSpriteWidth, mSpriteHeight);
            }
        }
    }
}
