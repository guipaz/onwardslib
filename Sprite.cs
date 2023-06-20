using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public class Sprite
    {
        public string Name { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public bool Flipped { get; set; }

        public Sprite(string name, string texture, Rectangle? rectangle = null)
        {
            Name = name;
            Texture = TextureLoader.Get(texture);
            SourceRectangle = rectangle ?? new Rectangle(0, 0, Texture.Width, Texture.Height);
        }

        public Sprite(string name, Texture2D texture, Rectangle rect)
        {
            Name = name;
            Texture = texture;
            SourceRectangle = rect;
        }

        public Sprite(Texture2D texture, Rectangle rect)
        {
            Name = texture.Name;
            Texture = texture;
            SourceRectangle = rect;
        }

        public Sprite(Texture2D texture)
        {
            Name = texture.Name;
            Texture = texture;
            SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
        }
    }
}
