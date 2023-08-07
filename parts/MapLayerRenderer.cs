using onwardslib.ecs;
using onwardslib.maps;

namespace onwardslib.parts
{
    public class MapLayerRenderer : Part, IRenderer
    {
        public int RenderOrder => (MapLayer.OrderMagnitude == MapLayer.OrderMagnitudes.Background ? int.MinValue : (int.MaxValue - MapLayer.MAX_LAYERS)) + MapLayer.Order;
        public MapLayer MapLayer { get; set; }

        public override void Load()
        {

        }

        public void Render()
        {
            //TODO
        }
    }
}
