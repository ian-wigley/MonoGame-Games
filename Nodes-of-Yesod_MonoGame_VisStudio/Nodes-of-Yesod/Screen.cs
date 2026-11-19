using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace Nodes_of_Yesod;

public class Screen
{
    public string Name { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public int ScreenCounter { get; set; }

    public List<TileData> Tiles { get; set; } =
    [
        new()
    ];

    public List<Rectangle> RectangleList { get; set; } = [];

    public List<Enemy> Enemies { get; set; } = [];

    public List<EdibleWallData> EdibleWall { get; set; } = [];
}

public class TileData
{
    public int Sx { get; set; }
    public int Sy { get; set; }
    public int Sw { get; set; }
    public int Sh { get; set; }
    public int Dx { get; set; }
    public int Dy { get; set; }
    public int Dw { get; set; }
    public int Dh { get; set; }

    [JsonIgnore] public Rectangle TileDestination => new(Dx, Dy, Dw, Dh);

    [JsonIgnore] public Rectangle TileSource => new(Sx, Sy, Sw, Sh);
}

public class EdibleWallData
{
    public Rectangle Rect { get; set; }

    public List<int> TileIds { get; set; } = [];
    public int OtherScreen { get; set; }
    public List<int> OtherTiles { get; set; } = [];
}