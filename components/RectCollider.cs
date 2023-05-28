using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using onwards.ecs;
using onwards.graphics;
using onwards.managers;

namespace onwards.components
{
    public class RectCollider : Component, ICollider, IRenderer
    {
        public const int BASE_UNIT_SIZE = 16;

        public CollisionManager CollisionManager { get; set; }
        public Vector2 Size { get; set; } = new Vector2(1);
        public Vector2 Pivot { get; set; } = new Vector2(.5f, .5f);
        public Vector2 Offset { get; set; }

        public Rectangle AbsoluteRectangle
        {
            get
            {
                if (!isDirty)
                {
                    return _absoluteRectangle;
                }

                isDirty = false;

                _absoluteRectangle.X = (int)(transform.Position.X - (int)(Size.X * BASE_UNIT_SIZE * Pivot.X) + Offset.X);
                _absoluteRectangle.Y = (int)(transform.Position.Y - (int)(Size.Y * BASE_UNIT_SIZE * Pivot.Y) + Offset.Y);
                _absoluteRectangle.Width = (int)(Size.X * BASE_UNIT_SIZE);
                _absoluteRectangle.Height = (int)(Size.Y * BASE_UNIT_SIZE);

                return _absoluteRectangle;
            }
        }

        public bool Passable { get; set; }

        Rectangle _absoluteRectangle;
        Transform transform;
        bool isDirty;
        
        public override void Load()
        {
            isDirty = true;

            transform = Entity.Get<Transform>();
            transform.OnChangePosition += SetDirty;
        }

        public override void Destroy()
        {
            transform.OnChangePosition -= SetDirty;
        }

        void SetDirty()
        {
            isDirty = true;
        }

        public Rectangle GetCollisionBounds()
        {
            return AbsoluteRectangle;
        }

        public bool Collides(Rectangle collider)
        {
            return collider.Intersects(GetCollisionBounds());
        }

        public Tag Tags { get; }
        public int Order => 9999;

        public void Render()
        {
            if (Input.Keyboard.Down(Keys.LeftShift))
            {
                Draw.DrawTexture(ColorTexture.Get(Color.Magenta), AbsoluteRectangle, Color.White * 0.5f);
                Draw.DrawTexture(ColorTexture.Get(Color.Blue), new Rectangle(transform.Position.ToPoint(), new Point(2, 2)), Color.White * 0.5f);
            }
        }

        public bool IsPassable()
        {
            return Passable;
        }

        //public bool Collides(Rectangle rectangle)
        //{
        //    return AbsoluteRectangle.Intersects(rectangle);
        //}

        //public bool CollisionValidator_IsValidPosition(ICollider collidable, Rectangle rect)
        //{
        //    return Entity == null || collidable == this || IsPassable() || !Engine.Physics.IsColliding(AbsoluteRectangle, rect);
        //}
    }
}