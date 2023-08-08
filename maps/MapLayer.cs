namespace onwardslib.maps
{
    public class MapLayer
    {
        public const int MAX_LAYERS = 9999;

        public enum OrderMagnitudeType
        {
            Background = 0, Foreground
        }

        [Flags]
        public enum TileFlipType
        {
            None = 0, Horizontal = 1, Vertical = 2,
        }

        public OrderMagnitudeType OrderMagnitude { get; set; }
        public int Order { get; set; }
        public Map Map { get; }
        public int[,] Tiles { get; private set; }
        public int[,] TileRotations { get; private set; }
        public TileFlipType[,] TileFlips { get; private set; }

        public MapLayer(Map map)
        {
            Map = map;
            Tiles = new int[map.Size.X, map.Size.Y];
            TileRotations = new int[map.Size.X, map.Size.Y];
            TileFlips = new TileFlipType[map.Size.X, map.Size.Y];
        }
    }
}
