using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public interface IMaestro
    {
        IEnumerable<Texture2D> ToRender { get; }
        void Load();
        void Draw();
        void Update();
    }
}
