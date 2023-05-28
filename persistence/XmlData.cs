using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using onwards.utils;

namespace onwards.persistence
{
    public class XmlData
    {
        public string Name { get; }
        public string Text { get; set; }

        readonly Dictionary<string, string> _attributes = new Dictionary<string, string>();
        readonly List<XmlData> _children = new List<XmlData>();

        public string this[string i]
        {
            get => GetAttribute<string>(i);
            set => SetAttribute(i, value);
        }

        protected XmlData(XmlElement element)
        {
            Name = element.Name;

            foreach (XmlAttribute attr in element.Attributes)
            {
                SetAttribute(attr.Name, attr.Value);
            }
        }

        protected XmlData(string name)
        {
            Name = name;
        }

        public static XmlData Create(string name)
        {
            return new XmlData(name);
        }

        public XmlData Clone()
        {
            var newData = Create(Name);

            newData.Text = Text;
            foreach (var kvp in _attributes)
            {
                newData.SetAttribute(kvp.Key, kvp.Value);
            }

            foreach (var child in _children)
            {
                newData.AddChild(child.Clone());
            }

            return newData;
        }

        public static XmlData Create(XmlElement element)
        {
            var data = new XmlData(element);
            
            foreach (XmlElement child in element.ChildNodes.OfType<XmlElement>())
            {
                data.AddChild(Create(child));
            }

            if (element.ChildNodes.OfType<XmlText>().Any())
            {
                var textElement = element.ChildNodes.OfType<XmlText>().First();
                data.Text = textElement.Value;
            }
            
            return data;
        }

        public XmlElement ToXmlElement(XmlDocument doc)
        {
            var element = doc.CreateElement(Name);

            foreach (var attribute in _attributes)
            {
                element.SetAttribute(attribute.Key, attribute.Value);
            }
            
            foreach (var child in _children)
            {
                var childElement = child.ToXmlElement(doc);
                element.AppendChild(childElement);
            }

            if (Text != null)
            {
                //TODO this might collide with other children type
                var textElement = doc.CreateTextNode(Text);
                element.AppendChild(textElement);
            }

            return element;
        }
        
        public T GetAttribute<T>(string name, T defaultValue = default)
        {
            if (_attributes.ContainsKey(name))
            {
                return ParseValue<T>(_attributes[name]);
            }

            return defaultValue;
        }

        public void SetAttribute<T>(string name, T value)
        {
            if (value is Vector2 vec2)
            {
                _attributes[name] = VectorUtils.VectorToString(vec2);
            }
            else
            {
                //TODO float point/comma
                _attributes[name] = value.ToString();
            }
        }

        public XmlData GetChild(string name)
        {
            foreach (var child in _children)
            {
                if (child.Name == name)
                {
                    return child;
                }
            }

            return null;
        }

        public XmlData GetChild(Func<XmlData, bool> filter)
        {
            foreach (var child in _children)
            {
                if (filter?.Invoke(child) ?? false)
                {
                    return child;
                }
            }

            return null;
        }

        public IEnumerable<XmlData> GetChildren(Func<XmlData, bool> filter)
        {
            foreach (var child in _children)
            {
                if (filter?.Invoke(child) ?? false)
                {
                    yield return child;
                }
            }
        }

        public IEnumerable<XmlData> GetChildren()
        {
            foreach (var child in _children)
            {
                yield return child;
            }
        }

        public IEnumerable<(string Name, string Value)> GetAttributes()
        {
            foreach (var child in _attributes)
            {
                yield return (child.Key, child.Value);
            }
        }

        public void AddChild(XmlData data)
        {
            _children.Add(data);
        }
        
        T ParseValue<T>(string value)
        {
            var type = typeof(T);

            if (type == typeof(string))
            {
                return (T) (object) value;
            }
            if (type == typeof(int) && int.TryParse(value, out var intValue))
            {
                return (T) (object) intValue;
            }
            if (type == typeof(Vector2) && VectorUtils.TryParse(value, out var vecValue))
            {
                return (T)(object)vecValue;
            }
            if (type == typeof(Point) && PointUtils.TryParse(value, out var pointValue))
            {
                return (T)(object)pointValue;
            }
            if (type == typeof(bool) && bool.TryParse(value, out var boolValue))
            {
                return (T)(object)boolValue;
            }
            if (type == typeof(float) && float.TryParse(value, out var floatValue))
            {
                return (T)(object)floatValue;
            }
            
            throw new ArgumentException("Not a primitive type");
        }
    }
}