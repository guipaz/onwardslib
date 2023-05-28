using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using onwards.graphics;

namespace onwards.utils
{
    public static class TextureLoader
    {
        static Dictionary<string, OTexture> loadedTextures = new Dictionary<string, OTexture>();
        
        public static OTexture Get(string name, int pixelsPerUnit = 16, string extension = ".png")
        {
            if (!loadedTextures.TryGetValue(name, out var texture))
            {
                string path;
                if (!name.EndsWith(extension))
                {
                    path = $"{EngineConstants.TEXTURES_FOLDER}/{name}{extension}";
                }
                else
                {
                    path = $"{EngineConstants.TEXTURES_FOLDER}/{name}";
                }

                using var stream = File.OpenRead(path);
                texture = new OTexture(FromStream(stream), pixelsPerUnit)
                {
                    Name = name
                };
                loadedTextures[name] = texture;
            }

            return texture;
        }

        static Texture2D FromStream(Stream stream)
        {
            var texture = Texture2D.FromStream(Engine.Instance.Graphics, stream);

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