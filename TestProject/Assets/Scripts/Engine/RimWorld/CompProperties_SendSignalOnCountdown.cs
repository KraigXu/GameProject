using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D4C RID: 3404
	public class CompProperties_SendSignalOnCountdown : CompProperties
	{
		// Token: 0x060052C7 RID: 21191 RVA: 0x001BA77E File Offset: 0x001B897E
		public CompProperties_SendSignalOnCountdown()
		{
			this.compClass = typeof(CompSendSignalOnCountdown);
		}

		// Token: 0x04002DB1 RID: 11697
		public SimpleCurve countdownCurveTicks;

		// Token: 0x04002DB2 RID: 11698
		public string signalTag;
	}
}
