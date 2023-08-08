using Microsoft.Xna.Framework;

namespace onwardslib.maps
{
    public class Map : IDisposable
    {
        public const int TILE_SIZE = 16;

        public IEnumerable<MapLayer> MapLayers => _mapLayers;
        public Point Size { get; }

        List<MapLayer> _mapLayers = new();
        List<MapTileset> _mapTilesets = new();
        Dictionary<int, Sprite> _loadedSpritesByGid = new();

        public Map(Point size)
        {
            Size = size;
        }

        public Sprite GetSpriteByGid(int gid)
        {
            if (!_loadedSpritesByGid.ContainsKey(gid))
            {
                throw new Exception($"No sprite for gid {gid}");
            }

            return _loadedSpritesByGid[gid];
        }

        public void Dispose()
        {
            _mapLayers.Clear();
            _mapTilesets.Clear();
            _loadedSpritesByGid.Clear();
        }
    }
}
