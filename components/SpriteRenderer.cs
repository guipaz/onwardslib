using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using onwards.ecs;
using onwards.graphics;

namespace onwards.components
{
    public class SpriteRenderer : Renderer
    {
        public Sprite Sprite { get; set; }
        public Vector2 Pivot { get; set; } = new Vector2(.5f, .5f);
        public bool FlipHorizontally { get; set; }
        public float Opacity { get; set; } = 1f;

        Transform transform;

        public override void Load()
        {
            transform = Entity.Get<Transform>();
        }

        public override int Order => (int) transform.Position.Y;

        public override void Render()
        {
            if (Sprite != null)
            {                            
                Draw.DrawSprite(Sprite, transform.Position - Sprite.SourceRectangle.Size.ToVector2() * Pivot,
                    Color.White * Opacity, 0, FlipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            }
        }
    }
}