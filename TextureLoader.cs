using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public static class TextureLoader
    {
        static IDictionary<string, Texture2D> _loadedTextures = new Dictionary<string, Texture2D>();
        static readonly Dictionary<Color, Texture2D> _colorTextures = new Dictionary<Color, Texture2D>();

        public static Func<string, Stream> CustomStreamFunc { get; set; }

        public static Texture2D Get(string name, string extension = ".png")
        {
            if (!_loadedTextures.TryGetValue(name, out var texture))
            {
                if (CustomStreamFunc != null)
                {
                    using Stream stream = CustomStreamFunc(name + extension);

                    texture = FromStream(stream);
                }
                else
                {
                    var path = $"{Engine.DataFolder}/textures/{name}{extension}";
                    using Stream stream = File.OpenRead(path);

                    texture = FromStream(stream);
                }

                texture.Name = name;
                _loadedTextures[name] = texture;
            }

            return texture;
        }

        public static Texture2D GetColor(Color color, int width = 2, int height = 2)
        {
            if (!_colorTextures.ContainsKey(color))
            {
                var data = new Color[width * height];
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        data[y * width + x] = color;

                var texture2D = new Texture2D(Engine.GraphicsDevice, width, height);
                texture2D.SetData(data);

                _colorTextures[color] = texture2D;
            }
            
            return _colorTextures[color];
        }

        static Texture2D FromStream(Stream stream)
        {
            var texture = Texture2D.FromStream(Engine.GraphicsDevice, stream);

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
