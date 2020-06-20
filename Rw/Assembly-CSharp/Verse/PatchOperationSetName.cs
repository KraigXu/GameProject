using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000208 RID: 520
	public class PatchOperationSetName : PatchOperationPathed
	{
		// Token: 0x06000ED1 RID: 3793 RVA: 0x00054564 File Offset: 0x00052764
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				XmlNode xmlNode2 = xmlNode.OwnerDocument.CreateElement(this.name);
				xmlNode2.InnerXml = xmlNode.InnerXml;
				xmlNode.ParentNode.InsertBefore(xmlNode2, xmlNode);
				xmlNode.ParentNode.RemoveChild(xmlNode);
			}
			return result;
		}

		// Token: 0x04000AF8 RID: 2808
		protected string name;
	}
}
