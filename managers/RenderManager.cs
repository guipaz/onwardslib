using System.Collections.Generic;
using onwards.components;
using onwards.ecs;

namespace onwards.managers
{
    public class RenderManager : Manager
    {
        List<Camera> _cameras = new List<Camera>();
        List<IRenderer> _renderers = new List<IRenderer>();

        public override void AddComponent(Component component)
        {
            if (component is Camera camera)
            {
                _cameras.Add(camera);
            }
            else if (component is IRenderer renderer)
            {
                _renderers.Add(renderer);
            }
        }

        public override void RemoveComponent(Component component)
        {
            if (component is Camera camera)
            {
                _cameras.Remove(camera);
            }
            else if (component is IRenderer renderer)
            {
                _renderers.Remove(renderer);
            }
        }

        public override void Clear()
        {
            _cameras.Clear();
            _renderers.Clear();
        }

        public override void Draw()
        {
            _renderers.Sort((a, b) => a.Order.CompareTo(b.Order));
            _cameras.Sort((a, b) => a.Order.CompareTo(b.Order));

            foreach (var camera in _cameras)
            {
                if (Mask.Contains(camera.Tag))
                {
                    camera.Render(_renderers);
                }
            }
        }
    }
}