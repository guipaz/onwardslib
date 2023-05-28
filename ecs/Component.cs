namespace onwards.ecs
{
    public abstract class Component
    {
        public bool Active { get; set; } = true;
        public Entity Entity { get; set; }

        public virtual void Load()
        {
        }
        
        public virtual void Destroy()
        {
        }

        public virtual void RemoveSelf()
        {
            Entity.Remove(this);
        }
    }
}