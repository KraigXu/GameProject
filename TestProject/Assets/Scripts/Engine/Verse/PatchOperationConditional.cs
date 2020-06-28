using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200020F RID: 527
	public class PatchOperationConditional : PatchOperationPathed
	{
		// Token: 0x06000EE1 RID: 3809 RVA: 0x000548D0 File Offset: 0x00052AD0
		protected override bool ApplyWorker(XmlDocument xml)
		{
			if (xml.SelectSingleNode(this.xpath) != null)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return this.match != null || this.nomatch != null;
		}

		// Token: 0x04000AFE RID: 2814
		private PatchOperation match;

		// Token: 0x04000AFF RID: 2815
		private PatchOperation nomatch;
	}
}
