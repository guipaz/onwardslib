using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwardslib.maps
{
    public class MapTileset
    {
        public Dictionary<int, Sprite> Sprites { get; }

        public MapTileset(Texture2D texture, int initialGid, int tileSize)
        {
            Sprites = new Dictionary<int, Sprite>();

            var amountX = texture.Width / tileSize;
            var amountY = texture.Height / tileSize;

            var i = 0;
            for (var y = 0; y < amountY; y++)
            {
                for (var x = 0; x < amountX; x++)
                {
                    Sprites[initialGid + i] = Sprite.Create(texture, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));
                    i++;
                }
            }
        }
    }
}
