using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod
{
    public class Charlie
    {
        private int spriteWidth = 64;
        private int spriteHeight = 69;
        private int summerSaultFrame;
        private int mBelowScreenCounter;
        private int[,] mtoTheUndergound;
        private double animTimer = 0;
        private const double elapsedSecs = 0.1f;
        private bool mTrip;
        private Texture2D mSprites;
        private Texture2D mCollisionRectangle;
        private List<Rectangle> mWalls;
        private List<Rectangle> mFloor;
        private List<Rectangle> mPlatforms;
        private List<Rectangle> mEdibleWalls;
        private List<Rectangle> mAlchiems;
        private List<Rectangle> mRoof;
        private SpriteEffects flip = SpriteEffects.None;
        private Rectangle summerSaultRect;

        public Charlie(int x, int y, int lives, Texture2D gamesprites, Texture2D collisionrect,
            List<Rectangle> walls, List<Rectangle> rects, List<Rectangle> plats,
            List<Rectangle> edible, List<Rectangle> alchiem, List<Rectangle> roof,
            int[,] toTheUndergound)
        {
            summerSaultFrame = 0;
            XPosition = x;
            YPosition = y;
            Lives = lives;
            mSprites = gamesprites;
            mCollisionRectangle = collisionrect;
            mWalls = walls;
            mFloor = rects;
            mPlatforms = plats;
            mEdibleWalls = edible;
            mAlchiems = alchiem;
            mRoof = roof;
            mtoTheUndergound = toTheUndergound;
            FacingLeft = false;
            Jump = false;
            summerSaultJump = false;
            WalkingOnFloor = false;
        }

        public void Update(int tempY, bool jumpright, ref bool belowMoon, ref bool trip, ref int belowScreenCounter, ref int screenCounter, ref int alchiem)
        {
            mTrip = trip;
            mBelowScreenCounter = belowScreenCounter;
            if (summerSaultJump)
            {
                if (jumpright)
                {
                    XPosition += 2;
                }
                else
                {
                    XPosition -= 2;
                }
                if (summerSaultFrame < 8 && mTrip)
                {
                    YPosition -= 10;
                }
                else if (summerSaultFrame >= 8 && summerSaultFrame < 16 && mTrip)
                {
                    YPosition += 10;
                }


                animTimer += elapsedSecs;
                if (animTimer > 0.2)
                {
                    animTimer = 0;
                    summerSaultFrame++;
                }

                summerSaultRect = new Rectangle(summerSaultFrame * spriteWidth, 1 * spriteHeight, spriteWidth, spriteHeight);

                if (summerSaultFrame >= 16)
                {
                    summerSaultFrame = 0;
                    summerSaultJump = false;
                }
            }

            collisions(ref belowMoon, ref belowScreenCounter, ref screenCounter, ref alchiem, ref trip);

            if (FacingLeft)
            {
                flip = SpriteEffects.FlipHorizontally;
            }
            else
            {
                flip = SpriteEffects.None;
            }

            if (Jump)
            {
                YPosition -= 5;
                if ((tempY - YPosition) >= 70)
                {
                    Jump = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle gameSpritesRect)
        {
            //Debugging Image
            //spriteBatch.Draw(mCollisionRectangle, new Vector2(mXPosition + 20, mYPosition), Color.White);

            if (!summerSaultJump)
            {
                spriteBatch.Draw(mSprites, new Vector2(XPosition, YPosition), gameSpritesRect, Color.White, 0, Vector2.Zero, 1.0f, flip, 0);
            }
            else
            {
                spriteBatch.Draw(mSprites, new Vector2(XPosition, YPosition), summerSaultRect, Color.White, 0, Vector2.Zero, 1.0f, flip, 0);
            }
        }

        private void collisions(ref bool belowMoon, ref int belowScreenCounter, ref int screenCounter, ref int alchiem, ref bool trip)
        {
            if (!belowMoon && (charlieRect.Intersects(ColTileRect0) || charlieRect.Intersects(ColTileRect1)))
            {
                belowScreenCounter = screenCounter;
                belowMoon = true;
                Falling = true;
                YPosition = 10;
            }

            if (belowMoon)
            {
                if (Falling && !summerSaultJump)
                {
                    foreach (Rectangle platforms in mPlatforms)
                    {
                        if (charlieRect.Intersects(platforms) && (charlieRect.Bottom == platforms.Top ||
                                charlieRect.Bottom == platforms.Top + 1 ||
                                charlieRect.Bottom == platforms.Top + 2 ||
                                charlieRect.Bottom == platforms.Top + 3))
                        {
                            Falling = false;
                            break;
                        }
                    }

                    foreach (Rectangle floor in mFloor)
                    {
                        if (charlieRect.Intersects(floor))
                        {
                            Falling = false;
                            WalkingOnFloor = true;
                            YPosition = 366;
                            break;
                        }
                    }
                }

                // Check to see if Charlie hits a ledge when summersault jumping
                if (summerSaultJump && mTrip && mPlatforms.Count > 0)
                {
                    foreach (Rectangle platforms in mPlatforms)
                    {
                        if (charlieRect.Intersects(platforms))
                        {
                            int charBottom = platforms.Top - charlieRect.Height;
                            if (charlieRect.Top <= platforms.Top)
                            {
                                YPosition = charBottom - 5;
                            }

                            if (charlieRect.Top >= platforms.Bottom - 15 || charlieRect.Top >= platforms.Bottom - 5)
                            {
                                YPosition += 1;
                                Jump = false;
                                Falling = true;
                                mTrip = false;
                                trip = false;
                            }
                        }
                    }
                }

                // Check to see if Charlie is jumping and hits a ledge from underneath
                if (Jump && mPlatforms.Count > 0)
                {
                    foreach (Rectangle platforms in mPlatforms)
                    {
                        if (charlieRect.Intersects(platforms) && (charlieRect.Top >= platforms.Bottom - 16 && charlieRect.Top <= platforms.Bottom - 10))
                        {
                            YPosition += 2;
                            Jump = false;
                            break;
                        }
                    }
                }

                // Check to see if Charlie has walked over a gap in the floor
                if (!Falling && !WalkingOnFloor && !summerSaultJump)
                {
                    int platformCount = mPlatforms.Count;
                    int checkCounter = 0;
                    foreach (Rectangle platforms in mPlatforms)
                    {
                        if (!charlieRect.Intersects(platforms))
                        {
                            checkCounter++;
                        }
                    }
                    if (checkCounter == platformCount)
                    {
                        Falling = true;
                    }
                }

                if (WalkingOnFloor)
                {
                    int floorCount = mFloor.Count;
                    int checkCounter = 0;
                    foreach (Rectangle floor in mFloor)
                    {
                        if (!charlieRect.Intersects(floor))
                        {
                            checkCounter++;
                        }
                    }
                    if (checkCounter == floorCount)
                    {
                        Falling = true;
                    }
                }

                // Check to see if Charlie walks into the walls
                if (mWalls.Count > 0)
                {
                    foreach (Rectangle wallRects in mWalls)
                    {
                        if (charlieRect.Intersects(wallRects))
                        {
                            if (FacingLeft)
                            {
                                XPosition = wallRects.Right - 19;
                                break;
                            }
                            else
                            {
                                XPosition = 680;
                                break;
                            }
                        }
                    }
                }

                if (mEdibleWalls.Count > 0)
                {
                    foreach (Rectangle edible in mEdibleWalls)
                    {
                        if (edible.IsEmpty && XPosition < 50)
                        {
                            if (screenCounter > 0)
                            {
                                screenCounter -= 1;
                                belowScreenCounter -= 1;
                                XPosition = 740;
                            }
                            else
                            {
                                screenCounter = 15;
                                belowScreenCounter = 15;
                                XPosition = 740;
                            }
                        }
                        else if (charlieRect.Intersects(edible))
                        {
                            if (FacingLeft)
                            {
                                XPosition = edible.Right - 19;
                                break;
                            }
                            else
                            {
                                XPosition = 680;
                                break;
                            }
                        }
                    }
                }

                if (mRoof.Count > 0)
                {
                    foreach (Rectangle roof in mRoof)
                    {
                        if (charlieRect.Intersects(roof))
                        {
                            YPosition += 5;
                            Jump = false;
                        }
                    }
                }

                if (mAlchiems.Count > 0)
                {
                    foreach (Rectangle alchiems in mAlchiems)
                    {
                        if (charlieRect.Intersects(alchiems))
                        {
                            int span = mBelowScreenCounter * 10;
                            for (int i = span; i < span + 13; i++)
                            {
                                for (int j = 1; j < 10; j++)
                                {

                                    if (mtoTheUndergound[i, j] == 22)
                                    {
                                        // replace the alchiems with space
                                        mtoTheUndergound[i, j] = 4;
                                        alchiem += 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public Rectangle charlieRect
        {
            get
            {
                return new Rectangle(XPosition + 20, YPosition, 30, 69);
            }
        }

        public Rectangle ColTileRect0 { get; set; }
        public Rectangle ColTileRect1 { get; set; }
        public bool summerSaultJump { get; set; }
        public bool Jump { get; set; }
        public bool Falling { get; set; }
        public bool FacingLeft { get; set; }
        public bool WalkingOnFloor { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int Lives { get; set; }
    }
}
