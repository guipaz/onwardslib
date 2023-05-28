using System;
using Microsoft.Xna.Framework;

namespace onwards.utils
{
    public static class VectorUtils
    {
        public static string VectorToString(Vector2 vec)
        {
            return $"{vec.X};{vec.Y}";
        }

        public static Vector2 Parse(string str)
        {
            var div = str.IndexOf(';');
            return new Vector2(float.Parse(str.Substring(0, div)),
                float.Parse(str.Substring(div + 1, str.Length - div - 1)));
        }

        public static bool TryParse(string str, out Vector2 vec2)
        {
            try
            {
                vec2 = Parse(str);
            }
            catch (Exception)
            {
                vec2 = Vector2.Zero;

                return false;
            }

            return true;
        }
    }
}