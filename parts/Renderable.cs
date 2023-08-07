using Microsoft.Xna.Framework;
using onwardslib;
using onwardslib.ecs;

namespace harvester.parts
{
    public class Renderable : Part
    {
        public Sprite Sprite { get; set; }
        public Vector2 Offset { get; set; }
        public bool FlipH { get; set; }
        public bool FlipV { get; set; }
        public bool Rotated { get; set; }
        public float Opacity { get; set; } = 1f;
    }
}
