using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007AB RID: 1963
	public class LordToilData_Party : LordToilData
	{
		// Token: 0x06003303 RID: 13059 RVA: 0x0011B544 File Offset: 0x00119744
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.presentForTicks.RemoveAll((KeyValuePair<Pawn, int> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Pawn, int>(ref this.presentForTicks, "presentForTicks", LookMode.Reference, LookMode.Undefined);
		}

		// Token: 0x04001B7F RID: 7039
		public Dictionary<Pawn, int> presentForTicks = new Dictionary<Pawn, int>();
	}
}
