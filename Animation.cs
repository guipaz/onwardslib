namespace onwardslib
{
    public class Animation
    {
        public string Id { get; set; }
        public IEnumerable<Sprite> Frames => _frames;

        List<Sprite> _frames = new();
    }
}
