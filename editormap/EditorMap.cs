using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using onwards.components;
using onwards.ecs;
using onwards.persistence;

namespace onwards.editormap
{
    public class EditorMap : IPersistent, ICollisionValidator
    {
        public const int TileSize = 16;

        public string Name { get; set; }
        public string Filename { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<EditorMapLayer> Layers { get; } = new List<EditorMapLayer>();
        public List<EditorTileset> Tilesets { get; set; } = new List<EditorTileset>();
        public bool[,] Collision { get; set; }
        public bool Clamps { get; set; } = true;
        public Dictionary<string, int> GidPerTileset { get; set; }
        public List<string> LocalEntityDefs { get; set; } = new List<string>();
        public int NextGid { get; set; } = 1;

        public EditorMap()
        {
        }

        public EditorMap(int width, int height)
        {
            Width = width;
            Height = height;

            Collision = new bool[width, height];
        }

        public bool IsInside(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        public bool CollisionValidator_IsValidPosition(ICollider collider, Rectangle rect)
        {
            var pX = rect.X + TileSize / 2;
            var pY = rect.Y + TileSize / 2;

            if (pX < 0 || pY < 0)
                return false;

            var min = new Point(pX / TileSize, pY / TileSize);
            var max = new Point((rect.X + rect.Width + TileSize / 2) / TileSize, (rect.Y + rect.Height + TileSize / 2) / TileSize);

            for (var y = min.Y; y <= max.Y; y++)
            {
                for (var x = min.X; x <= max.X; x++)
                {
                    if (!IsInside(x, y))
                    {
                        return false;
                    }

                    if (Collision[x, y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void LoadData(XmlData data)
        {
            var tilesets = data.GetChild("tilesets");
            var layers = data.GetChild("layers");
            var entityDefs = data.GetChild("entityDefs");
            var collision = data.GetChild("collision");

            Width = data.GetAttribute<int>("width");
            Height = data.GetAttribute<int>("height");
            NextGid = data.GetAttribute<int>("nextGid");

            Filename = data["filename"];
            Collision = new bool[Width, Height];


            if (tilesets != null)
            {
                foreach (var tilesetData in tilesets.GetChildren())
                {
                    var tileset = new EditorTileset();
                    tileset.LoadData(tilesetData);
                    Tilesets.Add(tileset);
                }
            }
            
            if (layers != null)
            {
                foreach (var layerData in layers.GetChildren())
                {
                    var layer = new EditorMapLayer(this);
                    layer.LoadData(layerData);
                    Layers.Add(layer);
                }
            }
            
            if (entityDefs != null)
            {
                foreach (var entityDefData in entityDefs.GetChildren())
                {
                    var entityDef = entityDefData.Text;
                    LocalEntityDefs.Add(entityDef);
                }
            }
            
            if (collision != null)
            {
                var collisionText = collision.Text;
                var collisionString = collisionText.Split(',').Select(int.Parse).ToArray();
                for (var y = 0; y < Height; y++)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        var i = y * Width + x;
                        Collision[x, y] = collisionString[i] == 1;
                    }
                }
            }
        }

        public XmlData SaveData()
        {
            var data = XmlData.Create("map");

            data["filename"] = Filename;
            data["name"] = Name ?? "Map";
            data["width"] = Width.ToString();
            data["height"] = Height.ToString();
            data["nextGid"] = NextGid.ToString();

            GidPerTileset = new Dictionary<string, int>();
            var tilesets = XmlData.Create("tilesets");
            foreach (var tileset in Tilesets)
            {
                GidPerTileset[tileset.Texture.Name] = tileset.InitialGid;
                tilesets.AddChild(tileset.SaveData());
            }
            data.AddChild(tilesets);

            var layers = XmlData.Create("layers");
            foreach (var layer in Layers)
            {
                layers.AddChild(layer.SaveData());
            }
            data.AddChild(layers);
            
            var collision = XmlData.Create("collision");
            var collisionList = new List<int>();
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    collisionList.Add(Collision[x, y] ? 1 : 0);
                }
            }
            collision.Text = string.Join(',', collisionList.Select(i => i.ToString()));
            data.AddChild(collision);

            var entityDefs = XmlData.Create("entityDefs");
            foreach (var entityDef in LocalEntityDefs)
            {
                //entityDefs.AddChild(entityDef.SaveData());
            }
            data.AddChild(entityDefs);

            return data;
        }
    }
}