using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

public class Mole(Texture2D sprite, List<Rectangle> walls, List<Rectangle> edibleWalls, int[,] toTheUnderground)
{
    private const double ElapsedSecs = 0.1f;
    private double animTimer;
    private SpriteEffects flip = SpriteEffects.None;
    private int mBelowScreenCounter;
    private int mMoleFrame;
    private const int SpriteHeight = 69;
    private const int SpriteWidth = 64;

    private List<Rectangle> mWalls = walls;

    private Rectangle MoleCollisionRect => new((int)MolePosX, (int)MolePosY, SpriteWidth, SpriteHeight);
    public bool Direction { get; set; } = false;
    public float MolePosX { get; set; } = 200;
    public float MolePosY { get; set; } = 100;


    public void Update(GameTime gameTime, int belowScreenCounter, int screenCounter)
    {
        mBelowScreenCounter = belowScreenCounter;
        animTimer += ElapsedSecs;
        if (animTimer > 1.2)
        {
            animTimer = 0;
            mMoleFrame++;
        }

        if (mMoleFrame >= 8) mMoleFrame = 0;

        if (MolePosX < 0) MolePosX += 2;

        if (MolePosX > 750) MolePosX -= 2;

        if (MolePosY < 0) MolePosY += 2;

        if (MolePosY > 400) MolePosY -= 2;

        flip = Direction ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        if (edibleWalls.Count > 0) Collisions(screenCounter);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, new Vector2(MolePosX, MolePosY),
            new Rectangle(mMoleFrame * SpriteWidth,
                6 * SpriteHeight,
                SpriteWidth, SpriteHeight), Color.White, 0, Vector2.Zero, 1.0f, flip, 0);
    }

    private void Collisions(int screenCounter)
    {
        foreach (var span in from ed in edibleWalls where MoleCollisionRect.Intersects(ed) select mBelowScreenCounter * 10)
            for (var i = span; i < span + 13; i++)
                switch (MolePosX)
                {
                    case < 100:
                    {
                        if (toTheUnderground[i, 1] == 15 || toTheUnderground[i, 1] == 17)
                        {
                            // replace the edible walls with replacement
                            toTheUnderground[i, 1] = 4;
                            if (screenCounter > 0)
                                toTheUnderground[i - 10, 12] = 4;
                            else
                                toTheUnderground[i + 150, 12] = 4;
                        }

                        break;
                    }
                    case > 650:
                    {
                        if (toTheUnderground[i, 12] == 16 || toTheUnderground[i, 12] == 18)
                            toTheUnderground[i, 12] = 4;
                        break;
                    }
                }
    }
}