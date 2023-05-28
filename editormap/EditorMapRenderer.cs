using Microsoft.Xna.Framework;
using onwards.graphics;

namespace onwards.editormap
{
    public class EditorMapRenderer
    {
        public EditorMap Map { get; set; }

        public virtual void DrawLayers(float opacity = 1f)
        {
            foreach (var mapLayer in Map.Layers)
            {
                if (!mapLayer.IsAbove)
                    DrawLayer(mapLayer);
            }
        }

        public virtual void DrawAboveLayers(float opacity = 1f)
        {
            //TODO fix this later
            foreach (var mapLayer in Map.Layers)
            {
                if (mapLayer.IsAbove)
                    DrawLayer(mapLayer, opacity);
            }
        }

        public virtual void DrawCollisions()
        {
            for (var y = 0; y < Map.Height; y++)
            {
                for (var x = 0; x < Map.Width; x++)
                {
                    if (Map.Collision[x, y])
                    {
                        Draw.DrawTexture(ColorTexture.Get(Color.Red),
                            new Rectangle(x * EditorMap.TileSize - EditorMap.TileSize / 2, y * EditorMap.TileSize - EditorMap.TileSize / 2,
                                EditorMap.TileSize, EditorMap.TileSize), Color.White * .5f);
                    }
                }
            }
        }

        public void DrawLayer(EditorMapLayer layer, float opacity = 1f)
        {
            for (var y = 0; y < Map.Height; y++)
            {
                for (var x = 0; x < Map.Width; x++)
                {
                    var sprite = layer.Sprites[x, y];
                    if (sprite != null)
                    {
                        //Draw.DrawSprite(sprite, new Vector2(x * Map.TileSize - Map.TileSize / 2, y * Map.TileSize - Map.TileSize / 2), Color.White * opacity);
                        sprite.Draw(new Point(x * EditorMap.TileSize - EditorMap.TileSize / 2, y * EditorMap.TileSize - EditorMap.TileSize / 2), opacity);
                    }
                }
            }
        }
    }
}