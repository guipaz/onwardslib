using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using onwardslib.ecs;
using onwardslib.maps;

namespace onwardslib.parts
{
    public class MapLayerRenderer : Part, IRenderer
    {
        public int RenderOrder => (MapLayer.OrderMagnitude == MapLayer.OrderMagnitudeType.Background ? int.MinValue : (int.MaxValue - MapLayer.MAX_LAYERS)) + MapLayer.Order;
        public MapLayer MapLayer { get; set; }

        RenderTarget2D _bakedTexture;

        public override void Load()
        {
            BakeTexture();
        }

        public void Render()
        {
            ODraw.Draw(_bakedTexture);
        }

        void BakeTexture()
        {
            using (var renderCamera = new Camera(MapLayer.Map.Size.X * Map.TILE_SIZE, MapLayer.Map.Size.Y * Map.TILE_SIZE))
            {
                var tileOffset = new Vector2(Map.TILE_SIZE / 2);

                ODraw.Begin(renderCamera);

                for (var y = 0; y < MapLayer.Map.Size.Y; y++)
                {
                    for (var x = 0; x < MapLayer.Map.Size.X; x++)
                    {
                        var dX = x * Map.TILE_SIZE;
                        var dY = y * Map.TILE_SIZE;
                        var spriteId = MapLayer.Tiles[x, y];

                        if (spriteId != 0)
                        {
                            var sprite = MapLayer.Map.GetSpriteByGid(spriteId);
                            var rotations = MapLayer.TileRotations[x, y];
                            var flips = MapLayer.TileFlips[x, y];
                            var flipH = (flips & MapLayer.TileFlipType.Horizontal) == MapLayer.TileFlipType.Horizontal;
                            var flipV = (flips & MapLayer.TileFlipType.Vertical) == MapLayer.TileFlipType.Vertical;

                            ODraw.Draw(sprite.Texture, new Rectangle(dX, dY, Map.TILE_SIZE, Map.TILE_SIZE),
                                        sprite.SourceRectangle, 1f, rotations * -90,
                                        flipH, flipV, tileOffset);
                        }

                    }
                }

                ODraw.End();

                var data = new Color[renderCamera.Width * renderCamera.Height];
                renderCamera.RenderTarget.GetData(data, 0, renderCamera.Width * renderCamera.Height);
                _bakedTexture = new RenderTarget2D(Engine.GraphicsDevice, renderCamera.Width, renderCamera.Height);
                _bakedTexture.SetData(data);
            }
        }

        public override void Dispose()
        {
            _bakedTexture.Dispose();
        }
    }
}
