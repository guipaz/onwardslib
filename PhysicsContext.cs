using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using onwards.components;

namespace onwards
{
    public interface ICollisionValidator
    {
        bool CollisionValidator_IsValidPosition(ICollider collider, Rectangle rect);
    }

    public class PhysicsContext
    {
        public List<ICollisionValidator> validators;

        Rectangle auxA;
        Rectangle auxC;

        public PhysicsContext()
        {
            validators = new List<ICollisionValidator>();
        }

        public bool IsColliding(Rectangle a, Rectangle b)
        {
            return a.Intersects(b);
        }

        public IEnumerable<ICollisionValidator> GetCollisions(ICollider obj, Rectangle rect)
        {
            return GetCollisions(obj, rect, validators);
        }

        public IEnumerable<ICollisionValidator> GetCollisions(ICollider obj, Rectangle rect, IEnumerable<ICollisionValidator> currentValidators)
        {
            if (currentValidators == null)
                yield break;

            foreach (var validator in currentValidators)
            {
                if (validator != null && !validator.CollisionValidator_IsValidPosition(obj, rect))
                {
                    yield return validator;
                }
            }
        }

        public IEnumerable<ICollisionValidator> GetCollisions(ICollider obj)
        {
            return GetCollisions(obj, obj.GetCollisionBounds());
        }

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
            var invalids = GetCollisions(obj, auxA, validators);
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

        public void Clear()
        {
            validators.Clear();
        }

        public void AddToContext(ICollisionValidator validator)
        {
            validators.Add(validator);
        }

        public void RemoveFromContext(ICollisionValidator validator)
        {
            validators.Remove(validator);
        }
    }
}