using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public static class TextureLoader
    {
        static IDictionary<string, OTexture> _loadedTextures = new Dictionary<string, OTexture>();
        static readonly Dictionary<Color, OTexture> _colorTextures = new Dictionary<Color, OTexture>();

        public static Func<string, Stream> CustomStreamFunc { get; set; }

        public static OTexture Get(string name, int pixelsToUnit = 16, string extension = ".png")
        {
            if (!_loadedTextures.TryGetValue(name, out var oTexture))
            {
                if (CustomStreamFunc != null)
                {
                    using Stream stream = CustomStreamFunc(name + extension);

                    var texture = FromStream(stream);
                    oTexture = new OTexture(texture, pixelsToUnit);
                }
                else
                {
                    var path = $"{Onwards.DataFolder}/textures/{name}{extension}";
                    using Stream stream = File.OpenRead(path);

                    var texture = FromStream(stream);
                    oTexture = new OTexture(texture, pixelsToUnit) { Name = name };
                }

                _loadedTextures[name] = oTexture;
            }

            return oTexture;
        }

        public static OTexture GetColor(Color color, int width = 2, int height = 2)
        {
            if (!_colorTextures.ContainsKey(color))
            {
                var data = new Color[width * height];
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        data[y * width + x] = color;

                var texture2D = new Texture2D(Onwards.GraphicsDevice, width, height);
                texture2D.SetData(data);

                _colorTextures[color] = new OTexture(texture2D);
            }
            
            return _colorTextures[color];
        }

        static Texture2D FromStream(Stream stream)
        {
            var texture = Texture2D.FromStream(Onwards.GraphicsDevice, stream);

#if !ANDROID
            var data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            for (int i = 0; i != data.Length; ++i)
                data[i] = Color.FromNonPremultiplied(data[i].ToVector4());
            texture.SetData(data);
#endif
            return texture;
        }
    }
}
