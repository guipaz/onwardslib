using System;
using Microsoft.Xna.Framework;

namespace onwards.utils
{
    public static class PointUtils
    {
        public static string PointToString(Point point)
        {
            return $"{point.X};{point.Y}";
        }

        public static Point Parse(string str)
        {
            var div = str.IndexOf(';');
            return new Point(int.Parse(str.Substring(0, div)),
                int.Parse(str.Substring(div + 1, str.Length - div - 1)));
        }

        public static bool TryParse(string str, out Point point)
        {
            try
            {
                point = Parse(str);
            }
            catch (Exception)
            {
                point = Point.Zero;

                return false;
            }

            return true;
        }
    }
}