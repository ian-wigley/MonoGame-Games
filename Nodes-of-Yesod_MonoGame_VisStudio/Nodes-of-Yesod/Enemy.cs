using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "name")]
[JsonDerivedType(typeof(Alf), "Alf")]
[JsonDerivedType(typeof(Bird), "Bird")]
[JsonDerivedType(typeof(BlueThingy), "BlueThingy")]
[JsonDerivedType(typeof(Caterpillar), "Caterpillar")]
[JsonDerivedType(typeof(ChasingEnemy), "ChasingEnemy")]
[JsonDerivedType(typeof(CockRoach), "CockRoach")]
[JsonDerivedType(typeof(Fire), "Fire")]
[JsonDerivedType(typeof(Fish), "Fish")]
[JsonDerivedType(typeof(GreenMeanie), "GreenMeanie")]
[JsonDerivedType(typeof(Plant), "Plant")]
[JsonDerivedType(typeof(RedSpaceman), "RedSpaceman")]
[JsonDerivedType(typeof(SpringBear), "SpringBear")]
[JsonDerivedType(typeof(TelePort), "TelePort")]
[JsonDerivedType(typeof(WhirlWind), "WhirlWind")]
[JsonDerivedType(typeof(WoodLouse), "WoodLouse")]
public abstract class Enemy
{
    protected const int MSpriteWidth = 64;
    protected const int MSpriteHeight = 69;
    protected bool facingLeft = false;
    protected SpriteEffects flip = SpriteEffects.None;
    protected int mFrame;
    protected Point mFrameSize;
    protected List<Rectangle> mPlatforms;
    protected float mSpeed;
    protected Texture2D mSprite;
    protected readonly List<Rectangle> mWalls;
    protected int millisecondsPerFrame;
    protected int timeSinceLastFrame;

    protected Enemy(float posX, float posY, float speedX, float speedY, Texture2D sprite,
        List<Rectangle> walls)
    {
        PositionX = posX;
        PositionY = posY;
        SpeedX = speedX;
        SpeedY = speedY;
        mSprite = sprite;
        mWalls = walls;
        mFrameSize = new Point(MSpriteWidth, MSpriteHeight);
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

    public float PositionX { get; set; }

    public float PositionY { get; set; }

    public float SpeedX { get; set; }

    public float SpeedY { get; set; }

    public int CurrentFrameX { get; set; }

    public int CurrentFrameY { get; set; }

    public int SheetSize { get; set; }

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
}