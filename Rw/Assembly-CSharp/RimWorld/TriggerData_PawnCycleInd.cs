using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007BA RID: 1978
	public class TriggerData_PawnCycleInd : TriggerData
	{
		// Token: 0x0600333B RID: 13115 RVA: 0x0011C052 File Offset: 0x0011A252
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.pawnCycleInd, "pawnCycleInd", 0, false);
		}

		// Token: 0x04001B93 RID: 7059
		public int pawnCycleInd;
	}
}
