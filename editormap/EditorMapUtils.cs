using Microsoft.Xna.Framework;

namespace onwards.editormap
{
    public static class EditorMapUtils
    {
        static Point aux;
        public static Point WorldToTile(Vector2 world, int tileSize)
        {
            aux.X = (int)(world.X + tileSize / 2) / tileSize;
            aux.Y = (int)(world.Y + tileSize / 2) / tileSize;
            return aux;
        }
    }
}