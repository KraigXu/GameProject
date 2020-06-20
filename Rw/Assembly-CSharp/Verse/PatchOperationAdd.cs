using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000203 RID: 515
	public class PatchOperationAdd : PatchOperationPathed
	{
		// Token: 0x06000EC7 RID: 3783 RVA: 0x000540F4 File Offset: 0x000522F4
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				result = true;
				XmlNode xmlNode = obj as XmlNode;
				if (this.order == PatchOperationAdd.Order.Append)
				{
					using (IEnumerator enumerator2 = node.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode node2 = (XmlNode)obj2;
							xmlNode.AppendChild(xmlNode.OwnerDocument.ImportNode(node2, true));
						}
						continue;
					}
				}
				if (this.order == PatchOperationAdd.Order.Prepend)
				{
					for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
					{
						xmlNode.PrependChild(xmlNode.OwnerDocument.ImportNode(node.ChildNodes[i], true));
					}
				}
			}
			return result;
		}

		// Token: 0x04000AF2 RID: 2802
		private XmlContainer value;

		// Token: 0x04000AF3 RID: 2803
		private PatchOperationAdd.Order order;

		// Token: 0x0200141B RID: 5147
		private enum Order
		{
			// Token: 0x04004C6D RID: 19565
			Append,
			// Token: 0x04004C6E RID: 19566
			Prepend
		}
	}
}
