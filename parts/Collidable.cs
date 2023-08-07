using Microsoft.Xna.Framework;
using onwardslib.ecs;

namespace onwardslib.parts
{
    public class Collidable : Part
    {
        public bool Collides { get; set; } = true;
        public Point Size { get; set; }
        public Vector2 Offset { get; set; }

        public Rectangle GetBoundsAt(Vector2 position)
        {
            return new Rectangle((int)(position.X + Offset.X),
                                 (int)(position.Y + Offset.Y), 
                                 Size.X, Size.Y);
        }

        public bool CollidesWith(Vector2 thisPosition, Collidable otherCollidable, Vector2 otherPosition)
        {
            return CollidesWith(thisPosition, otherCollidable.GetBoundsAt(otherPosition));
        }

        public bool CollidesWith(Vector2 thisPosition, Rectangle bounds)
        {
            return GetBoundsAt(thisPosition).Intersects(bounds);
        }
    }
}
