using Microsoft.Xna.Framework.Graphics;

namespace onwardslib.maps
{
    public class MapTileset
    {
        public Dictionary<int, Sprite> Sprites { get; }

        public MapTileset(Texture2D texture, int initialGid, int tileSize)
        {
            Sprites = new Dictionary<int, Sprite>();

            //TODO
        }
    }
}
