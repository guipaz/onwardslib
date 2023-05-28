using System.IO;
using onwards.monobmfont;

namespace onwards.utils
{
    public static class FontLoader
    {
        public static BMFont Load(string fontName)
        {
            var fontTextureName = FontData.GetTextureNameForFont(fontName);
            var fontTexture = TextureLoader.Get("../fonts/" + fontTextureName).Texture2D;

            var path = $"{EngineConstants.FONTS_FOLDER}/{fontName}.fnt";
            using var stream = File.OpenRead(path);
            var fontDesc = FontData.Load(stream);
            var bmFont = new BMFont(fontTexture, fontDesc);

            return bmFont;
        }
    }
}