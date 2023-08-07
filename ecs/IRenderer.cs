namespace onwardslib.ecs
{
    public interface IRenderer
    {
        int RenderOrder { get; }
        void Render();
    }
}
