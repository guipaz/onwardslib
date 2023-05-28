using onwards.ecs;

namespace onwards.components
{
    public abstract class Renderer : Component, IRenderer
    {
        public virtual Tag Tags { get; } = Tag.All;
        public virtual int Order { get; set; }
        public abstract void Render();
    }
}