using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	
	public class PatchOperationInsert : PatchOperationPathed
	{
		
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
					IEnumerator enumerator2 = node.ChildNodes.GetEnumerator();
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

		
		private XmlContainer value;

		
		private PatchOperationInsert.Order order = PatchOperationInsert.Order.Prepend;

		
		private enum Order
		{
			
			Append,
			
			Prepend
		}
	}
}
