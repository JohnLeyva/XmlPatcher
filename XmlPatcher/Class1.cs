using System;
using System.Linq;
using System.Xml;

namespace XmlPatcher
{
    public class Class1
    {
        int GetXmlNodeLevel(XmlNode xmlNode)
        {
            int currentLevel = 0;
            while (xmlNode != null) {
                xmlNode = xmlNode.ParentNode;
                currentLevel++;
            }
            return currentLevel;
        }

        public XmlWhitespace CreateXmlWhitespace(XmlNode xmlNode)
        {
            var result = xmlNode.ChildNodes.OfType<XmlWhitespace>()
                    .OrderBy( x => x.Length ).FirstOrDefault();
            if (result != null)
                return (XmlWhitespace)result.Clone();

            int length = GetXmlNodeLevel(xmlNode) * 2;
            return xmlNode.OwnerDocument.CreateWhitespace("".PadLeft(length));
        }
    }
}
