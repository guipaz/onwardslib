using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using onwardslib.utils;
using static onwardslib.GridSprite;

namespace onwardslib
{
    public class GridSprite
    {
        [Flags]
        public enum Context
        {
            None = 0,
            N = 1,
            E = 2,
            S = 4,
            W = 8,
            SW = 16,
            SE = 32,
            NE = 64,
            NW = 128,
            All = N | E | S | W | SW | SE | NE | NW,
        }

        public Texture2D Texture { get; set; }

        Dictionary<Context, Sprite> _sources = new();
        List<Context> _contextsByBits = new List<Context>();

        public GridSprite(Texture2D gridSpriteTexture)
        {
            Texture = gridSpriteTexture;

            var a = Utils.CountBits((uint)Context.W);

            _sources[Context.All] = Sprite.Get(Texture, new Rectangle(0, 16, 16, 16));
            _sources[Context.None] = Sprite.Get(Texture, new Rectangle(16, 16, 16, 16));
            _sources[Context.S] = Sprite.Get(Texture, new Rectangle(32, 0, 16, 16));

            _sources[Context.S | Context.N] = Sprite.Get(Texture, new Rectangle(32, 16, 16, 16));
            _sources[Context.S | Context.E] = Sprite.Get(Texture, new Rectangle(48, 0, 16, 16));

            _sources[Context.E | Context.S | Context.W] = Sprite.Get(Texture, new Rectangle(80, 0, 16, 16));
            _sources[Context.E | Context.S | Context.SE] = Sprite.Get(Texture, new Rectangle(0, 0, 16, 16));

            _sources[Context.E | Context.S | Context.W | Context.N] = Sprite.Get(Texture, new Rectangle(96, 0, 16, 16));
            _sources[Context.W | Context.E | Context.SW | Context.S] = Sprite.Get(Texture, new Rectangle(64, 0, 16, 16));
            _sources[Context.W | Context.E | Context.SE | Context.S] = Sprite.Get(Texture, new Rectangle(64, 0, 16, 16), true);

            _sources[Context.E | Context.S | Context.W | Context.N | Context.NW] = Sprite.Get(Texture, new Rectangle(80, 16, 16, 16));
            _sources[Context.W | Context.E | Context.SW | Context.S | Context.SE] = Sprite.Get(Texture, new Rectangle(16, 0, 16, 16));
            
            _sources[Context.NW | Context.N | Context.W | Context.E | Context.SW | Context.S] = Sprite.Get(Texture, new Rectangle(64, 16, 16, 16));
            _sources[Context.NW | Context.N | Context.W | Context.E | Context.SE | Context.S] = Sprite.Get(Texture, new Rectangle(96, 16, 16, 16));

            _sources[Context.NW | Context.N | Context.NE |Context.W | Context.E | Context.SW | Context.S] = Sprite.Get(Texture, new Rectangle(48, 16, 16, 16));

            foreach (var k in _sources.Keys)
            {
                _contextsByBits.Add(k);
            }

            _contextsByBits.Sort((b, a) => Utils.CountBits((uint)a).CompareTo(Utils.CountBits((uint)b)));
        }

        public static Context PointToContext(Point p)
        {
            if (p.X == -1 && p.Y == -1)
                return Context.NW;
            if (p.X == 0 && p.Y == -1)
                return Context.N;
            if (p.X == 1 && p.Y == -1)
                return Context.NE;
            if (p.X == -1 && p.Y == 0)
                return Context.W;
            if (p.X == 1 && p.Y == 0)
                return Context.E;
            if (p.X == -1 && p.Y == 1)
                return Context.SW;
            if (p.X == 0 && p.Y == 1)
                return Context.S;
            if (p.X == 1 && p.Y == 1)
                return Context.SE;
            return Context.None;
        }

        Context RotateContext(Context context)
        {
            var newContext = Context.None;

            if ((context & Context.N) == Context.N) newContext |= Context.E;
            if ((context & Context.E) == Context.E) newContext |= Context.S;
            if ((context & Context.S) == Context.S) newContext |= Context.W;
            if ((context & Context.W) == Context.W) newContext |= Context.N;

            if ((context & Context.NW) == Context.NW) newContext |= Context.NE;
            if ((context & Context.NE) == Context.NE) newContext |= Context.SE;
            if ((context & Context.SE) == Context.SE) newContext |= Context.SW;
            if ((context & Context.SW) == Context.SW) newContext |= Context.NW;

            return newContext;
        }

        public (Sprite sprite, int rotations) GetByContext(Context context)
        {
            if (!_sources.ContainsKey(context))
            {
                var tuple = Find(context);
                return tuple.sprite == null ? (_sources[Context.None], 0) : tuple;
            }

            return (_sources[context], 0);
        }

        (Sprite sprite, int rotations) Find(Context context)
        {
            var tuple = Rotate(context);

            if (tuple.sprite == null)
            {
                foreach (var bContext in _contextsByBits)
                {
                    tuple = RotateCompare(context, bContext);
                    if (tuple.sprite != null)
                    {
                        break;
                    }
                }
            }

            return tuple;
        }

        (Sprite sprite, int rotations) Rotate(Context context)
        {
            for (var i = 0; i < 3; i++)
            {
                context = RotateContext(context);
                if (_sources.ContainsKey(context))
                {
                    return (_sources[context], i + 1);
                }
            }

            return (null, 0);
        }

        (Sprite sprite, int rotations) RotateCompare(Context contextA, Context contextB)
        {
            if ((contextA & contextB) == contextB)
            {
                return (_sources[contextB], 0);
            }

            for (var i = 0; i < 3; i++)
            {
                contextA = RotateContext(contextA);
                if ((contextA & contextB) == contextB)
                {
                    return (_sources[contextB], i + 1);
                }
            }

            return (null, 0);
        }
    }
}
