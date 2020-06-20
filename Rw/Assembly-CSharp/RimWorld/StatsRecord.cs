using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A27 RID: 2599
	public class StatsRecord : IExposable
	{
		// Token: 0x06003D85 RID: 15749 RVA: 0x00145680 File Offset: 0x00143880
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.numRaidsEnemy, "numRaidsEnemy", 0, false);
			Scribe_Values.Look<int>(ref this.numThreatBigs, "numThreatsQueued", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsKilled, "colonistsKilled", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsLaunched, "colonistsLaunched", 0, false);
			Scribe_Values.Look<int>(ref this.greatestPopulation, "greatestPopulation", 3, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.UpdateGreatestPopulation();
			}
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x001456F5 File Offset: 0x001438F5
		public void Notify_ColonistKilled()
		{
			this.colonistsKilled++;
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x00145708 File Offset: 0x00143908
		public void UpdateGreatestPopulation()
		{
			int a = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>();
			this.greatestPopulation = Mathf.Max(a, this.greatestPopulation);
		}

		// Token: 0x040023E9 RID: 9193
		public int numRaidsEnemy;

		// Token: 0x040023EA RID: 9194
		public int numThreatBigs;

		// Token: 0x040023EB RID: 9195
		public int colonistsKilled;

		// Token: 0x040023EC RID: 9196
		public int colonistsLaunched;

		// Token: 0x040023ED RID: 9197
		public int greatestPopulation;
	}
}
