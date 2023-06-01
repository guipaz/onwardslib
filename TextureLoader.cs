using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public static class TextureLoader
    {
        static IDictionary<string, Texture2D> _loadedTextures = new Dictionary<string, Texture2D>();

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
                    var path = $"{Onwards.DataFolder}/textures/{name}{extension}";
                    using Stream stream = File.OpenRead(path);
                    texture = FromStream(stream);
                    texture.Name = name;
                }

                _loadedTextures[name] = texture;
            }

            return texture;
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
