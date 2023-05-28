namespace onwards.components
{
    public interface IRenderer
    {
        Tag Tags { get; }
        int Order { get; }

        void Render();
    }
}