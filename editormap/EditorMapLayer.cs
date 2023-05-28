using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using onwards.graphics;
using onwards.persistence;

namespace onwards.editormap
{
    public class EditorMapLayer : IPersistent
    {
        public EditorMap Map { get; set; }
        public string Name { get; set; }
        public Sprite[,] Sprites { get; set; }
        public bool IsAbove { get; set; }

        public EditorMapLayer(EditorMap map)
        {
            Map = map;
            Sprites = new Sprite[map.Width, map.Height];
        }

        public void LoadData(XmlData data)
        {
            Name = data.GetAttribute("name", Name);
            IsAbove = data.GetAttribute<bool>("above");

            var innerText = data.Text;
            var tiles = innerText.Split(',').Select(int.Parse).ToArray();

            for (var y = 0; y < Map.Height; y++)
            {
                for (var x = 0; x < Map.Width; x++)
                {
                    var i = y * Map.Width + x;
                    if (tiles[i] != 0)
                    {
                        EditorTileset tileset = null;
                        for (var j = Map.Tilesets.Count - 1; j >= 0; j--)
                        {
                            if (Map.Tilesets[j].InitialGid <= tiles[i])
                            {
                                tileset = Map.Tilesets[j];
                                break;
                            }
                        }

                        if (tileset != null)
                        {
                            Sprite sprite;
                            if (tileset.Animated)
                            {
                                var id = tiles[i] - tileset.InitialGid;
                                //var sY = id / (tileset.Texture.Texture2D.Width / 16);
                                sprite = new AnimatedTile(tileset.Texture,
                                        new Rectangle(0, id * 16, tileset.Texture.Texture2D.Width, 16))
                                    { Id = tileset.Texture.Name + "_" + id };
                            }
                            else
                            {
                                var id = tiles[i] - tileset.InitialGid;
                                var sX = id % (tileset.Texture.Texture2D.Width / 16);
                                var sY = id / (tileset.Texture.Texture2D.Width / 16);
                                sprite = new Sprite(tileset.Texture,
                                        new Rectangle(sX * 16, sY * 16, 16, 16))
                                    { Id = tileset.Texture.Name + "_" + id };
                            }
                            
                            Sprites[x, y] = sprite;
                        }
                    }
                }
            }
        }

        public XmlData SaveData()
        {
            var data = XmlData.Create("layer");
            data["name"] = Name;
            data["above"] = IsAbove.ToString();

            var tilesList = new List<int>();
            for (var y = 0; y < Map.Height; y++)
            {
                for (var x = 0; x < Map.Width; x++)
                {
                    var sprite = Sprites[x, y];
                    if (sprite != null)
                    {
                        var split = sprite.Id.Split("_");
                        var name = sprite.Id.Substring(0, sprite.Id.LastIndexOf("_", StringComparison.Ordinal));
                        var index = int.Parse(split[^1]);
                        tilesList.Add(index + Map.GidPerTileset[name]);
                    }
                    else
                    {
                        tilesList.Add(0);
                    }
                }
            }

            data.Text = string.Join(',', tilesList.Select(i => i.ToString()));
            return data;
        }
    }
}