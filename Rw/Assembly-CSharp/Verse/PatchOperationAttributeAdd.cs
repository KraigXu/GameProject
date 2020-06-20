using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200020A RID: 522
	public class PatchOperationAttributeAdd : PatchOperationAttribute
	{
		// Token: 0x06000ED5 RID: 3797 RVA: 0x000545F4 File Offset: 0x000527F4
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] == null)
				{
					XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
					xmlAttribute.Value = this.value;
					xmlNode.Attributes.Append(xmlAttribute);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x04000AFA RID: 2810
		protected string value;
	}
}
