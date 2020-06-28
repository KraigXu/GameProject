using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDC RID: 3548
	public class Alert_Custom : Alert
	{
		// Token: 0x0600561C RID: 22044 RVA: 0x001C8CA3 File Offset: 0x001C6EA3
		public override string GetLabel()
		{
			return this.label;
		}

		// Token: 0x0600561D RID: 22045 RVA: 0x001C8CAB File Offset: 0x001C6EAB
		public override TaggedString GetExplanation()
		{
			return this.explanation;
		}

		// Token: 0x0600561E RID: 22046 RVA: 0x001C8CB8 File Offset: 0x001C6EB8
		public override AlertReport GetReport()
		{
			return this.report;
		}

		// Token: 0x04002F15 RID: 12053
		public string label;

		// Token: 0x04002F16 RID: 12054
		public string explanation;

		// Token: 0x04002F17 RID: 12055
		public AlertReport report;
	}
}
