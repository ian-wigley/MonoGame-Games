using System.Collections.Generic;

namespace Nodes_of_Yesod;

public class Level(int levelNumber)
{
    public int LevelNumber { get; private set; } = levelNumber;

    public List<Tiled> TileList { get; } = [];

    public void AddTile(Tiled tile)
    {
        TileList.Add(tile);
    }
}