using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D4E RID: 3406
	public class CompProperties_SendSignalOnPawnProximity : CompProperties
	{
		// Token: 0x060052D3 RID: 21203 RVA: 0x001BAA10 File Offset: 0x001B8C10
		public CompProperties_SendSignalOnPawnProximity()
		{
			this.compClass = typeof(CompSendSignalOnPawnProximity);
		}

		// Token: 0x04002DB6 RID: 11702
		public bool triggerOnPawnInRoom;

		// Token: 0x04002DB7 RID: 11703
		public float radius;

		// Token: 0x04002DB8 RID: 11704
		public int enableAfterTicks;

		// Token: 0x04002DB9 RID: 11705
		public bool onlyHumanlike;

		// Token: 0x04002DBA RID: 11706
		public string signalTag;
	}
}
