using Microsoft.Xna.Framework;
using onwards.ecs;
using onwards.graphics;

namespace onwards.components
{
    public class ColorRenderer : Renderer
    {
        public override int Order => 999;

        public Color Color { get; set; }
        public float Opacity { get; set; } = 1f;
        public Rectangle Bounds { get; set; }

        public override void Render()
        {
            Draw.DrawTexture(ColorTexture.Get(Color), Bounds, Color.White * Opacity);
        }
    }
}