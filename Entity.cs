using onwardslib.parts;

namespace onwardslib
{
    public abstract class Entity
    {
        List<IPart> _parts = new List<IPart>();

        public Transform Transform { get; }
        public IEnumerable<IPart> Parts { get => _parts; }

        public Entity()
        {
            Transform = AddPart<Transform>();
        }

        public T AddPart<T>() where T : IPart
        {
            var part = Activator.CreateInstance<T>();
            AddPart(part);
            return part;
        }

        public void AddPart(IPart part)
        {
            _parts.Add(part);
        }

        public void RemovePart(IPart part)
        {
            _parts.Remove(part);
        }

        public T GetPart<T>() where T : IPart
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
