using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using onwards.editormap;
using onwards.graphics;
using onwards.utils;

namespace onwards
{
    public class MapSerialization
    {
        public EditorMap Load(string filename)
        {
            if (!File.Exists(EngineConstants.MAPS_FOLDER + filename))
            {
                return null;
            }

            var xmlText = File.ReadAllText(EngineConstants.MAPS_FOLDER + filename);

            var doc = new XmlDocument();
            doc.LoadXml(xmlText);

            var mapElement = doc["map"];
            var tilesetsElement = mapElement["tilesets"];
            var layersElement = mapElement["layers"];
            var collisionElement = mapElement["collision"];

            var width = int.Parse(mapElement.Attributes["width"].Value);
            var height = int.Parse(mapElement.Attributes["height"].Value);

            var map = new EditorMap(width, height)
            {
                Filename = filename
            };

            foreach (XmlElement tilesetElement in tilesetsElement)
            {
                map.Tilesets.Add(new EditorTileset
                {
                    InitialGid = int.Parse(tilesetElement.Attributes["initialGid"].Value),
                    Texture = TextureLoader.Get(tilesetElement.Attributes["name"].Value)
                });
            }

            foreach (XmlElement layerElement in layersElement)
            {
                var layer = new EditorMapLayer(map)
                {
                    Name = layerElement.Attributes["name"].Value,
                    IsAbove = bool.Parse(layerElement.Attributes["above"].Value)
                };

                var innerText = layerElement.InnerText;
                var tiles = innerText.Split(',').Select(int.Parse).ToArray();

                for (var y = 0; y < map.Height; y++)
                {
                    for (var x = 0; x < map.Width; x++)
                    {
                        var i = y * map.Width + x;
                        if (tiles[i] != 0)
                        {
                            EditorTileset tileset = null;
                            for (var j = map.Tilesets.Count - 1; j >= 0; j--)
                            {
                                if (map.Tilesets[j].InitialGid <= tiles[i])
                                {
                                    tileset = map.Tilesets[j];
                                    break;
                                }
                            }

                            if (tileset != null)
                            {
                                var id = tiles[i] - tileset.InitialGid;
                                var sX = id % (tileset.Texture.Texture2D.Width / 16);
                                var sY = id / (tileset.Texture.Texture2D.Width / 16);
                                var sprite = new Sprite(tileset.Texture,
                                    new Rectangle(sX * 16, sY * 16, 16, 16)) { Id = tileset.Texture.Name + "_" + id };
                                layer.Sprites[x, y] = sprite;
                            }
                        }
                    }
                }

                map.Layers.Add(layer);
            }

            if (collisionElement != null)
            {
                var collisionText = collisionElement.InnerText;
                var collision = collisionText.Split(',').Select(int.Parse).ToArray();
                for (var y = 0; y < map.Height; y++)
                {
                    for (var x = 0; x < map.Width; x++)
                    {
                        var i = y * map.Width + x;
                        map.Collision[x, y] = collision[i] == 1;
                    }
                }
            }
            
            return map;
        }

        public void Save(EditorMap map, string filename)
        {
            var doc = new XmlDocument();
            var mapElement = doc.CreateElement("map");
            mapElement.SetAttribute("name", "test");
            mapElement.SetAttribute("width", map.Width.ToString());
            mapElement.SetAttribute("height", map.Height.ToString());
            doc.AppendChild(mapElement);

            // tilesets
            var gidPerTileset = new Dictionary<string, int>();
            var tilesetsElement = doc.CreateElement("tilesets");
            foreach (var tileset in map.Tilesets)
            {
                gidPerTileset[tileset.Texture.Name] = tileset.InitialGid;
                
                var tilesetElement = doc.CreateElement("tileset");
                tilesetElement.SetAttribute("name", tileset.Texture.Name);
                tilesetElement.SetAttribute("initialGid", tileset.InitialGid.ToString());
                tilesetsElement.AppendChild(tilesetElement);
            }
            mapElement.AppendChild(tilesetsElement);

            // layers
            var layersElement = doc.CreateElement("layers");
            mapElement.AppendChild(layersElement);

            foreach (var layer in map.Layers)
            {
                var layerElement = doc.CreateElement("layer");
                layersElement.AppendChild(layerElement);

                layerElement.SetAttribute("name", layer.Name);
                layerElement.SetAttribute("above", layer.IsAbove.ToString());

                var tilesList = new List<int>();
                for (var y = 0; y < map.Height; y++)
                {
                    for (var x = 0; x < map.Width; x++)
                    {
                        var sprite = layer.Sprites[x, y];
                        if (sprite != null)
                        {
                            var split = sprite.Id.Split("_");
                            var name = sprite.Id.Substring(0, sprite.Id.LastIndexOf("_", StringComparison.Ordinal));
                            var index = int.Parse(split[^1]);
                            tilesList.Add(index + gidPerTileset[name]);
                        }
                        else
                        {
                            tilesList.Add(0);
                        }
                    }
                }

                layerElement.InnerText = string.Join(',', tilesList.Select(i => i.ToString()));
            }

            // collisions
            var collisionElement = doc.CreateElement("collision");
            mapElement.AppendChild(collisionElement);

            var collisionList = new List<int>();
            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    collisionList.Add(map.Collision[x, y] ? 1 : 0);
                }
            }
            collisionElement.InnerText = string.Join(',', collisionList.Select(i => i.ToString()));

            var filePath = EngineConstants.MAPS_FOLDER + filename;
            var file = new FileInfo(filePath);
            file?.Directory?.Create();

            doc.Save(filePath);
        }
    }
}