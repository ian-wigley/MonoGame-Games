using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nodes_of_Yesod
{
    public class Mole
    {
        private int mMoleFrame = 0;
        private int mSpriteWidth = 64;
        private int mSpriteHeight = 69;
        private int[,] mtoTheUndergound;
        private int mBelowScreenCounter;
        private Texture2D mSprites;
        private List<Rectangle> mWalls;
        private List<Rectangle> mEdibleWalls;

        protected double animTimer = 0;
        protected const double elapsedSecs = 0.1f;
        private SpriteEffects flip = SpriteEffects.None;

        public Mole(Texture2D sprite, List<Rectangle> walls, List<Rectangle> edibleWalls, int[,] toTheUndergound)
        {
            mSprites = sprite;
            mWalls = walls;
            mEdibleWalls = edibleWalls;
            mtoTheUndergound = toTheUndergound;
        }


        public void Update(GameTime gameTime, int belowScreenCounter, int screenCounter)
        {
            mBelowScreenCounter = belowScreenCounter;
            animTimer += elapsedSecs;
            if (animTimer > 1.2)
            {
                animTimer = 0;
                mMoleFrame++;
            }
            if (mMoleFrame >= 8)
            {
                mMoleFrame = 0;
            }

            if (MolePosX < 0)
            {
                MolePosX += 2;
            }

            if (MolePosX > 750)
            {
                MolePosX -= 2;
            }

            if (MolePosY < 0)
            {
                MolePosY += 2;
            }

            if (MolePosY > 400)
            {
                MolePosY -= 2;
            }

            if (Direction)
            {
                flip = SpriteEffects.FlipHorizontally;
            }
            else
            {
                flip = SpriteEffects.None;
            }
            if (mEdibleWalls.Count > 0)
            {
                collisions(screenCounter);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mSprites, new Vector2(MolePosX, MolePosY),
                       new Rectangle(mMoleFrame * mSpriteWidth,
                       6 * mSpriteHeight,
                       mSpriteWidth, mSpriteHeight), Color.White, 0, Vector2.Zero, 1.0f, flip, 0);
        }

        private void collisions(int screenCounter)
        {
            foreach (Rectangle ed in mEdibleWalls)
            {
                if (MoleCollisionRect.Intersects(ed))
                {
                    int span = mBelowScreenCounter * 10;
                    for (int i = span; i < span + 13; i++)
                    {
                        if (MolePosX < 100)
                        {
                            if (mtoTheUndergound[i, 1] == 15 || mtoTheUndergound[i, 1] == 17)
                            {
                                // replace the edible walls with replacement
                                mtoTheUndergound[i, 1] = 4;
                                if (screenCounter > 0)
                                {
                                    mtoTheUndergound[(i - 10), 12] = 4;
                                }
                                else
                                {
                                    mtoTheUndergound[(i + 150), 12] = 4;
                                }
                            }
                        }
                        else
                        {
                            if (MolePosX > 650)
                            {
                                if (mtoTheUndergound[i, 12] == 16 || mtoTheUndergound[i, 12] == 18)
                                {
                                    mtoTheUndergound[i, 12] = 4;
                                }
                            }
                        }
                    }
                }
            }
        }
        public Rectangle MoleCollisionRect
        {
            get
            {
                return new Rectangle((int)MolePosX, (int)MolePosY, mSpriteWidth, mSpriteHeight);
            }
        }

        public bool Direction { get; set; } = false;

        public float MolePosX { get; set; } = 200;

        public float MolePosY { get; set; } = 100;
    }
}
