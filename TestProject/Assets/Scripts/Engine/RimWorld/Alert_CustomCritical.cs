using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDD RID: 3549
	public class Alert_CustomCritical : Alert_Critical
	{
		// Token: 0x06005620 RID: 22048 RVA: 0x001C8CC8 File Offset: 0x001C6EC8
		public override string GetLabel()
		{
			return this.label;
		}

		// Token: 0x06005621 RID: 22049 RVA: 0x001C8CD0 File Offset: 0x001C6ED0
		public override TaggedString GetExplanation()
		{
			return this.explanation;
		}

		// Token: 0x06005622 RID: 22050 RVA: 0x001C8CDD File Offset: 0x001C6EDD
		public override AlertReport GetReport()
		{
			return this.report;
		}

		// Token: 0x04002F18 RID: 12056
		public string label;

		// Token: 0x04002F19 RID: 12057
		public string explanation;

		// Token: 0x04002F1A RID: 12058
		public AlertReport report;
	}
}
