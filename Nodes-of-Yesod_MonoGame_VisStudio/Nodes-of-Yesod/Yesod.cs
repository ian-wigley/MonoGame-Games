//////////////////////////////////////////////////////////
//                                                      //
//       Nodes of Yesod - Written by Ian Wigley         //
//           Monogame re-write Nov 2020                 //
//    Static values removed & general code clean up     //
//                                                      //
//////////////////////////////////////////////////////////

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Nodes_of_Yesod
{
    public class Yesod : Game
    {
        private readonly List<Enemy> enemies = new List<Enemy>();
        // Floor textures
        private readonly List<Rectangle> rects = new List<Rectangle>();
        // Platforms & Ledges
        private readonly List<Rectangle> platform = new List<Rectangle>();
        // Standard Walls
        private readonly List<Rectangle> walls = new List<Rectangle>();
        // Mole edible
        private readonly List<Rectangle> edibleWalls = new List<Rectangle>();
        // Alcheims
        private readonly List<Rectangle> alchiems = new List<Rectangle>();
        // Roof rocks
        private readonly List<Rectangle> roof = new List<Rectangle>();

        private readonly List<Rectangle> testList = new List<Rectangle>();

        private GraphicsManager m_graphicsManager;
        private Charlie man;
        private Rocket rocket;
        private Earth earth;
        private Mole mole;

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private KeyboardState oldState;

        private Texture2D gameSprites;
        private Texture2D panel;
        private Texture2D moonRocks;
        private Texture2D undergroundTiles;
        private Texture2D frontScreen;
        private Texture2D collisionTile;
        private SpriteFont font;

        private Rectangle gameSpritesRect;
        private Rectangle heartBeatRect;

        private Rectangle test;

        private double animTimer = 0;
        private double heartBeatTimer = 0;
        private const double elapsedSecs = 0.1f;

        private int tempY;

        private const int spriteWidth = 64;
        private const int spriteHeight = 69;
        private const int rockWidth = 100;
        private const int rockHeight = 117;
        private const int lowerRockWidth = 100;
        private const int lowerRockHeight = 100;

        private int currentFrame = 0;
        private int heartBeatFrame = 8;
        private int seconds = 0;
        private int minutes = 0;

        private const float hole0X = 300.0f;
        private const float hole1X = 500.0f;
        private const float holesY = 400.0f;

        private bool moleManAlive = false;
        private bool gameOn = false;
        private bool jumpRight = false;
        private bool trip = false;
        private bool belowMoon = false;

        private int alchiem = 0;
        private int screenCounter = 0;
        private int belowScreenCounter = 0;

        public Yesod()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            base.Initialize();
            oldState = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameSprites = Content.Load<Texture2D>("sprites");
            m_graphicsManager = new GraphicsManager(gameSprites, enemies, walls, platform);

            //Debugging Image
            Texture2D colRect = Content.Load<Texture2D>("charlieTile");
            panel = Content.Load<Texture2D>("panel");
            moonRocks = Content.Load<Texture2D>("surfaceTiles");
            undergroundTiles = Content.Load<Texture2D>("undergroundTiles");
            collisionTile = Content.Load<Texture2D>("collisionTile");
            frontScreen = Content.Load<Texture2D>("nodesStartScreen");
            font = Content.Load<SpriteFont>("font");

            man = new Charlie(150, 350, 3, gameSprites, colRect, walls, rects, platform, edibleWalls, alchiems, roof, m_graphicsManager.LevelTiles);
            rocket = new Rocket(gameSprites);
            earth = new Earth(gameSprites);
            mole = new Mole(gameSprites, walls, edibleWalls, m_graphicsManager.LevelTiles);

            m_graphicsManager.LoadLevels();
        }

        protected override void Update(GameTime gameTime)
        {
            updateInput();

            if (man.XPosition < 50)
            {
                if (screenCounter > 0)
                {
                    screenCounter -= 1;
                    belowScreenCounter -= 1;
                    if (edibleWalls.Count > 0)
                    {
                        man.XPosition = 740;
                    }
                    else
                    {
                        man.XPosition = 680;
                    }
                    clearAll();
                    if (belowMoon)
                    {
                        m_graphicsManager.ConfigureEnemies(belowScreenCounter);
                    }
                }
                else
                {
                    screenCounter = 15;
                    belowScreenCounter += 15;
                    man.XPosition = 740;
                    clearAll();
                    if (belowMoon)
                    {
                        m_graphicsManager.ConfigureEnemies(belowScreenCounter);
                    }
                }
            }

            if (man.XPosition > 750)
            {
                if (screenCounter < 15)
                {
                    screenCounter += 1;
                    belowScreenCounter += 1;
                    man.XPosition = 55;
                    clearAll();
                    m_graphicsManager.ConfigureEnemies(belowScreenCounter);
                }
                else
                {
                    screenCounter = 0;
                    belowScreenCounter -= 15;
                    man.XPosition = 55;
                    clearAll();
                    m_graphicsManager.ConfigureEnemies(belowScreenCounter);
                }
            }

            gameSpritesRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            heartBeatRect = new Rectangle(heartBeatFrame * spriteWidth, 9 * spriteHeight, spriteWidth, spriteHeight);

            test = new Rectangle(0, 0, 40, 30);

            heartBeatTimer += elapsedSecs;
            if (heartBeatTimer > 0.4)
            {
                heartBeatTimer = 0;
                heartBeatFrame++;
            }
            if (heartBeatFrame >= 6)
            {
                heartBeatFrame = 0;
            }

            if (belowMoon && man.Falling)
            {
                if (man.YPosition < 12)
                {
                    m_graphicsManager.ConfigureEnemies(belowScreenCounter);
                }
                man.YPosition += 2;
                if (man.YPosition >= 425)
                {
                    man.YPosition = 20;
                    man.WalkingOnFloor = false;
                    belowScreenCounter += 16;
                    clearAll();
                    m_graphicsManager.ConfigureEnemies(belowScreenCounter);
                }

                if (man.YPosition <= 15 && belowScreenCounter > 15)
                {
                    man.YPosition = 400;
                    belowScreenCounter -= 16;
                    man.Jump = false;
                    clearAll();
                }
            }

            if (belowMoon && !man.Falling)
            {
                // Allow us to jump out from under the moon surface....
                if (man.YPosition <= 30 && belowScreenCounter < 15)
                {
                    man.Falling = false;
                    man.WalkingOnFloor = false;
                    belowMoon = false;
                    man.YPosition = 250;
                    clearAll();
                    belowScreenCounter = 0;
                    m_graphicsManager.ConfigureEnemies(belowScreenCounter);
                }
                if (man.YPosition <= 15 && belowScreenCounter > 15)
                {
                    man.YPosition = 400;
                    belowScreenCounter -= 16;
                    man.Jump = false;
                    clearAll();
                }
            }

            if (belowMoon)
            {
                foreach (Enemy en in enemies)
                {
                    var chaser = en as ChasingEnemy;
                    if (chaser != null)
                    {
                        ChasingEnemy chasingEnemy = (ChasingEnemy)en;
                        chasingEnemy.Update(gameTime, new Vector2(man.XPosition, man.YPosition));
                    }
                    else
                    {
                        en.Update(gameTime);
                    }
                }

                rects.Clear();
                walls.Clear();
                platform.Clear();
                edibleWalls.Clear();
                alchiems.Clear();
                testList.Clear();
                if (roof.Count > 0)
                {
                    roof.Clear();
                }

                m_graphicsManager.LevelList[belowScreenCounter].TileList.ForEach(i => { platform.Add(i.TileRect); });
            }
            else if(!belowMoon)
            {
                earth.Update(gameTime);
            }

            if (moleManAlive)
            {
                mole.Update(gameTime, belowScreenCounter, screenCounter);
            }

            man.Update(tempY, jumpRight, ref belowMoon, ref trip, ref belowScreenCounter, ref screenCounter, ref alchiem);

            rocket.Update(gameTime);

            seconds = gameTime.ElapsedGameTime.Seconds;
            minutes = gameTime.ElapsedGameTime.Minutes;

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            if (!gameOn)
            {
                spriteBatch.Draw(frontScreen, new Vector2(0, 0), Color.White);
                Vector2 textLocation = new Vector2(10, 10);
                spriteBatch.DrawString(font, "XNA NODES OF YESOD REMAKE ", textLocation + new Vector2(295.0f, 020.0f), Color.Yellow);
                spriteBatch.DrawString(font, "Start", textLocation + new Vector2(480.0f, 100.0f), Color.Yellow);
                spriteBatch.DrawString(font, "Instructions", textLocation + new Vector2(480.0f, 120.0f), Color.Yellow);
                spriteBatch.DrawString(font, "Define Keys ", textLocation + new Vector2(480.0f, 140.0f), Color.Yellow);
                spriteBatch.DrawString(font, "Exit", textLocation + new Vector2(480.0f, 160.0f), Color.Yellow);
                spriteBatch.DrawString(font, "Hit X to Start ", textLocation + new Vector2(480.0f, 240.0f), Color.Yellow);
            }

            else
            {
                if (!belowMoon)
                {

                    if (rocket.RocketScreen == screenCounter)
                    {
                        rocket.Draw(spriteBatch);
                    }

                    for (int j = 0; j < 8; j++)
                    {
                        spriteBatch.Draw(moonRocks,
                        new Rectangle((j * rockWidth),
                        170,
                        rockWidth, rockHeight),
                        new Rectangle((m_graphicsManager.UpperRockArray[screenCounter, j] * rockWidth), 0, rockWidth, rockHeight),
                        Color.White);
                    }

                    Rectangle moundRect = new Rectangle((m_graphicsManager.MoundArray[screenCounter, 0] * lowerRockWidth), 300, lowerRockWidth, lowerRockHeight);

                    if (m_graphicsManager.HoleArray0[screenCounter, 0] == 0)
                    {
                        man.ColTileRect0 = new Rectangle(340, 415, 20, 18);
                        Rectangle holeRect0 = new Rectangle((m_graphicsManager.HoleArray0[screenCounter, 0] * lowerRockWidth), 300, lowerRockWidth, lowerRockHeight);
                        spriteBatch.Draw(moonRocks, new Vector2(hole0X, holesY), holeRect0, Color.White);
                        spriteBatch.Draw(collisionTile, new Vector2(hole0X + 20.0f, holesY + 15.0f), man.ColTileRect0, Color.White);
                    }

                    if (m_graphicsManager.HoleArray0[screenCounter, 0] != 0)
                    {
                        man.ColTileRect0 = new Rectangle(340, 0, 20, 18);
                    }

                    if (m_graphicsManager.HoleArray1[screenCounter, 0] == 0)
                    {
                        man.ColTileRect1 = new Rectangle(540, 415, 20, 18);
                        Rectangle holeRect1 = new Rectangle((m_graphicsManager.HoleArray1[screenCounter, 0] * lowerRockWidth), 300, lowerRockWidth, lowerRockHeight);
                        spriteBatch.Draw(moonRocks, new Vector2(hole1X, holesY), holeRect1, Color.White);
                        spriteBatch.Draw(collisionTile, new Vector2(hole1X + 20.0f, holesY + 15.0f), man.ColTileRect1, Color.White);
                    }
                    if (m_graphicsManager.HoleArray1[screenCounter, 0] != 0)
                    {
                        man.ColTileRect1 = new Rectangle(340, 0, 20, 18);
                    }

                    spriteBatch.Draw(moonRocks, new Vector2(100.0f, 300.0f), moundRect, Color.White);
                    spriteBatch.Draw(moonRocks, new Vector2(600.0f, 300.0f), moundRect, Color.White);

                    for (int l = 0; l < 8; l++)
                    {
                        spriteBatch.Draw(moonRocks,
                                    new Rectangle((l * lowerRockWidth),
                                    470,
                                    lowerRockWidth, lowerRockHeight),
                                    new Rectangle((m_graphicsManager.LowerRockArray[screenCounter, l] * lowerRockWidth), 170, lowerRockWidth, lowerRockHeight),
                                    Color.White);
                    }
                    earth.Draw(spriteBatch);
                }

                else if (belowMoon)
                {
                    //rects.Clear();
                    //walls.Clear();
                    //platform.Clear();
                    //edibleWalls.Clear();
                    //alchiems.Clear();
                    //testList.Clear();
                    //if (roof.Count > 0)
                    //{
                    //    roof.Clear();
                    //}


                    ////m_graphicsManager.TileList.ForEach(i => { if (i.Drawable(x, y)) { spriteBatch.Draw(undergroundTiles, i.TileRect, i.TileTexture, Color.White); } });
                    m_graphicsManager.LevelList[belowScreenCounter].TileList.ForEach(i => { spriteBatch.Draw(undergroundTiles, i.TileRect, i.TileTexture, Color.White); });


 //                   m_graphicsManager.LevelList[belowScreenCounter].TileList.ForEach(i => { platform.Add(i.TileRect); });

                    //int textureY = 0;
                    //int textureX = 0;

                    //for (int ii = 0; ii < 10; ii++)
                    //{
                    //    for (int jj = 0; jj < 13; jj++)
                    //    {
                    //        // Iterate through the array & point to the start position of the texture to be grabbed by the wallRect
                    //        int platforms = (m_graphicsManager.LevelTiles[(belowScreenCounter * 10) + ii, jj]);

                    //        if (platforms < 19)
                    //        {
                    //            textureY = 0;
                    //            textureX = (0 * unGroTileWidth);
                    //        }

                    //        // If the image is greater than 19 then point to the next line in the texture  
                    //        if (platforms >= 19 && platforms < 38)
                    //        {
                    //            textureY = (unGroTileHeight * 1);
                    //            textureX = (19 * unGroTileWidth);
                    //        }

                    //        // If the image is greater than 38 then point to the next line in the texture
                    //        if (platforms >= 38 && platforms < 57)
                    //        {
                    //            textureY = (unGroTileHeight * 2);
                    //            textureX = (38 * unGroTileWidth);
                    //        }

                    //        // If the image is greater than 57 then point to the next line in the texture
                    //        if (platforms >= 57 && platforms < 76)
                    //        {
                    //            textureY = (unGroTileHeight * 3);
                    //            textureX = (57 * unGroTileWidth);
                    //        }

                    //        //Grab the rectangle texture from the underground graphics.png  
                    //        Rectangle wallRect = new Rectangle((platforms * unGroTileWidth) - textureX, textureY, unGroTileWidth, unGroTileHeight);

                    //        spriteBatch.Draw(undergroundTiles, new Rectangle((jj * unGroTileWidth),
                    //        ii * 48, 62, 48), wallRect, Color.White);

                    //        Rectangle collisionRects = new Rectangle((jj * unGroTileWidth), ii * 48, 62, 48);

                    //        // Store the tiles to collide with in a List of Rectangles
                    //        // Check if floor tiles exist & only 1 screen high (i!) 

                    //        // Alchiems
                    //        if (platforms == 19 || platforms == 20 || platforms == 21 || platforms == 22)
                    //        {
                    //            alchiems.Add(collisionRects);
                    //        }
                    //        // Mole edible wall textures
                    //        if (platforms == 15 || platforms == 16 || platforms == 17 || platforms == 18)
                    //        {
                    //            edibleWalls.Add(collisionRects);
                    //        }
                    //        // Floor textures
                    //        if (platforms == 5 || platforms == 6 || platforms == 7 || platforms == 8
                    //            || platforms == 24 || platforms == 25 || platforms == 26
                    //            || platforms == 43 || platforms == 44)
                    //        {
                    //            rects.Add(collisionRects);
                    //        }
                    //        // Platforms & Ledges
                    //        if (platforms == 9 || platforms == 10 || platforms == 11 || platforms == 13
                    //            || platforms == 14 || platforms == 28 || platforms == 29
                    //            || platforms == 47 || platforms == 48)
                    //        {
                    //            platform.Add(collisionRects);

                    //            //Copy the x,y co-ord from the platform into the test
                    //            test.Location = collisionRects.Location;
                    //            testList.Add(test);
                    //        }
                    //        // Wall textures                               
                    //        if (platforms == 0 || platforms == 1 || platforms == 2 || platforms == 3)
                    //        {
                    //            walls.Add(collisionRects);
                    //        }
                    //        // Roof textures
                    //        if (platforms >= 57 && platforms < 76)
                    //        {
                    //            roof.Add(collisionRects);
                    //        }
                    //    }
                    //}

                    foreach (Enemy en in enemies)
                    {
                        en.Draw(spriteBatch);
                    }
                }

                if (moleManAlive)
                {
                    mole.Draw(spriteBatch);
                }

                man.Draw(spriteBatch, gameSpritesRect);

                spriteBatch.DrawString(font, "Underground screen counter : " + belowScreenCounter, new Vector2(0.0f, 30.0f), Color.White);

                spriteBatch.Draw(panel, new Vector2(30, 550), Color.White);
                spriteBatch.Draw(gameSprites, new Vector2(390.0f, 535.0f), heartBeatRect, Color.White);
                spriteBatch.DrawString(font, "" + alchiem, new Vector2(250.0f, 555.0f), Color.White);
                spriteBatch.DrawString(font, "" + man.Lives, new Vector2(395.0f, 555.0f), Color.White);

                if (seconds < 10 && minutes == 0)
                {
                    spriteBatch.DrawString(font, "00:0" + seconds, new Vector2(635.0f, 555.0f), Color.White);
                }
                else if (seconds >= 10 && minutes == 0)
                {
                    spriteBatch.DrawString(font, "00:" + seconds, new Vector2(635.0f, 555.0f), Color.White);
                }
                if (seconds < 10 && minutes > 0)
                {
                    spriteBatch.DrawString(font, "0" + minutes + ":0" + seconds, new Vector2(635.0f, 555.0f), Color.White);
                }
                else if (seconds >= 10 && minutes > 0)
                {
                    spriteBatch.DrawString(font, "0" + minutes + ":" + seconds, new Vector2(635.0f, 555.0f), Color.White);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void clearAll()
        {
            enemies.Clear();
            rects.Clear();
            walls.Clear();
            platform.Clear();
            edibleWalls.Clear();
            alchiems.Clear();
            testList.Clear();
            if (roof.Count > 0)
            {
                roof.Clear();
            }
        }

        private void updateInput()
        {
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();

            if (!gameOn)
            {
                // Allows the game to exit
                if (gamePad.Buttons.Back == ButtonState.Pressed ||
                    keyboard.IsKeyDown(Keys.Escape))
                    Exit();

                if (gamePad.Buttons.Back == ButtonState.Pressed ||
                    keyboard.IsKeyDown(Keys.X))
                {
                    gameOn = true;
                }
            }
            else
            {
                if (gamePad.Buttons.Back == ButtonState.Pressed ||
                    keyboard.IsKeyDown(Keys.Escape))
                    Exit();

                if (keyboard.IsKeyDown(Keys.M) && (belowMoon))
                {
                    mole.MolePosX = man.XPosition;
                    mole.MolePosY = man.YPosition;
                    moleManAlive = true;
                }

                if (keyboard.IsKeyDown(Keys.N))
                {
                    moleManAlive = false;
                }

                if (gamePad.DPad.Left == ButtonState.Pressed ||
                    keyboard.IsKeyDown(Keys.Left) && (!man.Falling) && (!moleManAlive))
                {
                    man.FacingLeft = true;
                    man.XPosition -= 2;
                    animTimer += elapsedSecs;
                    if (animTimer > 0.6)
                    {
                        animTimer = 0;
                        currentFrame++;
                    }
                    if (currentFrame >= 6)
                    {
                        currentFrame = 0;
                    }
                }

                if (gamePad.DPad.Left == ButtonState.Pressed ||
                    keyboard.IsKeyDown(Keys.Left) && (moleManAlive))
                {
                    mole.Direction = true;
                    mole.MolePosX -= 2;
                }


                if (gamePad.DPad.Right == ButtonState.Pressed ||
                    keyboard.IsKeyDown(Keys.Right) && (!man.Falling) && (!moleManAlive))
                {
                    man.XPosition += 2;
                    man.FacingLeft = false;

                    animTimer += elapsedSecs;
                    if (animTimer > 0.6)
                    {
                        animTimer = 0;
                        currentFrame++;
                    }
                    if (currentFrame >= 6)
                    {
                        currentFrame = 0;
                    }
                }

                if (gamePad.DPad.Left == ButtonState.Pressed ||
                    keyboard.IsKeyDown(Keys.Right) && (moleManAlive))
                {
                    mole.Direction = false;
                    mole.MolePosX += 2;
                }

                if (gamePad.DPad.Left == ButtonState.Pressed ||
                    keyboard.IsKeyDown(Keys.Down) && (moleManAlive))
                {
                    mole.MolePosY += 2;
                }

                if (gamePad.DPad.Left == ButtonState.Pressed ||
                    keyboard.IsKeyDown(Keys.Up) && (moleManAlive))
                {
                    mole.MolePosY -= 2;
                }

                if (keyboard.IsKeyDown(Keys.Right) && keyboard.IsKeyDown(Keys.LeftControl)
                    && oldState.IsKeyUp(Keys.LeftControl) && (!man.Falling))
                {
                    man.WalkingOnFloor = false;
                    man.summerSaultJump = true;
                    jumpRight = true;
                    trip = true;

                }

                if (keyboard.IsKeyDown(Keys.Left) && keyboard.IsKeyDown(Keys.LeftControl)
                    && oldState.IsKeyUp(Keys.LeftControl) && (!man.Falling))
                {
                    man.WalkingOnFloor = false;
                    man.summerSaultJump = true;
                    jumpRight = false;
                    trip = true;
                }

                if (keyboard.IsKeyDown(Keys.F1))
                {
                    man.Falling = false;
                    man.WalkingOnFloor = false;
                    belowMoon = false;
                    man.YPosition = 350;
                    belowScreenCounter = screenCounter;
                    clearAll();
                }

                if (keyboard.IsKeyDown(Keys.F2))
                {
                    man.Falling = false;
                }

                if (gamePad.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.LeftControl)
                    && (belowMoon) && (!man.Falling) && (!man.summerSaultJump))
                {
                    tempY = man.YPosition;
                    man.Jump = true;
                }

                oldState = keyboard;
            }
        }
    }
}
