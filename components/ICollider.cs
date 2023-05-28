using Microsoft.Xna.Framework;
using onwards.managers;

namespace onwards.components
{
    public interface ICollider
    {
        CollisionManager CollisionManager { get; set; }
        Rectangle GetCollisionBounds();
        bool Collides(Rectangle collider);
        bool IsPassable();
    }
}