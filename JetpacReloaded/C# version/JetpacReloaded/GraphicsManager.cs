using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JetPacReloaded;

public class GraphicsManager
{
    private const int TileRowCount = 20;
    private const int TileColCount = 43;
    private static readonly char[] Separator = ['.'];
    private List<Tile> _tileList = [];

    private int[,] LevelTiles { get; } = new int[TileRowCount, TileColCount];

    public List<Tile> TileList { get; } = [];

    public void LoadLevels()
    {
        var count = 0;
        const int unGroTileHeight = 48;
        const int unGroTileWidth = 62;
        try
        {
            var levels = string.Join(Path.DirectorySeparatorChar,
                             Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar).SkipLast(3)) +
                         "/Levels/levels.txt";
            var wordFile = new StreamReader(levels);
            while (wordFile.ReadLine() is { } wordLine)
            {
                var mFileContents = wordLine.Split(new[] { ',' });
                for (var i = 0; i < TileColCount - 1; i++)
                    // Animated tiles contain 'a'
                    if (mFileContents[i].Contains('a'))
                    {
                        var animationDetail = mFileContents[i].Split(Separator);
                        var j = int.Parse(animationDetail[0]);
                        TileList.Add(new TileAnimation(int.Parse(animationDetail[0]), i * unGroTileWidth,
                            count * unGroTileHeight, int.Parse(animationDetail[2]), GetTileType(j)));
                    }
                    else
                    {
                        var value = int.Parse(mFileContents[i]);
                        LevelTiles[count, i] = value;
                        if (value != 0)
                            TileList.Add(new TileNoAnimation(value, i * unGroTileWidth, count * unGroTileHeight,
                                GetTileType(value)));
                    }

                count++;
            }

            wordFile.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("The following error occured while attempting to read the file: " + e.Message);
        }
        //           WriteDebugInformation();
    }

    public void WriteDebugInformation()
    {
        File.WriteAllLines(Directory.GetCurrentDirectory() + "\\Debug.txt",
            TileList.Select(i =>
                "Tile X : " + i.TileRect.X + ", Tile Y : " + i.TileRect.Y + ", Tile Type " +
                i.GetTileCollisionType));
    }


    private TileCollisionType GetTileType(int value)
    {
        if (value is >= 1 and <= 4) return TileCollisionType.Wall;

        return value is >= 5 and <= 8 ? TileCollisionType.Platform : TileCollisionType.Impassable;
    }
}