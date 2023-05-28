using System.Linq;
using Microsoft.Xna.Framework;
using onwards.utils;

namespace onwards.graphics
{
    public class Sprite
    {
        public string Id { get; set; }
        public OTexture Texture { get; }
        public Rectangle SourceRectangle { get; protected set; }
        public Point Offset { get; }

        public Sprite(OTexture texture, Rectangle sourceRectangle, Point? offset = null)
        {
            Texture = texture;
            SourceRectangle = sourceRectangle;
            Offset = offset ?? Point.Zero;
        }

        public virtual void Draw(Point position, float opacity = 1f)
        {
            graphics.Draw.DrawSprite(this, position.ToVector2(), Color.White * opacity);
        }

        public virtual void Draw(Rectangle destinationRectangle, float opacity = 1f)
        {
            graphics.Draw.DrawSprite(this, destinationRectangle, Color.White * opacity);
        }

        public string ToId()
        {
            return GetId(Texture.Name, SourceRectangle);
        }

        public static string GetId(string textureName, Rectangle rectangle)
        {
            return $"{textureName}_{rectangle.X}_{rectangle.Y}_{rectangle.Width}_{rectangle.Height}";
        }

        public static Sprite FromId(string id)
        {
            var parts = id.Split('_');
            var rect = new Rectangle(int.Parse(parts[^4]), int.Parse(parts[^3]), int.Parse(parts[^2]), int.Parse(parts[^1]));

            var textureName = string.Join('_', parts.Take(parts.Length - 4));

            var texture = TextureLoader.Get(textureName);
            return new Sprite(texture, rect)
            {
                Id = id,
            };
        }
    }
}