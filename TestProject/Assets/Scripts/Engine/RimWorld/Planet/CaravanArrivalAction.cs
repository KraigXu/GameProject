using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200121B RID: 4635
	public abstract class CaravanArrivalAction : IExposable
	{
		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x06006B71 RID: 27505
		public abstract string Label { get; }

		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x06006B72 RID: 27506
		public abstract string ReportString { get; }

		// Token: 0x06006B73 RID: 27507 RVA: 0x002584FF File Offset: 0x002566FF
		public virtual FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			return true;
		}

		// Token: 0x06006B74 RID: 27508
		public abstract void Arrived(Caravan caravan);

		// Token: 0x06006B75 RID: 27509 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ExposeData()
		{
		}
	}
}
