using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200020C RID: 524
	public class PatchOperationAttributeSet : PatchOperationAttribute
	{
		// Token: 0x06000ED9 RID: 3801 RVA: 0x0005472C File Offset: 0x0005292C
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] != null)
				{
					xmlNode.Attributes[this.attribute].Value = this.value;
				}
				else
				{
					XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
					xmlAttribute.Value = this.value;
					xmlNode.Attributes.Append(xmlAttribute);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04000AFB RID: 2811
		protected string value;
	}
}
