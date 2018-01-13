using System;
using System.Xml;
using CodeMill.Core.Common;
using CodeMill.Core.Common.Xml;
using CodeMill.Core.Model;

namespace CodeMill.Core.Xml
{
    public static class MapReader
    {
        public static INamedMap Read(string filename)
        {
            filename = PathUtility.ResolvePath(filename);
            var xml = new XmlDocument();
            xml.Load(filename);
            var name = xml.DocumentElement.GetAttribute("Name");
            var data = new NamedMap(name);
            var nodes = xml.SelectNodes("/Map/Item");
            foreach (XmlNode node in nodes)
            {
                data.Add(node.GetAttributeValue("Key"), node.InnerText);
            }
            return data;
        }
    }
}
