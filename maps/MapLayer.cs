namespace onwardslib.maps
{
    public class MapLayer
    {
        public const int MAX_LAYERS = 9999;

        public enum OrderMagnitudes
        {
            Background = 0, Foreground
        }

        public OrderMagnitudes OrderMagnitude { get; set; }
        public int Order { get; set; }
        public Map Map { get; }
        public int[,] Tiles { get; }

        public MapLayer(Map map)
        {
            Map = map;
            Tiles = new int[map.Size.X, map.Size.Y];
        }
    }
}
