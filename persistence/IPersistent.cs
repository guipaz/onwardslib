using System.Xml;

namespace onwards.persistence
{
    public interface IPersistent
    {
        void LoadData(XmlData data);
        XmlData SaveData();
    }
}