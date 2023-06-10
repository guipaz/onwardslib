using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public class OTexture
    {
        public string Name { get; set; }
        public Texture2D Texture2D { get; protected set; }
        public int PixelsToUnit { get; } = 16;

        public OTexture(Texture2D texture2D, int pixelsToUnit = 16)
        {
            if (texture2D == null)
            {
                throw new Exception("Null texture");
            }

            Name = texture2D.Name;
            Texture2D = texture2D;
            PixelsToUnit = pixelsToUnit;
        }

        public static OTexture Get(string textureName, int pixelsToUnit = 16)
        {
            return TextureLoader.Get(textureName, pixelsToUnit);
        }
    }
}
