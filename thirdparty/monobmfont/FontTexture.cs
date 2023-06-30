using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace monobmfont {
    [DataContract]
    public class FontTexture {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("file")]
        public string File { get; set; }
    }
}