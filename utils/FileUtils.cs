using System.Collections.Generic;
using System.IO;
using System.Xml;
using onwards.persistence;

namespace onwards.utils
{
    public class FileUtils
    {
        public static IEnumerable<string> GetFileNames(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            var files = directoryInfo.GetFiles();

            foreach (var f in files)
            {
                yield return f.Name;
            }
        }

        public static XmlData LoadXml(string path)
        {
            var xmlDocument = new XmlDocument();

            if (!path.EndsWith(".xml"))
            {
                path += ".xml";
            }

            var text = File.ReadAllText(path);
            xmlDocument.LoadXml(text);
            return XmlData.Create(xmlDocument.FirstChild as XmlElement);
        }
    }
}