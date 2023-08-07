namespace onwardslib.ecs
{
    public abstract class Entity
    {
        List<Part> _parts = new List<Part>();

        public IEnumerable<Part> Parts { get => _parts; }

        public T AddPart<T>() where T : Part
        {
            var part = Activator.CreateInstance<T>();
            AddPart(part);
            return part;
        }

        public void AddPart(Part part)
        {
            _parts.Add(part);
        }

        public void RemovePart(Part part)
        {
            _parts.Remove(part);
        }

        public T GetPart<T>() where T : Part
        {
            foreach (var part in _parts)
            {
                if (part is T)
                {
                    return (T)part;
                }
            }

            return default;
        }
    }
}
