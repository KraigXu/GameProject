using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000204 RID: 516
	public class PatchOperationAddModExtension : PatchOperationPathed
	{
		// Token: 0x06000EC9 RID: 3785 RVA: 0x0005421C File Offset: 0x0005241C
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

		// Token: 0x04000AF4 RID: 2804
		private XmlContainer value;
	}
}
