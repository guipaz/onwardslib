using System;
using Microsoft.Xna.Framework;
using onwards.ecs;

namespace onwards.components
{
    public class Transform : Component
    {
        bool isDirty;

        Vector2 position;
        public Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                isDirty = true;
                OnChangePosition?.Invoke();
            }
        }

        Matrix _matrix;
        public Matrix Matrix
        {
            get
            {
                if (isDirty)
                {
                    _matrix = Matrix.CreateTranslation(position.X, position.Y, 0);
                }

                return _matrix;
            }
        }

        public event Action OnChangePosition;

        public Rectangle ApplyMatrix(Rectangle input)
        {
            var startPos = ApplyMatrix(input.Location.ToVector2());
            var finalPos = ApplyMatrix(input.Location.ToVector2() + input.Size.ToVector2());

            return new Rectangle((int)startPos.X, (int)startPos.Y, (int)(finalPos.X - startPos.X), (int)(finalPos.Y - startPos.Y));
        }

        public Vector2 ApplyMatrix(Vector2 input)
        {
            return Vector2.Transform(input, Matrix);
        }
    }
}