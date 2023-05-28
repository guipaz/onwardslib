using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace onwards
{
    public class AStar
    {
        public class Node : IEquatable<Node>
        {
            public Node Parent;
            public readonly Point Position;
            public int DistanceToTarget;
            public int Cost;
            public int Weight;
            public int F
            {
                get
                {
                    if (DistanceToTarget != -1 && Cost != -1)
                        return DistanceToTarget + Cost;
                    return -1;
                }
            }
            public bool Walkable;

            public Node(Point pos, bool walkable, int weight = 1)
            {
                Parent = null;
                Position = pos;
                DistanceToTarget = -1;
                Cost = 1;
                Weight = weight;
                Walkable = walkable;
            }
            
            public bool Equals(Node other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Position.Equals(other.Position);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Node) obj);
            }

            public override int GetHashCode()
            {
                return Position.GetHashCode();
            }
        }

        public interface IPathfindable
        {
            bool IsPassableAt(Point position);
            int GetWeightAt(Point position);
        }

        int width;
        int height;
        IPathfindable pathfindable;

        public AStar(int width, int height, IPathfindable pathfindable)
        {
            this.width = width;
            this.height = height;
            this.pathfindable = pathfindable;
        }

        public Stack<Node> FindPath(Point Start, Point End)
        {
            var start = new Node(new Point(Start.X, Start.Y), true);
            var end = new Node(new Point(End.X, End.Y), true);

            var Path = new Stack<Node>();
            var OpenList = new List<Node>();
            var ClosedList = new List<Node>();
            var current = start;
            List<Node> adjacencies;

            // add start node to Open List
            OpenList.Add(start);

            while (OpenList.Count != 0 && !ClosedList.Exists(x => x.Position == end.Position))
            {
                current = OpenList[0];
                OpenList.Remove(current);
                ClosedList.Add(current);
                adjacencies = GetAdjacentNodes(current);

                foreach (var n in adjacencies)
                {
                    if (!ClosedList.Contains(n) && n.Walkable)
                    {
                        if (!OpenList.Contains(n))
                        {
                            n.Parent = current;
                            n.DistanceToTarget = Math.Abs(n.Position.X - end.Position.X) + Math.Abs(n.Position.Y - end.Position.Y);
                            n.Cost = n.Weight + n.Parent.Cost;
                            OpenList.Add(n);
                            OpenList = OpenList.OrderBy(node => node.F).ToList<Node>();
                        }
                    }
                }
            }

            // construct path, if end was not closed return null
            if (!ClosedList.Exists(x => x.Position == end.Position))
            {
                return null;
            }

            // if all good, return path
            var temp = ClosedList[ClosedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                Path.Push(temp);
                temp = temp.Parent;
            } while (temp != start && temp != null);
            return Path;
        }

        List<Node> GetAdjacentNodes(Node n)
        {
            var temp = new List<Node>();
            
            var x = n.Position.X;
            var y = n.Position.Y;

            TryAddNode(temp, x, y + 1);
            TryAddNode(temp, x, y - 1);
            TryAddNode(temp, x + 1, y);
            TryAddNode(temp, x - 1, y);

            return temp;
        }

        void TryAddNode(List<Node> temp, int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                var pos = new Point(x, y);
                var passable = pathfindable.IsPassableAt(pos);
                var weight = pathfindable.GetWeightAt(pos);
                temp.Add(new Node(pos, passable, weight));
            }
        }
    }
}