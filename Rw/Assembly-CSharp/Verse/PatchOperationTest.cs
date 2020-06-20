using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200020E RID: 526
	public class PatchOperationTest : PatchOperationPathed
	{
		// Token: 0x06000EDF RID: 3807 RVA: 0x000548BE File Offset: 0x00052ABE
		protected override bool ApplyWorker(XmlDocument xml)
		{
			return xml.SelectSingleNode(this.xpath) != null;
		}
	}
}
