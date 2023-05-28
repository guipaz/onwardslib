using Microsoft.Xna.Framework.Graphics;

namespace onwards.graphics
{
    public class OTexture
    {
        public const int DEFAULT_PIXELS_TO_UNIT = 16;

        public string Name { get; set; }
        public Texture2D Texture2D { get; }
        public int PixelsToUnit { get; }
        
        public OTexture(Texture2D texture2D, int pixelsToUnit = DEFAULT_PIXELS_TO_UNIT)
        {
            Name = texture2D?.Name;
            Texture2D = texture2D;
            PixelsToUnit = pixelsToUnit;
        }
    }
}