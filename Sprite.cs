using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public bool FlipH { get; set; }
        public bool FlipV { get; set; }

        public Sprite(string texture, Rectangle? rectangle = null)
        {
            Texture = TextureLoader.Get(texture);
            SourceRectangle = rectangle ?? new Rectangle(0, 0, Texture.Width, Texture.Height);
        }

        public Sprite(Texture2D texture, Rectangle rect)
        {
            Texture = texture;
            SourceRectangle = rect;
        }

        public Sprite(Texture2D texture) : this(texture, new Rectangle(0, 0, texture.Width, texture.Height))
        {
        }

        public static Sprite Create(Texture2D tex, Rectangle rect)
        {
            return new Sprite(tex, rect);
        }
    }
}
