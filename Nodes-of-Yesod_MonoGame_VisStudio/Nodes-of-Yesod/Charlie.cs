using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

public class Charlie(
    int x,
    int y,
    int lives,
    Texture2D gameSprites,
    Texture2D collisionRect,
    List<Rectangle> walls,
    List<Rectangle> rects,
    List<Rectangle> plats,
    List<Rectangle> mEdibleWalls,
    List<Rectangle> mAlchiems,
    List<Rectangle> mRoof,
    int[,] toTheUnderground)
{
    private const double ElapsedSecs = 0.1f;
    private double animTimer;
    private SpriteEffects flip = SpriteEffects.None;
    private int belowScreenCounter;
    private bool mTrip;
    private const int SpriteHeight = 69;
    private const int SpriteWidth = 64;
    private int summerSaultFrame;
    private Rectangle summerSaultRect;
    private Texture2D mCollisionRectangle = collisionRect;
    
    private Rectangle CharlieRect => new(XPosition + 20, YPosition, 30, 69);

    public Rectangle ColTileRect0 { get; set; }
    public Rectangle ColTileRect1 { get; set; }
    public bool SummerSaultJump { get; set; }
    public bool Jump { get; set; }
    public bool Falling { get; set; }
    public bool FacingLeft { get; set; }
    public bool WalkingOnFloor { get; set; }
    public int XPosition { get; set; } = x;
    public int YPosition { get; set; } = y;
    public int Lives { get; } = lives;

    public void Update(int tempY, bool jumpright, ref bool belowMoon, ref bool trip, ref int belowScreenCounter,
        ref int screenCounter, ref int alchiem)
    {
        mTrip = trip;
        this.belowScreenCounter = belowScreenCounter;
        if (SummerSaultJump)
        {
            if (jumpright)
                XPosition += 2;
            else
                XPosition -= 2;
            switch (summerSaultFrame)
            {
                case < 8 when mTrip:
                    YPosition -= 10;
                    break;
                case >= 8 and < 16 when mTrip:
                    YPosition += 10;
                    break;
            }


            animTimer += ElapsedSecs;
            if (animTimer > 0.2)
            {
                animTimer = 0;
                summerSaultFrame++;
            }

            summerSaultRect =
                new Rectangle(summerSaultFrame * SpriteWidth, 1 * SpriteHeight, SpriteWidth, SpriteHeight);

            if (summerSaultFrame >= 16)
            {
                summerSaultFrame = 0;
                SummerSaultJump = false;
            }
        }

        Collisions(ref belowMoon, ref belowScreenCounter, ref screenCounter, ref alchiem, ref trip);

        flip = FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        if (!Jump) return;
        YPosition -= 5;
        if (tempY - YPosition >= 70) Jump = false;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle gameSpritesRect)
    {
        //Debugging Image
        //spriteBatch.Draw(mCollisionRectangle, new Vector2(mXPosition + 20, mYPosition), Color.White);

        if (!SummerSaultJump)
            spriteBatch.Draw(gameSprites, new Vector2(XPosition, YPosition), gameSpritesRect, Color.White, 0, Vector2.Zero,
                1.0f, flip, 0);
        else
            spriteBatch.Draw(gameSprites, new Vector2(XPosition, YPosition), summerSaultRect, Color.White, 0, Vector2.Zero,
                1.0f, flip, 0);
    }

    private void Collisions(ref bool belowMoon, ref int belowScreenCounter, ref int screenCounter, ref int alchiem,
        ref bool trip)
    {
        if (!belowMoon && (CharlieRect.Intersects(ColTileRect0) || CharlieRect.Intersects(ColTileRect1)))
        {
//            belowScreenCounter = screenCounter;
            screenCounter += 16;
            belowMoon = true;
            Falling = true;
            YPosition = 10;
        }

        if (!belowMoon) return;
        if (Falling && !SummerSaultJump)
        {
            foreach (var _ in plats.Where(platforms => CharlieRect.Intersects(platforms) && (CharlieRect.Bottom == platforms.Top ||
                         CharlieRect.Bottom == platforms.Top + 1 ||
                         CharlieRect.Bottom == platforms.Top + 2 ||
                         CharlieRect.Bottom == platforms.Top + 3)))
            {
                Falling = false;
                break;
            }

            if (rects.Any(floor => CharlieRect.Intersects(floor)))
            {
                Falling = false;
                WalkingOnFloor = true;
                YPosition = 366;
            }
        }

        // Check to see if Charlie hits a ledge when summersault jumping
        if (SummerSaultJump && mTrip && plats.Count > 0)
            foreach (var platforms in plats)
                if (CharlieRect.Intersects(platforms))
                {
                    var charBottom = platforms.Top - CharlieRect.Height;
                    if (CharlieRect.Top <= platforms.Top) YPosition = charBottom - 5;

                    if (CharlieRect.Top >= platforms.Bottom - 15 || CharlieRect.Top >= platforms.Bottom - 5)
                    {
                        YPosition += 1;
                        Jump = false;
                        Falling = true;
                        mTrip = false;
                        trip = false;
                    }
                }

        // Check to see if Charlie is jumping and hits a ledge from underneath
        if (Jump && plats.Count > 0)
            foreach (var platforms in plats)
                if (CharlieRect.Intersects(platforms) && CharlieRect.Top >= platforms.Bottom - 16 &&
                    CharlieRect.Top <= platforms.Bottom - 10)
                {
                    YPosition += 2;
                    Jump = false;
                    break;
                }

        // Check to see if Charlie has walked over a gap in the floor
        if (!Falling && !WalkingOnFloor && !SummerSaultJump)
        {
            var platformCount = plats.Count;
            var checkCounter = 0;
            foreach (var platforms in plats)
                if (!CharlieRect.Intersects(platforms))
                    checkCounter++;

            if (checkCounter == platformCount) Falling = true;
        }

        if (WalkingOnFloor)
        {
            var floorCount = rects.Count;
            var checkCounter = 0;
            foreach (var floor in rects)
                if (!CharlieRect.Intersects(floor))
                    checkCounter++;

            if (checkCounter == floorCount) Falling = true;
        }

        // Check to see if Charlie walks into the walls
        if (walls.Count > 0)
            foreach (var wallRects in walls)
                if (CharlieRect.Intersects(wallRects))
                {
                    if (FacingLeft)
                    {
                        XPosition = wallRects.Right - 19;
                        break;
                    }

                    XPosition = 680;
                    break;
                }

        if (mEdibleWalls.Count > 0)
            foreach (var edible in mEdibleWalls)
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
                else if (CharlieRect.Intersects(edible))
                {
                    if (FacingLeft)
                    {
                        XPosition = edible.Right - 19;
                        break;
                    }

                    XPosition = 680;
                    break;
                }

        if (mRoof.Count > 0)
            foreach (var roof in mRoof)
                if (CharlieRect.Intersects(roof))
                {
                    YPosition += 5;
                    Jump = false;
                }

        if (mAlchiems.Count > 0)
            foreach (var alchiems in mAlchiems)
                if (CharlieRect.Intersects(alchiems))
                {
                    var span = this.belowScreenCounter * 10;
                    for (var i = span; i < span + 13; i++)
                    for (var j = 1; j < 10; j++)
                        if (toTheUnderground[i, j] == 22)
                        {
                            // replace the alchiems with space
                            toTheUnderground[i, j] = 4;
                            alchiem += 1;
                        }
                }
    }
}