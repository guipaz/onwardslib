using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public class Sprite
    {
        public static Dictionary<string, Sprite> _createdSprites = new();
        public static Sprite Get(Texture2D texture, Rectangle rect)
        {
            var id = texture.Name + "_" + rect.ToString();
            if (!_createdSprites.ContainsKey(id))
            {
                _createdSprites[id] = new Sprite(texture, rect);
            }
            return _createdSprites[id];
        }

        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        
        protected Sprite(Texture2D texture, Rectangle rect)
        {
            Texture = texture;
            SourceRectangle = rect;
        }

        public static Sprite Create(Texture2D tex, Rectangle rect)
        {
            return Sprite.Get(tex, rect);
        }
    }
}
