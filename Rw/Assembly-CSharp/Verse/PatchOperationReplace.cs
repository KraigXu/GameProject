using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000207 RID: 519
	public class PatchOperationReplace : PatchOperationPathed
	{
		// Token: 0x06000ECF RID: 3791 RVA: 0x000544A0 File Offset: 0x000526A0
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				XmlNode parentNode = xmlNode.ParentNode;
				foreach (object obj in node.ChildNodes)
				{
					XmlNode node2 = (XmlNode)obj;
					parentNode.InsertBefore(parentNode.OwnerDocument.ImportNode(node2, true), xmlNode);
				}
				parentNode.RemoveChild(xmlNode);
			}
			return result;
		}

		// Token: 0x04000AF7 RID: 2807
		private XmlContainer value;
	}
}
