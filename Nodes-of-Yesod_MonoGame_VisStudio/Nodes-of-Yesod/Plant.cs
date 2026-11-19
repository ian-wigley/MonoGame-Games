using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

public class Plant : Enemy
{
    public Plant() : base(0, 0, 0, null, null)
    {
    }

    public Plant(float xPos, float yPos, float speedX, Texture2D sprite, List<Rectangle> walls)
        : base(xPos, yPos, speedX, 1.0f, sprite, walls)
    {
        mSprite = sprite;
        CurrentFrameX = 2;
        CurrentFrameY = 9;
        SheetSize = 3;
        timeSinceLastFrame = 0;
    }

    public Rectangle PlantRect => new(CurrentFrameX * MSpriteWidth, 9 * MSpriteHeight, MSpriteWidth, MSpriteHeight);

    public override void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        if (timeSinceLastFrame > 0.6)
        {
            timeSinceLastFrame = 0;
            CurrentFrameX++;
        }
        if (CurrentFrameX >= 6) CurrentFrameX = 0;
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(mSprite, new Vector2(CurrentFrameX, CurrentFrameY), PlantRect, Color.White);
    }
}