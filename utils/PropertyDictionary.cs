using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace onwards.utils
{
    public class PropertyDictionary
    {
        Dictionary<string, string> _data = new Dictionary<string, string>();

        public PropertyDictionary(XmlElement node)
        {
            foreach (XmlElement child in node.ChildNodes)
            {
                _data[child.GetAttribute("name")] = child.GetAttribute("value");
            }
        }

        public string Get(string key, string defaultValue = null)
        {
            return Get<string>(key);
        }

        public T Get<T>(string key, T defaultValue = default)
        {
            if (!_data.ContainsKey(key))
            {
                return defaultValue;
            }

            return Convert<T>(_data[key]);
        }

        T Convert<T>(string value)
        {
            if (typeof(T) == typeof(float))
            {
                return (T) (object) float.Parse(value, CultureInfo.InvariantCulture);
            }
            if (typeof(T) == typeof(int))
            {
                return (T)(object)int.Parse(value, CultureInfo.InvariantCulture);
            }
            if (typeof(T) == typeof(bool))
            {
                return (T)(object)bool.Parse(value);
            }

            return (T) (object) value;
        }
    }
}