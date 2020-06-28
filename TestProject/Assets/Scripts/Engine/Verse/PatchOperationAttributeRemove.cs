using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200020B RID: 523
	public class PatchOperationAttributeRemove : PatchOperationAttribute
	{
		// Token: 0x06000ED7 RID: 3799 RVA: 0x0005469C File Offset: 0x0005289C
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] != null)
				{
					xmlNode.Attributes.Remove(xmlNode.Attributes[this.attribute]);
					result = true;
				}
			}
			return result;
		}
	}
}
