using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

public class GraphicsManager(Texture2D gamesprites, List<Enemy> enemies, List<Rectangle> walls, List<Rectangle> plats)
{
    private static readonly Random Rand = new();


    public List<Screen> Screens { get; } = [];

    
    public int[,] HoleArray0 { get; } =
    {
        { 0 }, { 2 }, { 2 }, { 0 }, { 2 }, { 2 }, { 2 }, { 2 }, { 2 }, { 2 }, { 2 }, { 2 }, { 2 }, { 0 }, { 2 },
        { 2 }
    };

    public int[,] HoleArray1 { get; } =
    {
        { 0 }, //0
        { 2 }, //1
        { 2 }, //2
        { 2 }, //3
        { 0 }, //4
        { 2 }, //5
        { 2 }, //6
        { 2 }, //7
        { 0 }, //8
        { 2 }, //9
        { 0 }, //10
        { 2 }, //11
        { 2 }, //12
        { 2 }, //13
        { 2 }, //14
        { 2 }
    };

    public int[,] LevelTiles { get; } = new int[3181, 13];

    private int[,] EnemyCollection => new int[256, 3];

    // public List<Level> LevelList { get; } = [];

    public async Task LoadJsonLevel()
    {
        try
        {
            var path = Directory.GetCurrentDirectory() + @"\Levels\levels.json";
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new RectangleJsonConverter(), new EdibleWallConverter() }
            };

            var levels = await File.ReadAllTextAsync(path);
            var jsonLevels = JsonSerializer.Deserialize<List<Screen>>(levels, options);

            if (jsonLevels != null) Screens.AddRange(jsonLevels);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading levels.json: {ex.Message}");
        }
    }

    public void ConfigureEnemies(int belowScreenCounter)
    {
        for (var i = 0; i < 3; i++)
        {
            var index = EnemyCollection[belowScreenCounter, i];
            switch (index)
            {
                case 1: //a
                    enemies.Add(new Alf(300.0f, 366.0f, 1.0f, gamesprites, walls));
                    break;
                case 2: //b
                    enemies.Add(new Bird(300.0f, 366.0f, 1.0f, gamesprites, walls));
                    break;
                case 3: //c
                    enemies.Add(new Caterpillar(300.0f, 366.0f, 1.0f, gamesprites, walls));
                    break;
                case 4: //d
                    enemies.Add(new GreenMeanie(Rand.Next(100, 400), 366.0f, 1.0f, gamesprites, walls));
                    break;
                case 5: //e
                    enemies.Add(new Fire(300.0f, 372.0f, 1.0f, gamesprites, walls));
                    break;
                case 6: //f
                    enemies.Add(new Fish(300.0f, 300.0f, 1.0f, 1.0f, gamesprites, walls));
                    break;
                case 7: //g
                    //mEnemies.Add(new Plant(300.0f, 300.0f, 1.0f, 1.0f, mSprites, mWalls));
                    enemies.Add(new Plant(300.0f, 300.0f, 1.0f, gamesprites, walls));
                    break;
                case 8: //h
                    enemies.Add(new WhirlWind(Rand.Next(100, 400), Rand.Next(100, 400), 1.0f, 1.0f, gamesprites,
                        walls));
                    break;
                case 9: //i
                    enemies.Add(new WoodLouse(300.0f, 300.0f, 1.0f, gamesprites, walls));
                    break;
                case 10:
                    enemies.Add(new RedSpaceman(Rand.Next(100, 400), Rand.Next(100, 400), 1.0f, 1.0f, gamesprites,
                        walls));
                    break;
                case 11:
                    break;
                case 12:
                    break;
                case 13:
                    break;
            }
        }

        for (var j = 0; j < 3; j++)
        {
            var floatingEnemies = Rand.Next(1, 6);
            switch (floatingEnemies)
            {
                case 1:
                    enemies.Add(new SpringBear(Rand.Next(100, 400), Rand.Next(100, 400), 1.0f, 1.0f, gamesprites,
                        walls, plats));
                    break;
                case 2:
                    enemies.Add(new BlueThingy(Rand.Next(100, 400), Rand.Next(100, 400), 1.0f, 1.0f, gamesprites,
                        walls, plats));
                    break;
                case 3:
                    enemies.Add(new ChasingEnemy(300.0f, 300.0f, 1.0f, 1.0f, gamesprites, walls));
                    break;
                case 4:
                    enemies.Add(new SpringBear(Rand.Next(100, 400), Rand.Next(100, 400), 1.0f, 1.0f, gamesprites,
                        walls, plats));
                    break;
                case 5:
                    enemies.Add(new BlueThingy(Rand.Next(100, 400), Rand.Next(100, 400), 1.0f, 1.0f, gamesprites,
                        walls, plats));
                    break;
                case 6:
                    enemies.Add(new ChasingEnemy(300.0f, 300.0f, 1.0f, 1.0f, gamesprites, walls));
                    break;
            }
        }
    }
}

public class RectangleJsonConverter : JsonConverter<Rectangle>
{
    public override Rectangle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected start of object for Rectangle");

        int left = 0, top = 0, right = 0, bottom = 0;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return new Rectangle(left, top, right, bottom); // x, y, width, height

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            var name = reader.GetString()!;
            if (name != "tileIds")
            {
                reader.Read();
                var value = reader.GetInt32();

                switch (name.ToLowerInvariant())
                {
                    case "left": left = value; break;
                    case "top": top = value; break;
                    case "right": right = value; break;
                    case "bottom": bottom = value; break;
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Rectangle value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("left", value.X);
        writer.WriteNumber("top", value.Y);
        writer.WriteNumber("right", value.Width);
        writer.WriteNumber("bottom", value.Height);
        writer.WriteEndObject();
    }
}

public class EdibleWallConverter : JsonConverter<EdibleWallData>
{
    public override EdibleWallData Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected start of object for EdibleWallData");

        var result = new EdibleWallData();
        int left = 0, top = 0, right = 0, bottom = 0;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                result.Rect = new Rectangle(left, top, right, bottom); // x, y, width, height
                return result;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            var name = reader.GetString()!;
            reader.Read();

            switch (name.ToLowerInvariant())
            {
                case "left": left = reader.GetInt32(); 
                    break;
                case "top": top = reader.GetInt32(); 
                    break;
                case "right": right = reader.GetInt32(); 
                    break;
                case "bottom": bottom = reader.GetInt32(); 
                    break;
                case "tileids":
                    result.TileIds = JsonSerializer.Deserialize<List<int>>(ref reader, options) ?? []; 
                    break;
                case "otherscreen": result.OtherScreen = reader.GetInt32(); 
                    break;
                case "othertiles":
                    result.OtherTiles = JsonSerializer.Deserialize<List<int>>(ref reader, options) ?? []; 
                    break;
                default: reader.Skip(); 
                    break;
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, EdibleWallData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("left", value.Rect.X);
        writer.WriteNumber("top", value.Rect.Y);
        writer.WriteNumber("right", value.Rect.Width);
        writer.WriteNumber("bottom", value.Rect.Height);
    
        writer.WritePropertyName("tileIds");
        JsonSerializer.Serialize(writer, value.TileIds, options);
    
        writer.WriteNumber("otherScreen", value.OtherScreen);
    
        writer.WritePropertyName("otherTiles");
        JsonSerializer.Serialize(writer, value.OtherTiles, options);
    
        writer.WriteEndObject();
    }
}