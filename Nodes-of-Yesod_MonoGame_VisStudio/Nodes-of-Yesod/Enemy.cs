using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nodes_of_Yesod
{
    public abstract class Enemy
    {
        protected Point mFrameSize;
        protected int mFrame;
        protected const int mSpriteWidth = 64;
        protected const int mSpriteHeight = 69;
        protected List<Rectangle> mWalls;
        protected List<Rectangle> mPlatforms;
        protected Texture2D mSprite;
        protected int timeSinceLastFrame;
        protected int millisecondsPerFrame;
        protected float mSpeed;
        protected bool facingLeft = false;
        protected SpriteEffects flip = SpriteEffects.None;

        protected Enemy(float posX, float posY, float speedX, float speedY, Texture2D sprite,
            List<Rectangle> walls)
        {
            PositionX = posX;
            PositionY = posY;
            SpeedX = speedX;
            SpeedY = speedY;
            mSprite = sprite;
            mWalls = walls;
            mFrameSize = new Point(mSpriteWidth, mSpriteHeight);
            CurrentFrameX = 7;
            CurrentFrameY = 5;
            SheetSize = 4;
            mFrame = 0;
        }

        protected Enemy(float posX, float posY, float speed, Texture2D sprite, List<Rectangle> walls)
        {
            PositionX = posX;
            PositionY = posY;
            SpeedX = speed;
            mSprite = sprite;
            mWalls = walls;
            mFrameSize = new Point(64, 69);
            CurrentFrameX = 4;
            CurrentFrameY = 4;
            SheetSize = 4;
        }

        public abstract void Update(GameTime gameTime);

        public void Update(GameTime gameTime, Vector2 charliePos)
        {
            // Method intentionally left empty.
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mSprite, new Vector2(PositionX, PositionY),
                       new Rectangle(CurrentFrameX * mFrameSize.X,
                           CurrentFrameY * mFrameSize.Y,
                           mFrameSize.X, mFrameSize.Y), Color.White, 0, Vector2.Zero, 1.0f, flip, 0);
        }

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float SpeedX { get; set; }

        public float SpeedY { get; set; }

        public int CurrentFrameX { get; set; }

        public int CurrentFrameY { get; set; }

        public int SheetSize { get; set; }
    }
}
