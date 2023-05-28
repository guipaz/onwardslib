using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using onwards.components;
using onwards.ecs;

namespace onwards.managers
{
    public class CollisionManager : Manager
    {
        List<ICollider> _colliders = new List<ICollider>();
        
        public override void AddComponent(Component component)
        {
            if (component is ICollider collider)
            {
                _colliders.Add(collider);
                collider.CollisionManager = this;
            }
        }

        public override void RemoveComponent(Component component)
        {
            if (component is ICollider collider)
            {
                _colliders.Remove(collider);
                if (collider.CollisionManager == this)
                {
                    collider.CollisionManager = null;
                }
            }
        }

        public IEnumerable<ICollider> GetCollisions(ICollider obj, Rectangle rect)
        {
            return GetCollisions(obj, rect, _colliders);
        }

        public IEnumerable<ICollider> GetCollisions(ICollider obj, Rectangle rect, IEnumerable<ICollider> colliders)
        {
            if (colliders == null)
                yield break;

            foreach (var validator in colliders)
            {
                if (validator != null &&
                    validator != obj &&
                    !validator.IsPassable() &&
                    validator.Collides(rect))
                {
                    yield return validator;
                }
            }
        }

        public IEnumerable<ICollider> GetCollisions(ICollider obj)
        {
            return GetCollisions(obj, obj.GetCollisionBounds());
        }

        Rectangle auxA;
        Rectangle auxC;
        public void SimulateMovement(ICollider obj, ref float finalMovX, ref float finalMovY)
        {
            if (obj.IsPassable())
                return;

            var collisionBounds = obj.GetCollisionBounds();
            var position = collisionBounds.Location;
            var size = collisionBounds.Size;

            auxA.X = (int)Math.Round(position.X + finalMovX, 0, MidpointRounding.ToEven);
            auxA.Y = (int)Math.Round(position.Y + finalMovY, 0, MidpointRounding.ToEven);
            auxA.Width = size.X;
            auxA.Height = size.Y;

            // not valid
            var invalids = GetCollisions(obj, auxA, _colliders);
            if (invalids.Any())
            {
                // tries removing X
                auxC = auxA;
                auxC.X = position.X;
                var nInvalids = GetCollisions(obj, auxC, invalids);

                if (nInvalids.Any())
                {
                    // tries removing Y
                    auxC = auxA;
                    auxC.Y = position.Y;
                    nInvalids = GetCollisions(obj, auxC, invalids);
                    if (nInvalids.Any())
                    {
                        // nothing works, disable both
                        finalMovX = 0;
                        finalMovY = 0;
                    }
                    else
                    {
                        finalMovY = 0;
                    }
                }
                else
                {
                    finalMovX = 0;
                }
            }
        }

        public override void Clear()
        {
            _colliders.Clear();
        }
    }
}