using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace XmlPatcher
{
    public static class Class1
    {
        static int GetXmlNodeLevel(this XmlNode xmlNode)
        {
            int currentLevel = 0;
            while (xmlNode != null) {
                xmlNode = xmlNode.ParentNode;
                currentLevel++;
            }
            return currentLevel;
        }

        public static XmlWhitespace CreateXmlWhitespace(XmlNode xmlNode)
        {
            var result = xmlNode.ChildNodes.OfType<XmlWhitespace>()
                    .OrderBy( x => x.Length ).FirstOrDefault();
            if (result != null)
                return (XmlWhitespace)result.Clone();

            int length = xmlNode.GetXmlNodeLevel() * 2;
            return xmlNode.OwnerDocument.CreateWhitespace("".PadLeft(length));
        }

        public static IEnumerable<XmlNode> GetNodes(this XmlNode xmlNode, IEnumerable<string> parts)
        {
            var nodes = new XmlNode[] { xmlNode };
            foreach (var part in parts)
            {
                nodes = nodes.SelectMany(x => x.ChildNodes.OfType<XmlElement>()
                   .Where(i => i.LocalName == part)).OfType<XmlElement>().ToArray();
            }
            return nodes;
        }

        public static IEnumerable<XmlNode> GetNodes(this XmlNode xmlNode, params string[] parts)
        {
            return xmlNode.GetNodes(parts);
        }

        public static XmlElement EnsureXmlElement(this XmlElement xmlNode, string propName, string propValue)
        {
            var result = (XmlElement)xmlNode.GetNodes(propName).FirstOrDefault();
            if (result == null) {
                result = xmlNode.OwnerDocument.CreateElement(propName);
                XmlWhitespace xmlSpace = CreateXmlWhitespace(xmlNode);
                if (xmlNode.LastChild.NodeType == XmlNodeType.Whitespace) {
                    xmlNode.InsertBefore(xmlSpace, xmlNode.LastChild);
                    xmlNode.InsertBefore(result, xmlNode.LastChild);
                }
                else {
                    xmlNode.AppendChild(xmlSpace);
                    xmlNode.AppendChild(result);
                }
            }
            result.InnerText = propValue;
            return result;
        }
    }
}
