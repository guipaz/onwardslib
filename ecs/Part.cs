namespace onwardslib.ecs
{
    public abstract class Part : IDisposable
    {
        public Entity Entity { get; set; }

        public T GetPart<T>() where T : Part
        {
            return Entity.GetPart<T>();
        }
        public virtual void Load() { }
        public virtual void Dispose() { }
    }
}
