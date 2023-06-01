using Microsoft.Xna.Framework;

namespace onwardslib
{
    public class Sprite
    {
        public string Name { get; set; }
        public OTexture Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public bool Flipped { get; set; }

        public Sprite(string name, string texture, Rectangle? rectangle = null)
        {
            Name = name;
            Texture = OTexture.Get(texture);
            SourceRectangle = rectangle ?? new Rectangle(0, 0, Texture.Texture2D.Width, Texture.Texture2D.Height);
        }

        public Sprite(string name, OTexture texture, Rectangle rect)
        {
            Name = name;
            Texture = texture;
            SourceRectangle = rect;
        }

        public Sprite(OTexture texture, Rectangle rect)
        {
            Name = texture.Name;
            Texture = texture;
            SourceRectangle = rect;
        }

        public Sprite(OTexture texture)
        {
            Name = texture.Name;
            Texture = texture;
            SourceRectangle = new Rectangle(0, 0, Texture.Texture2D.Width, Texture.Texture2D.Height);
        }
    }
}
