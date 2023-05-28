using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwards.graphics
{
    public class ColorTexture : Texture2D
    {
        static readonly Dictionary<Color, ColorTexture> cached = new Dictionary<Color, ColorTexture>();
        static int defaultWidth = 2;
        static int defaultHeight = 2;

        ColorTexture(Color color) : base(Engine.Instance.Graphics, defaultWidth, defaultHeight)
        {
            var data = new Color[defaultWidth * defaultHeight];
            for (int y = 0; y < defaultHeight; y++)
            for (int x = 0; x < defaultWidth; x++)
                data[y * defaultWidth + x] = color;
            
            SetData(data);
        }

        public static ColorTexture Get(Color color)
        {
            if (!cached.ContainsKey(color))
                return cached[color] = new ColorTexture(color);
            return cached[color];
        }
    }
}