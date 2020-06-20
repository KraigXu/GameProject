using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000206 RID: 518
	public class PatchOperationRemove : PatchOperationPathed
	{
		// Token: 0x06000ECD RID: 3789 RVA: 0x00054458 File Offset: 0x00052658
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				xmlNode.ParentNode.RemoveChild(xmlNode);
			}
			return result;
		}
	}
}
