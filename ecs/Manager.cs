using System;
using System.Collections;
using System.Collections.Generic;

namespace onwards.ecs
{
    public abstract class Manager
    {
        public Tag Mask { get; } = Tag.All;
        public virtual int Order { get; }
        public virtual void AddComponent(Component component) { }
        public virtual void RemoveComponent(Component component) { }
        public virtual void Clear() { }
        public virtual void Update() { }
        public virtual void Draw() { }
    }
}