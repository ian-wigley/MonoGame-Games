//////////////////////////////////////////////////////////
//                                                      //
//       Nodes of Yesod - Written by Ian Wigley         //
//           Monogame re-write Nov 2020                 //
//    Static values removed & general code clean up     //
//                                                      //
//////////////////////////////////////////////////////////

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Nodes_of_Yesod;

public class Yesod : Game
{
    private const double ElapsedSecs = 0.1f;
    private const int SpriteWidth = 64;
    private const int SpriteHeight = 69;
    private const float Hole0X = 300.0f;
    private const float Hole1X = 500.0f;
    private const float HolesY = 400.0f;

    // Alcheims
    private readonly List<Rectangle> alchiems = [];

    // Mole edible
    private readonly List<Rectangle> edibleWalls = [];
    private readonly List<Enemy> enemies = [];

    private readonly GraphicsDeviceManager graphics;

    // Platforms & Ledges
    private readonly List<Rectangle> platform = [];

    // Floor textures
    private readonly List<Rectangle> rects = [];

    // Roof rocks
    private readonly List<Rectangle> roof = [];

    //private readonly List<Rectangle> testList = [];

    // Standard Walls
    private readonly List<Rectangle> walls = [];

    private int alchiem;

    private double animTimer;
    private bool belowMoon;

    private Texture2D collisionTile;

    private int currentFrame;
    private Earth earth;
    private SpriteFont font;
    private Texture2D frontScreen;
    private bool gameOn;

    private Texture2D gameSprites;

    private Rectangle gameSpritesRect;
    private int heartBeatFrame = 8;
    private Rectangle heartBeatRect;
    private double heartBeatTimer;
    private bool jumpRight;

    private GraphicsManager graphicsManager;
    private Charlie man;
    private int minutes;
    private Mole mole;

    private bool moleManAlive;
    private Texture2D tiles;
    private KeyboardState oldState;
    private Texture2D panel;
    private Rocket rocket;
    private int screenCounter;
    private int seconds;
    private SpriteBatch spriteBatch;

    private int tempY;

    //private Rectangle test;
    private bool trip;

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

    protected override async void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        gameSprites = Content.Load<Texture2D>("sprites");
        graphicsManager = new GraphicsManager(gameSprites, enemies, walls, platform);

        //Debugging Image
        var colRect = Content.Load<Texture2D>("charlieTile");
        panel = Content.Load<Texture2D>("panel");
        tiles = Content.Load<Texture2D>("tiles");
        collisionTile = Content.Load<Texture2D>("collisionTile");
        frontScreen = Content.Load<Texture2D>("nodesStartScreen");
        font = Content.Load<SpriteFont>("font");

        man = new Charlie(150, 350, 3, gameSprites, colRect, walls, rects, platform, edibleWalls, 
            alchiems, roof, graphicsManager.LevelTiles);
        rocket = new Rocket(gameSprites);
        earth = new Earth(gameSprites);
        mole = new Mole(gameSprites, walls, edibleWalls, graphicsManager.LevelTiles);

        await graphicsManager.LoadJsonLevel();
    }

    protected override void Update(GameTime gameTime)
    {
        UpdateInput();
        if (man.XPosition < 50)
        {
            if (screenCounter > 0)
            {
                screenCounter -= 1;
                man.XPosition = edibleWalls.Count > 0 ? 740 : 680;
            }
            else
            {
                screenCounter = 15;
                man.XPosition = 740;
            }

            ClearAll();
            if (belowMoon) graphicsManager.ConfigureEnemies(screenCounter);
        }

        if (man.XPosition > 750)
        {
            if (screenCounter < 15)
            {
                screenCounter += 1;
            }
            else
            {
                screenCounter = 0;
            }

            man.XPosition = 55;
            ClearAll();
            graphicsManager.ConfigureEnemies(screenCounter);
        }

        gameSpritesRect = new Rectangle(currentFrame * SpriteWidth, 0, SpriteWidth, SpriteHeight);
        heartBeatRect = new Rectangle(heartBeatFrame * SpriteWidth, 9 * SpriteHeight, SpriteWidth, SpriteHeight);

        //test = new Rectangle(0, 0, 40, 30);

        heartBeatTimer += ElapsedSecs;
        if (heartBeatTimer > 0.4)
        {
            heartBeatTimer = 0;
            heartBeatFrame++;
        }

        if (heartBeatFrame >= 6) heartBeatFrame = 0;

        if (belowMoon && man.Falling)
        {
            if (man.YPosition < 12) graphicsManager.ConfigureEnemies(screenCounter);
            man.YPosition += 2;
            if (man.YPosition >= 425)
            {
                man.YPosition = 20;
                man.WalkingOnFloor = false;
                screenCounter += 16;
                ClearAll();
                graphicsManager.ConfigureEnemies(screenCounter);
            }

            // if (man.YPosition <= 15 && screenCounter > 15) //belowScreenCounter > 15)
            // {
            //     man.YPosition = 400;
            //     //belowScreenCounter -= 16;
            //     screenCounter -= 16;
            //     man.Jump = false;
            //     clearAll();
            // }
        }

        if (belowMoon && !man.Falling)
        {
            // Allow us to jump out from under the moon surface....
            if (man.YPosition <= 30 && screenCounter < 15)
            {
                man.Falling = false;
                man.WalkingOnFloor = false;
                belowMoon = false;
                man.YPosition = 250;
                ClearAll();
                screenCounter = 0;
                graphicsManager.ConfigureEnemies(screenCounter);
            }

            if (man.YPosition <= 15 && screenCounter > 15)
            {
                man.YPosition = 400;
                screenCounter -= 16;
                man.Jump = false;
                ClearAll();
            }
        }

        switch (belowMoon)
        {
            case true:
            {
                foreach (var en in enemies)
                    if (en is ChasingEnemy chaser)
                        chaser.Update(gameTime, new Vector2(man.XPosition, man.YPosition));
                    else
                        en.Update(gameTime);

                rects.Clear();
                walls.Clear();
                platform.Clear();
                edibleWalls.Clear();
                alchiems.Clear();
                //testList.Clear();
                if (roof.Count > 0) roof.Clear();

                graphicsManager.Screens[screenCounter].Tiles
                    .ForEach(i => { platform.Add(i.TileDestination); });
                break;
            }
            case false:
                earth.Update(gameTime);
                break;
        }

        if (moleManAlive) mole.Update(gameTime, screenCounter, screenCounter);

        man.Update(tempY, jumpRight, ref belowMoon, ref trip, ref screenCounter,
            ref screenCounter, ref alchiem);

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
            var textLocation = new Vector2(10, 10);
            spriteBatch.DrawString(font, "XNA NODES OF YESOD REMAKE ", textLocation + new Vector2(295.0f, 020.0f),
                Color.Yellow);
            spriteBatch.DrawString(font, "Start", textLocation + new Vector2(480.0f, 100.0f), Color.Yellow);
            spriteBatch.DrawString(font, "Instructions", textLocation + new Vector2(480.0f, 120.0f), Color.Yellow);
            spriteBatch.DrawString(font, "Define Keys ", textLocation + new Vector2(480.0f, 140.0f), Color.Yellow);
            spriteBatch.DrawString(font, "Exit", textLocation + new Vector2(480.0f, 160.0f), Color.Yellow);
            spriteBatch.DrawString(font, "Hit X to Start ", textLocation + new Vector2(480.0f, 240.0f), Color.Yellow);
        }

        else
        {
            graphicsManager.Screens[screenCounter].Tiles.ForEach(i =>
            {
                spriteBatch.Draw(
                    tiles,
                    i.TileDestination,
                    i.TileSource,
                    Color.White);
            });

            switch (belowMoon)
            {
                case false:
                {
                    if (rocket.RocketScreen == screenCounter) rocket.Draw(spriteBatch);
                    if (graphicsManager.HoleArray0[screenCounter, 0] == 0)
                    {
                        man.ColTileRect0 = new Rectangle(340, 400, 20, 18);
                        spriteBatch.Draw(collisionTile, new Vector2(Hole0X + 20.0f, HolesY + 15.0f), man.ColTileRect0,
                            Color.White);
                    }

                    if (graphicsManager.HoleArray0[screenCounter, 0] != 0)
                        man.ColTileRect0 = new Rectangle(340, 0, 20, 18);

                    if (graphicsManager.HoleArray1[screenCounter, 0] == 0)
                    {
                        man.ColTileRect1 = new Rectangle(540, 415, 20, 18);
                        spriteBatch.Draw(collisionTile, new Vector2(Hole1X + 20.0f, HolesY + 15.0f), man.ColTileRect1,
                            Color.White);
                    }

                    if (graphicsManager.HoleArray1[screenCounter, 0] != 0)
                        man.ColTileRect1 = new Rectangle(340, 0, 20, 18);
                    break;
                }
                case true:
                {
                    //         //rects.Clear();
                    //         //walls.Clear();
                    //         //platform.Clear();
                    //         //edibleWalls.Clear();
                    //         //alchiems.Clear();
                    //         //testList.Clear();
                    //         //if (roof.Count > 0)
                    //         //{
                    //         //    roof.Clear();
                    //         //}
                    //
                    //         m_graphicsManager.screens[screenCounter /*belowScreenCounter*/].Tiles.ForEach(i =>
                    //         {
                    //             spriteBatch.Draw(
                    //                 undergroundTiles, 
                    //                 i.TileDestination, 
                    //                 i.TileSource, 
                    //                 Color.White);
                    //         });
                    //
                    //
                    //         // m_graphicsManager.LevelList[belowScreenCounter].TileList.ForEach(i => { platform.Add(i.TileRect); });
                    //
                    //         //int textureY = 0;
                    //         //int textureX = 0;
                    //
                    //         //for (int ii = 0; ii < 10; ii++)
                    //         //{
                    //         //    for (int jj = 0; jj < 13; jj++)
                    //         //    {
                    //         //        // Iterate through the array & point to the start position of the texture to be grabbed by the wallRect
                    //         //        int platforms = (m_graphicsManager.LevelTiles[(belowScreenCounter * 10) + ii, jj]);
                    //
                    //         //        if (platforms < 19)
                    //         //        {
                    //         //            textureY = 0;
                    //         //            textureX = (0 * unGroTileWidth);
                    //         //        }
                    //
                    //         //        // If the image is greater than 19 then point to the next line in the texture  
                    //         //        if (platforms >= 19 && platforms < 38)
                    //         //        {
                    //         //            textureY = (unGroTileHeight * 1);
                    //         //            textureX = (19 * unGroTileWidth);
                    //         //        }
                    //
                    //         //        // If the image is greater than 38 then point to the next line in the texture
                    //         //        if (platforms >= 38 && platforms < 57)
                    //         //        {
                    //         //            textureY = (unGroTileHeight * 2);
                    //         //            textureX = (38 * unGroTileWidth);
                    //         //        }
                    //
                    //         //        // If the image is greater than 57 then point to the next line in the texture
                    //         //        if (platforms >= 57 && platforms < 76)
                    //         //        {
                    //         //            textureY = (unGroTileHeight * 3);
                    //         //            textureX = (57 * unGroTileWidth);
                    //         //        }
                    //
                    //         //        //Grab the rectangle texture from the underground graphics.png  
                    //         //        Rectangle wallRect = new Rectangle((platforms * unGroTileWidth) - textureX, textureY, unGroTileWidth, unGroTileHeight);
                    //
                    //         //        spriteBatch.Draw(undergroundTiles, new Rectangle((jj * unGroTileWidth),
                    //         //        ii * 48, 62, 48), wallRect, Color.White);
                    //
                    //         //        Rectangle collisionRects = new Rectangle((jj * unGroTileWidth), ii * 48, 62, 48);
                    //
                    //         //        // Store the tiles to collide with in a List of Rectangles
                    //         //        // Check if floor tiles exist & only 1 screen high (i!) 
                    //
                    //         //        // Alchiems
                    //         //        if (platforms == 19 || platforms == 20 || platforms == 21 || platforms == 22)
                    //         //        {
                    //         //            alchiems.Add(collisionRects);
                    //         //        }
                    //         //        // Mole edible wall textures
                    //         //        if (platforms == 15 || platforms == 16 || platforms == 17 || platforms == 18)
                    //         //        {
                    //         //            edibleWalls.Add(collisionRects);
                    //         //        }
                    //         //        // Floor textures
                    //         //        if (platforms == 5 || platforms == 6 || platforms == 7 || platforms == 8
                    //         //            || platforms == 24 || platforms == 25 || platforms == 26
                    //         //            || platforms == 43 || platforms == 44)
                    //         //        {
                    //         //            rects.Add(collisionRects);
                    //         //        }
                    //         //        // Platforms & Ledges
                    //         //        if (platforms == 9 || platforms == 10 || platforms == 11 || platforms == 13
                    //         //            || platforms == 14 || platforms == 28 || platforms == 29
                    //         //            || platforms == 47 || platforms == 48)
                    //         //        {
                    //         //            platform.Add(collisionRects);
                    //
                    //         //            //Copy the x,y co-ord from the platform into the test
                    //         //            test.Location = collisionRects.Location;
                    //         //            testList.Add(test);
                    //         //        }
                    //         //        // Wall textures                               
                    //         //        if (platforms == 0 || platforms == 1 || platforms == 2 || platforms == 3)
                    //         //        {
                    //         //            walls.Add(collisionRects);
                    //         //        }
                    //         //        // Roof textures
                    //         //        if (platforms >= 57 && platforms < 76)
                    //         //        {
                    //         //            roof.Add(collisionRects);
                    //         //        }
                    //         //    }
                    //         //}
                    //
                    foreach (var en in enemies) en.Draw(spriteBatch);

                    break;
                }
            }

            if (moleManAlive) mole.Draw(spriteBatch);
            man.Draw(spriteBatch, gameSpritesRect);
            spriteBatch.DrawString(font, "Screen counter : " + screenCounter, new Vector2(0.0f, 30.0f), Color.White);

            spriteBatch.Draw(panel, new Vector2(30, 550), Color.White);
            spriteBatch.Draw(gameSprites, new Vector2(390.0f, 535.0f), heartBeatRect, Color.White);
            spriteBatch.DrawString(font, "" + alchiem, new Vector2(250.0f, 555.0f), Color.White);
            spriteBatch.DrawString(font, "" + man.Lives, new Vector2(395.0f, 555.0f), Color.White);

            switch (seconds)
            {
                case < 10 when minutes == 0:
                    spriteBatch.DrawString(font, "00:0" + seconds, new Vector2(635.0f, 555.0f), Color.White);
                    break;
                case >= 10 when minutes == 0:
                    spriteBatch.DrawString(font, "00:" + seconds, new Vector2(635.0f, 555.0f), Color.White);
                    break;
            }
            switch (seconds)
            {
                case < 10 when minutes > 0:
                    spriteBatch.DrawString(font, "0" + minutes + ":0" + seconds, new Vector2(635.0f, 555.0f), Color.White);
                    break;
                case >= 10 when minutes > 0:
                    spriteBatch.DrawString(font, "0" + minutes + ":" + seconds, new Vector2(635.0f, 555.0f), Color.White);
                    break;
            }
        }

        spriteBatch.End();
        base.Draw(gameTime);
    }

    private void ClearAll()
    {
        enemies.Clear();
        rects.Clear();
        walls.Clear();
        platform.Clear();
        edibleWalls.Clear();
        alchiems.Clear();
        if (roof.Count > 0) roof.Clear();
    }

    private void UpdateInput()
    {
        var gamePad = GamePad.GetState(PlayerIndex.One);
        var keyboard = Keyboard.GetState();

        if (!gameOn)
        {
            // Allows the game to exit
            if (gamePad.Buttons.Back == ButtonState.Pressed ||
                keyboard.IsKeyDown(Keys.Escape))
                Exit();

            if (gamePad.Buttons.Back == ButtonState.Pressed ||
                keyboard.IsKeyDown(Keys.X))
                gameOn = true;
        }
        else
        {
            if (gamePad.Buttons.Back == ButtonState.Pressed ||
                keyboard.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboard.IsKeyDown(Keys.M) && belowMoon)
            {
                mole.MolePosX = man.XPosition;
                mole.MolePosY = man.YPosition;
                moleManAlive = true;
            }

            if (keyboard.IsKeyDown(Keys.N)) moleManAlive = false;

            if (gamePad.DPad.Left == ButtonState.Pressed ||
                (keyboard.IsKeyDown(Keys.Left) && !man.Falling && !moleManAlive))
            {
                man.FacingLeft = true;
                man.XPosition -= 2;
                animTimer += ElapsedSecs;
                if (animTimer > 0.6)
                {
                    animTimer = 0;
                    currentFrame++;
                }

                if (currentFrame >= 6) currentFrame = 0;
            }

            if (gamePad.DPad.Left == ButtonState.Pressed ||
                (keyboard.IsKeyDown(Keys.Left) && moleManAlive))
            {
                mole.Direction = true;
                mole.MolePosX -= 2;
            }


            if (gamePad.DPad.Right == ButtonState.Pressed ||
                (keyboard.IsKeyDown(Keys.Right) && !man.Falling && !moleManAlive))
            {
                man.XPosition += 2;
                man.FacingLeft = false;

                animTimer += ElapsedSecs;
                if (animTimer > 0.6)
                {
                    animTimer = 0;
                    currentFrame++;
                }

                if (currentFrame >= 6) currentFrame = 0;
            }

            if (gamePad.DPad.Left == ButtonState.Pressed ||
                (keyboard.IsKeyDown(Keys.Right) && moleManAlive))
            {
                mole.Direction = false;
                mole.MolePosX += 2;
            }

            if (gamePad.DPad.Left == ButtonState.Pressed ||
                (keyboard.IsKeyDown(Keys.Down) && moleManAlive))
                mole.MolePosY += 2;

            if (gamePad.DPad.Left == ButtonState.Pressed ||
                (keyboard.IsKeyDown(Keys.Up) && moleManAlive))
                mole.MolePosY -= 2;

            if (keyboard.IsKeyDown(Keys.Right) && keyboard.IsKeyDown(Keys.LeftControl)
                                               && oldState.IsKeyUp(Keys.LeftControl) && !man.Falling)
            {
                man.WalkingOnFloor = false;
                man.summerSaultJump = true;
                jumpRight = true;
                trip = true;
            }

            if (keyboard.IsKeyDown(Keys.Left) && keyboard.IsKeyDown(Keys.LeftControl)
                                              && oldState.IsKeyUp(Keys.LeftControl) && !man.Falling)
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
                man.XPosition = 100;
                man.YPosition = 350;
                screenCounter = 0;
                ClearAll();
            }

            if (keyboard.IsKeyDown(Keys.F2)) man.Falling = false;

            if (gamePad.Buttons.A == ButtonState.Pressed || (keyboard.IsKeyDown(Keys.LeftControl)
                                                             && belowMoon && !man.Falling && !man.summerSaultJump))
            {
                tempY = man.YPosition;
                man.Jump = true;
            }

            oldState = keyboard;
        }
    }
}