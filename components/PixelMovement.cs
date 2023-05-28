using System;
using Microsoft.Xna.Framework;
using onwards.ecs;

namespace onwards.components
{
    public enum Direction
    {
        Down = 0, Left, Up, Right
    }

    public class PixelMovement : Component, IUpdater
    {
        const float THRESHOLD = 0.1f;

        public Direction Direction { get; set; }
        public bool IsMoving { get; private set; }

        bool movedLastFrame;

        float pixelsPerSecond = 54f; // speed
        float diagonalPixelsPerSecond = 40f; // speed

        Point movementDirection;
        Vector2 vec;

        RectCollider _rectCollider;
        Transform _transform;
        
        bool idled;
        
        public override void Load()
        {
            _rectCollider = Entity.Get<RectCollider>();
            _transform = Entity.Get<Transform>();
        }

        public void MoveTowards(Point movementDirection)
        {
            movedLastFrame = true;
            this.movementDirection = movementDirection;
        }

        public void Update()
        {
            IsMoving = false;

            if (movedLastFrame)
            {
                IsMoving = true;

                movedLastFrame = false;
                idled = false;
                
                // moved, calculates how much since last frame based on the speed and direction
                var speed = vec.X != 0 && vec.Y != 0 ? diagonalPixelsPerSecond : pixelsPerSecond;
                vec.X = movementDirection.X * speed * Engine.Instance.DeltaTime;
                vec.Y = movementDirection.Y * speed * Engine.Instance.DeltaTime;

                if (vec.X < 0)
                {
                    Direction = Direction.Left;
                }
                else if (vec.X > 0)
                {
                    Direction = Direction.Right;
                }
                else if (vec.Y > 0)
                {
                    Direction = Direction.Down;
                }
                else if (vec.Y < 0)
                {
                    Direction = Direction.Up;
                }

                // moves the entity if there's movement to be done
                if (Math.Abs(vec.X) > THRESHOLD || Math.Abs(vec.Y) > THRESHOLD)
                {
                    _rectCollider.CollisionManager.SimulateMovement(_rectCollider, ref vec.X, ref vec.Y);

                    if (vec != Vector2.Zero)
                    {
                        _transform.Position += vec;
                    }
                }
            }
            else if (!idled)
            {
                idled = true;
            }
        }        
    }
}