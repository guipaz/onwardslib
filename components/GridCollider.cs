using Microsoft.Xna.Framework;
using onwards.ecs;
using onwards.managers;

namespace onwards.components
{
    public class GridCollider : Component, ICollider
    {
        public CollisionManager CollisionManager { get; set; }
        public bool[,] Grid { get; set; }

        public Rectangle GetCollisionBounds()
        {
            return Rectangle.Empty;
        }

        public bool IsInside(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Grid.GetLength(0) && y < Grid.GetLength(1);
        }

        public bool Collides(Rectangle rect)
        {
            var pX = rect.X;
            var pY = rect.Y;

            if (pX < 0 || pY < 0)
                return true;

            var min = new Point(pX / Engine.Instance.TileSize, pY / Engine.Instance.TileSize);
            var max = new Point((rect.X + rect.Width) / Engine.Instance.TileSize, (rect.Y + rect.Height) / Engine.Instance.TileSize);

            for (var y = min.Y; y <= max.Y; y++)
            {
                for (var x = min.X; x <= max.X; x++)
                {
                    if (!IsInside(x, y))
                    {
                        return true;
                    }

                    if (Grid[x, y])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsPassable()
        {
            return false;
        }
    }
}