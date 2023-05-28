using onwards.graphics;
using onwards.persistence;
using onwards.utils;

namespace onwards.editormap
{
    public class EditorTileset : IPersistent
    {
        public int InitialGid { get; set; }
        public OTexture Texture { get; set; }
        public bool Animated { get; set; }
        
        public void LoadData(XmlData data)
        {
            InitialGid = data.GetAttribute("initialGid", InitialGid);
            Texture = TextureLoader.Get(data.GetAttribute<string>("name"));
            Animated = data.GetAttribute("animated", Animated);
        }

        public XmlData SaveData()
        {
            var data = XmlData.Create("tileset");
            data["name"] = Texture.Name;
            data["initialGid"] = InitialGid.ToString();
            data["animated"] = Animated.ToString();
            return data;
        }
    }
}