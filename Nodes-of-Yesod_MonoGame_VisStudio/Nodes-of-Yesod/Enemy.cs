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
    protected const int SpriteWidth = 64;
    protected const int SpriteHeight = 69;
    protected bool facingLeft = false;
    protected SpriteEffects flip = SpriteEffects.None;
    protected Point frameSize;
    protected List<Rectangle> platforms;
    protected float speed;
    protected Texture2D sprite;
    protected readonly List<Rectangle> walls;
    protected Rectangle rect;
    protected int millisecondsPerFrame;
    protected int timeSinceLastFrame;
    protected int mFrame;

    protected Enemy(float posX, float posY, float speedX, float speedY, Texture2D sprite,
        List<Rectangle> walls)
    {
        PositionX = posX;
        PositionY = posY;
        SpeedX = speedX;
        SpeedY = speedY;
        this.sprite = sprite;
        this.walls = walls;
        frameSize = new Point(SpriteWidth, SpriteHeight);
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
        this.sprite = sprite;
        this.walls = walls;
        frameSize = new Point(64, 69);
        CurrentFrameX = 4;
        CurrentFrameY = 4;
        SheetSize = 4;
    }

    protected float PositionX { get; set; }
    protected float PositionY { get; set; }
    protected float SpeedX { get; set; }
    protected float SpeedY { get; set; }
    protected int CurrentFrameX { get; set; }
    protected int CurrentFrameY { get; set; }
    protected int SheetSize { get; init; }
    public abstract void Update(GameTime gameTime);

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, new Vector2(PositionX, PositionY),
            new Rectangle(CurrentFrameX * frameSize.X,
                CurrentFrameY * frameSize.Y,
                frameSize.X, frameSize.Y), Color.White, 0,
            Vector2.Zero, 1.0f, flip, 0);
    }
}