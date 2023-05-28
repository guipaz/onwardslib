using System.Collections.Generic;

namespace onwards
{
    public static class EngineConstants
    {
#if DEBUG
        public static readonly string REPO = "../../../assets";
        public static readonly string TEXTURES_FOLDER = $"{REPO}/textures/";
        public static readonly string FONTS_FOLDER = $"{REPO}/fonts/";
        public static readonly string MAPS_FOLDER = $"{REPO}/maps/";
        public static readonly string DATA_FOLDER = $"{REPO}/data/";
#else
        public const string TEXTURES_FOLDER = "assets/textures/";
        public const string FONTS_FOLDER = "assets/fonts/";
        public const string MAPS_FOLDER = "assets/data/";
#endif
    }
}