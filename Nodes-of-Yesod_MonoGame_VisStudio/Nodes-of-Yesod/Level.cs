using System.Collections.Generic;

namespace Nodes_of_Yesod
{
    public class Level
    {

        public Level(int levelNumber)
        {
            LevelNumber = levelNumber;
        }

        public void AddTile(Tile tile)
        {
            TileList.Add(tile);
        }

        public int LevelNumber { get; private set; }

        public List<Tile> TileList { get; private set; } = new List<Tile>();

    }
}
