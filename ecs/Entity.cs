using System;
using System.Collections.Generic;

namespace onwards.ecs
{
    public class Entity
    {
        public bool Loaded { get; private set; }
        
        public List<Component> Components { get; } = new List<Component>();

        public event Action<Component> OnAddComponent;
        public event Action<Component> OnRemoveComponent;

        public T Add<T>() where T : Component
        {
            var t = Activator.CreateInstance<T>();
            Add(t);
            return t;
        }

        public void Add(Component c)
        {
            Components.Add(c);
            c.Entity = this;

            if (Loaded)
            {
                c.Load();
            }

            OnAddComponent?.Invoke(c);
        }

        public void Remove(Component c)
        {
            Components.Remove(c);
            if (c.Entity == this)
            {
                c.Entity = null;
            }

            OnRemoveComponent?.Invoke(c);

            c.Destroy();
        }

        public Component Get(Type type)
        {
            foreach (var c in Components)
            {
                if (c.GetType() == type)
                {
                    return c;
                }
            }

            return null;
        }

        public T Get<T>()
        {
            foreach (var c in Components)
            {
                if (c is T t)
                {
                    return t;
                }
            }

            return default;
        }

        public virtual void Load()
        {
            foreach (var c in Components)
            {
                c.Load();
            }

            Loaded = true;
        }

        public virtual void Destroy()
        {
            var temp = new List<Component>(Components);
            foreach (var component in temp)
            {
                component.RemoveSelf();
            }
        }
    }
}