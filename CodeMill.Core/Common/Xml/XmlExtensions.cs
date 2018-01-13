using System;
using System.Linq;
using System.Text;
using System.Xml;

namespace CodeMill.Core.Common.Xml
{
    internal static class XmlExtensions
    {
        #region XmlDocument Extensions

        public static void SaveOptimized(this XmlDocument document, string filename)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            var setting = new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                CloseOutput = true,
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "    ",
            };
            using (var writer = XmlWriter.Create(filename, setting))
            {
                document.Save(writer);
            }
        }

        #endregion XmlDocument Extensions

        #region XmlNode Extensions

        public static string GetAttributeValue(this XmlNode node, string name)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            var attr = node.Attributes[name];
            if (attr != null)
            {
                return attr.Value;
            }
            return null;
        }

        public static XmlNode FetchAttributeValue(this XmlNode node, string name, Action<string> callback)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            var attr = node.Attributes[name];
            if (attr != null)
            {
                callback.Invoke(attr.Value);
            }
            return node;
        }

        #endregion XmlNode Extensions

        #region XmlNodeList Extensions

        public static TResult[] ToArray<TResult>(this XmlNodeList list, Func<XmlNode, TResult> convertor)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            return list.Cast<XmlNode>().Select(convertor).ToArray();
        }

        #endregion XmlNodeList Extensions
    }
}
