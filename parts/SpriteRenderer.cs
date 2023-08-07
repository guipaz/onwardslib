using Microsoft.Xna.Framework;
using onwardslib.ecs;

namespace onwardslib.parts
{
    public class SpriteRenderer : Part, IRenderer
    {
        public Sprite Sprite { get; set; }
        public Vector2 Offset { get; set; }
        public bool FlipH { get; set; }
        public bool FlipV { get; set; }

        public int RenderOrder => (int)_transform.Position.Y; //TODO

        Transform _transform;

        public override void Load()
        {
            _transform = GetPart<Transform>();
        }

        public void Render()
        {
            ODraw.Draw(Sprite, _transform.Position, Offset);
        }
    }
}
