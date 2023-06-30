using monobmfont;

namespace onwardslib
{
    public static class FontLoader
    {
        public static BMFont Load(string fontName)
        {
            var fontTextureName = FontData.GetTextureNameForFont(fontName);
            var fontTexture = TextureLoader.Get("../fonts/" + fontTextureName);

            var path = $"{Engine.DataFolder}/fonts/{fontName}.fnt";
            using var stream = File.OpenRead(path);
            var fontDesc = FontData.Load(stream);
            
            return new BMFont(fontTexture, fontDesc);
        }
    }
}