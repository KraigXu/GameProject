using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000205 RID: 517
	public class PatchOperationInsert : PatchOperationPathed
	{
		// Token: 0x06000ECB RID: 3787 RVA: 0x0005431C File Offset: 0x0005251C
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				result = true;
				XmlNode xmlNode = obj as XmlNode;
				XmlNode parentNode = xmlNode.ParentNode;
				if (this.order == PatchOperationInsert.Order.Append)
				{
					using (IEnumerator enumerator2 = node.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode node2 = (XmlNode)obj2;
							parentNode.InsertAfter(parentNode.OwnerDocument.ImportNode(node2, true), xmlNode);
						}
						continue;
					}
				}
				if (this.order == PatchOperationInsert.Order.Prepend)
				{
					for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
					{
						parentNode.InsertBefore(parentNode.OwnerDocument.ImportNode(node.ChildNodes[i], true), xmlNode);
					}
				}
			}
			return result;
		}

		// Token: 0x04000AF5 RID: 2805
		private XmlContainer value;

		// Token: 0x04000AF6 RID: 2806
		private PatchOperationInsert.Order order = PatchOperationInsert.Order.Prepend;

		// Token: 0x0200141C RID: 5148
		private enum Order
		{
			// Token: 0x04004C70 RID: 19568
			Append,
			// Token: 0x04004C71 RID: 19569
			Prepend
		}
	}
}
