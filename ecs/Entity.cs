namespace onwardslib.ecs
{
    public abstract class Entity
    {
        public IEnumerable<Part> Parts { get => _parts; }

        List<Part> _parts = new List<Part>();
        List<Part> _toAdd = new List<Part>();
        List<Part> _toRemove = new List<Part>();

        public T AddPart<T>(bool immediate = false) where T : Part
        {
            var part = Activator.CreateInstance<T>();
            AddPart(part, immediate);
            return part;
        }

        public void AddPart(Part part, bool immediate = false)
        {
            if (immediate)
            {
                _parts.Add(part);
            }
            else
            {
                _toAdd.Add(part);
            }
        }

        public void RemovePart(Part part, bool immediate = false)
        {
            if (immediate)
            {
                _parts.Remove(part);
            }
            else
            {
                _toRemove.Add(part);
            }
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
