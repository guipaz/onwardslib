using System;
using System.Collections.Generic;
using System.Linq;

namespace onwards
{
    public class ProcMaster
    {
        Random random = new Random();

        public static ProcMaster Debug = new ProcMaster();

        public void SetSeed(int seed)
        {
            random = new Random(seed);
        }

        public int Between(int a, int b)
        {
            return random.Next(a, b);
        }

        public int Roll(int n)
        {
            return random.Next(1, n + 1);
        }

        public T OneOf<T>(IEnumerable<T> a)
        {
            if (!a.Any())
                return default;
            return a.ElementAt(random.Next(0, a.Count()));
        }

        public IEnumerable<T> WeightedPick<T>(Dictionary<T, int> map, int pick = 1)
        {
            var keys = map.Keys;
            var from = new List<T>();
            foreach (var k in keys)
            {
                var weight = map[k];
                for (int i = 0; i < weight; i++)
                {
                    from.Add(k);
                }
            }

            for (int i = 0; i < pick; i++)
            {
                var selected = Roll(from.Count);
                yield return from[selected];
            }
        }
    }
}