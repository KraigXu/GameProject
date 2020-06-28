using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000200 RID: 512
	public class XmlContainer
	{
		// Token: 0x06000EBF RID: 3775 RVA: 0x00053FE9 File Offset: 0x000521E9
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}

		// Token: 0x04000AED RID: 2797
		public XmlNode node;
	}
}
