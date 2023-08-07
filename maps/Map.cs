using Microsoft.Xna.Framework;

namespace onwardslib.maps
{
    public class Map
    {
        public const int TILE_SIZE = 16;

        List<MapLayer> _mapLayers;

        public IEnumerable<MapLayer> MapLayers => _mapLayers;
        public Point Size { get; }

        public Map(Point size)
        {
            Size = size;
        }
    }
}
