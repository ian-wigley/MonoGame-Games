using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Nodes_of_Yesod
{
    public class GraphicsManager
    {
        private readonly List<Enemy> mEnemies;
        private readonly List<Rectangle> mWalls;
        private readonly List<Rectangle> mPlatforms;
        private readonly static Random rand = new Random();
        private readonly Texture2D mSprites;

        private readonly int[,] mUpperRockArray = {{0,1,1,1,1,2,2,2,3,3,3},    //0     
                                        {2,1,3,0,1,3,1,0,2,2,1},    //1
                                        {1,2,3,0,1,3,3,0,3,2,1},    //2
                                        {3,1,2,0,3,2,0,3,2,0,1},    //3
                                        {1,2,3,0,0,3,2,3,2,1,0},    //4
                                        {2,1,3,0,1,3,1,0,2,2,1},    //5
                                        {1,2,3,0,1,3,3,0,3,2,1},    //6
                                        {0,2,3,3,0,1,3,3,2,1,1},    //7
                                        {2,1,3,0,1,3,1,0,2,2,1},    //8
                                        {1,2,3,0,1,3,3,0,3,2,1},    //9
                                        {3,1,2,0,3,2,0,3,2,0,1},    //10
                                        {1,2,3,0,0,3,2,3,2,1,0},    //11
                                        {2,1,3,0,1,3,1,0,2,2,1},    //12
                                        {1,2,3,0,1,3,3,0,3,2,1},    //13
                                        {0,2,3,3,0,1,3,3,2,1,1},    //14
                                        {1,2,3,1,2,3,1,3,2,1,1}};   //15

        private readonly int[,] mMoundArray = { { 1 }, { 1 }, { 1 }, { 2 }, { 1 }, { 2 }, { 2 }, { 2 }, { 1 }, { 1 }, { 2 }, { 1 }, { 2 }, { 2 }, { 2 }, { 2 } };
        private readonly int[,] mHoleArray0 = { { 0 }, { 2 }, { 2 }, { 0 }, { 2 }, { 2 }, { 2 }, { 2 }, { 2 }, { 2 }, { 2 }, { 2 }, { 2 }, { 0 }, { 2 }, { 2 } };


        private readonly int[,] mHoleArray1 = {{0}, //0
                                    {2}, //1
                                    {2}, //2
                                    {2}, //3
                                    {0}, //4
                                    {2}, //5
                                    {2}, //6
                                    {2}, //7
                                    {0}, //8
                                    {2}, //9
                                    {0}, //10
                                    {2}, //11
                                    {2}, //12
                                    {2}, //13
                                    {2}, //14
                                    {2}}; //15


        private readonly int[,] mLowerRockArray = {{0,1,2,3,2,1,3,0,2,1,0},    //0     
                                        {2,1,3,0,1,3,1,0,2,2,1},    //1
                                        {1,2,3,0,1,3,3,0,3,2,1},    //2
                                        {3,1,2,0,3,2,0,3,2,0,1},    //3
                                        {1,2,3,0,0,3,2,3,2,1,0},    //4
                                        {2,1,3,0,1,3,1,0,2,2,1},    //5
                                        {1,2,3,0,1,3,3,0,3,2,1},    //6
                                        {0,2,3,3,0,1,3,3,2,1,1},    //7
                                        {2,1,3,0,1,3,1,0,2,2,1},    //8
                                        {1,2,3,0,1,3,3,0,3,2,1},    //9
                                        {3,1,2,0,3,2,0,3,2,0,1},    //10
                                        {1,2,3,0,0,3,2,3,2,1,0},    //11
                                        {2,1,3,0,1,3,1,0,2,2,1},    //12
                                        {1,2,3,0,1,3,3,0,3,2,1},    //13
                                        {0,2,3,3,0,1,3,3,2,1,1},    //14
                                        {1,2,3,1,2,3,1,3,2,1,1}};   //15


        public GraphicsManager(Texture2D gamesprites, List<Enemy> enemies, List<Rectangle> walls, List<Rectangle> plats)
        {
            mSprites = gamesprites;
            mEnemies = enemies;
            mWalls = walls;
            mPlatforms = plats;
        }

        public void LoadLevels()
        {
            string wordLine;
            const int tileColCount = 13;
            const int unGroTileHeight = 48;
            const int unGroTileWidth = 62;

            int testCounter = 0;
            Level level = new Level(0);

            try
            {
                StreamReader wordFile = new StreamReader(Directory.GetCurrentDirectory() + "\\levels.txt");
                while ((wordLine = wordFile.ReadLine()) != null)
                {
                    if (testCounter == 0)
                    {
                        level = new Level(LevelList.Count);
                        LevelList.Add(level);
                    }

                    string[] mFileContents = wordLine.Split(new char[] { ',' });
                    for (int i = 0; i < tileColCount; i++)
                    {
                        var value = int.Parse(mFileContents[i]);

                        if (value != 4)
                        {
                            level.AddTile(new Tile(value, i * unGroTileWidth, testCounter * unGroTileHeight));
                        }
                    }
                    testCounter = (testCounter + 1) % 10;
                }
                wordFile.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("The following error occured while attempting to read the file: " + e.Message);
            }
        }

        public void ConfigureEnemies(int belowScreenCounter)
        {
            int index;
            int floatingEnemies;
            for (int i = 0; i < 3; i++)
            {
                index = EnemyCollection[belowScreenCounter, i];
                switch (index)
                {
                    case 1:  //a
                        mEnemies.Add(new Alf(300.0f, 366.0f, 1.0f, mSprites, mWalls));
                        break;
                    case 2:  //b
                        mEnemies.Add(new Bird(300.0f, 366.0f, 1.0f, mSprites, mWalls));
                        break;
                    case 3:  //c
                        mEnemies.Add(new Caterpillar(300.0f, 366.0f, 1.0f, mSprites, mWalls));
                        break;
                    case 4:  //d
                        mEnemies.Add(new GreenMeanie((float)rand.Next(100, 400), 366.0f, 1.0f, mSprites, mWalls));
                        break;
                    case 5:  //e
                        mEnemies.Add(new Fire(300.0f, 372.0f, 1.0f, mSprites, mWalls));
                        break;
                    case 6:  //f
                        mEnemies.Add(new Fish(300.0f, 300.0f, 1.0f, 1.0f, mSprites, mWalls));
                        break;
                    case 7:  //g
                        mEnemies.Add(new Plant(300.0f, 300.0f, 1.0f, 1.0f, mSprites, mWalls));
                        break;
                    case 8:  //h
                        mEnemies.Add(new WhirlWind((float)rand.Next(100, 400), (float)rand.Next(100, 400), 1.0f, 1.0f, mSprites, mWalls));
                        break;
                    case 9:  //i
                        mEnemies.Add(new WoodLouse(300.0f, 300.0f, 1.0f, mSprites, mWalls));
                        break;
                    case 10:
                        mEnemies.Add(new RedSpaceman((float)rand.Next(100, 400), (float)rand.Next(100, 400), 1.0f, 1.0f, mSprites, mWalls));
                        break;
                    case 11:
                        break;
                    case 12:
                        break;
                    case 13:
                        break;
                }
            }
            for (int j = 0; j < 3; j++)
            {
                floatingEnemies = rand.Next(1, 6);
                switch (floatingEnemies)
                {
                    case 1:
                        mEnemies.Add(new SpringBear((float)rand.Next(100, 400), (float)rand.Next(100, 400), 1.0f, 1.0f, mSprites, mWalls, mPlatforms));
                        break;
                    case 2:
                        mEnemies.Add(new BlueThingy((float)rand.Next(100, 400), (float)rand.Next(100, 400), 1.0f, 1.0f, mSprites, mWalls, mPlatforms));
                        break;
                    case 3:
                        mEnemies.Add(new ChasingEnemy(300.0f, 300.0f, 1.0f, 1.0f, mSprites, mWalls));
                        break;
                    case 4:
                        mEnemies.Add(new SpringBear((float)rand.Next(100, 400), (float)rand.Next(100, 400), 1.0f, 1.0f, mSprites, mWalls, mPlatforms));
                        break;
                    case 5:
                        mEnemies.Add(new BlueThingy((float)rand.Next(100, 400), (float)rand.Next(100, 400), 1.0f, 1.0f, mSprites, mWalls, mPlatforms));
                        break;
                    case 6:
                        mEnemies.Add(new ChasingEnemy(300.0f, 300.0f, 1.0f, 1.0f, mSprites, mWalls));
                        break;
                }
            }
        }

        public int[,] UpperRockArray { get { return mUpperRockArray; } }

        public int[,] MoundArray { get { return mMoundArray; } }

        public int[,] HoleArray0 { get { return mHoleArray0; } }

        public int[,] HoleArray1 { get { return mHoleArray1; } }

        public int[,] LowerRockArray { get { return mLowerRockArray; } }

        public int[,] LevelTiles { get; set; } = new int[3181, 13];

        public int[,] EnemyCollection { get { return new int[256, 3]; } }

        public List<Level> LevelList { get; private set; } = new List<Level>();

    }
}
