using Microsoft.Xna.Framework;

namespace onwards.utils
{
    public static class CalcUtils
    {
        public static Point IsoLocalToGlobal(int isoW, int isoH, int localX, int localY)
        {
            var globalX = (localX - localY) * isoW;
            var globalY = (localX + localY) * isoH;

            return new Point(globalX, globalY);
        }

        public static Point IsoGlobalToLocal(int isoW, int isoH, int globalX, int globalY)
        {
            var offsetX = globalX < 0 ? isoW : 0;

            var localX = ((globalX - offsetX) / isoW + globalY / isoH) / 2;
            var localY = (globalY / isoH - (globalX - offsetX) / isoW) / 2;

            return new Point(localX, localY);
        }
    }
}