using Microsoft.Xna.Framework;
using onwardslib;

namespace harvester.parts
{
    public class Renderable : IPart
    {
        public Sprite Sprite { get; set; }
        public Vector2 Offset { get; set; }
    }
}
