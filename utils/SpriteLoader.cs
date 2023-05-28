using System.Collections.Generic;
using Microsoft.Xna.Framework;
using onwards.graphics;

namespace onwards.utils
{
    public static class SpriteLoader
    {
        static Dictionary<string, Sprite> _loadedSprites = new Dictionary<string, Sprite>();

        public static Sprite Get(string id)
        {
            if (!_loadedSprites.TryGetValue(id, out var sprite))
            {
                sprite = Sprite.FromId(id);
                _loadedSprites[sprite.Id] = sprite;
            }

            return sprite;
        }

        public static Sprite Get(string textureName, Rectangle sourceRectangle)
        {
            return Get(Sprite.GetId(textureName, sourceRectangle));
        }
    }
}
