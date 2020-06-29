using System;
using System.Xml;

namespace Verse
{
	
	public class PatchOperationAddModExtension : PatchOperationPathed
	{
		
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				XmlNode xmlNode2 = xmlNode["modExtensions"];
				if (xmlNode2 == null)
				{
					xmlNode2 = xmlNode.OwnerDocument.CreateElement("modExtensions");
					xmlNode.AppendChild(xmlNode2);
				}
				foreach (object obj2 in node.ChildNodes)
				{
					XmlNode node2 = (XmlNode)obj2;
					xmlNode2.AppendChild(xmlNode.OwnerDocument.ImportNode(node2, true));
				}
				result = true;
			}
			return result;
		}

		
		private XmlContainer value;
	}
}
