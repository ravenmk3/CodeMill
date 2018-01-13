using System;
using System.Xml;
using CodeMill.Core.Common;
using CodeMill.Core.Common.Xml;
using CodeMill.Core.Model;

namespace CodeMill.Core.Xml
{
    public static class MapWriter
    {
        public static void Write(INamedMap map, string filename)
        {
            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }
            var xml = new XmlDocument();
            var root = xml.CreateElement("Map");
            root.SetAttribute("Name", map.Name);
            xml.AppendChild(root);
            foreach (var entry in map)
            {
                var node = xml.CreateElement("Item");
                root.AppendChild(node);
                node.SetAttribute("Key", entry.Key);
                node.InnerText = entry.Value;
            }
            filename = PathUtility.ResolvePath(filename);
            xml.SaveOptimized(filename);
        }
    }
}
