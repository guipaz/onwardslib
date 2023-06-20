using Microsoft.Xna.Framework;

namespace onwardslib.parts
{
    public class Collidable : IPart
    {
        public bool Collides { get; set; } = true;
        public Point Size { get; set; }
        public Point Offset { get; set; }
    }
}
