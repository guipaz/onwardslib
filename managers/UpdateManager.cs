using System.Collections.Generic;
using onwards.components;
using onwards.ecs;

namespace onwards.managers
{
    public class UpdateManager : Manager
    {
        List<IUpdater> _updaters = new List<IUpdater>();

        public override void AddComponent(Component component)
        {
            if (component is IUpdater updater)
            {
                _updaters.Add(updater);
            }
        }

        public override void RemoveComponent(Component component)
        {
            if (component is IUpdater updater)
            {
                _updaters.Remove(updater);
            }
        }

        public override void Clear()
        {
            _updaters.Clear();
        }

        public override void Update()
        {
            foreach (var updater in _updaters)
            {
                updater.Update();
            }
        }
    }
}