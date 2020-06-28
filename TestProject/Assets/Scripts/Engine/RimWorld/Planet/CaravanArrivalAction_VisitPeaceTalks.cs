using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001221 RID: 4641
	public class CaravanArrivalAction_VisitPeaceTalks : CaravanArrivalAction
	{
		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x06006BA7 RID: 27559 RVA: 0x00259059 File Offset: 0x00257259
		public override string Label
		{
			get
			{
				return "VisitPeaceTalks".Translate(this.peaceTalks.Label);
			}
		}

		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x06006BA8 RID: 27560 RVA: 0x0025907A File Offset: 0x0025727A
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.peaceTalks.Label);
			}
		}

		// Token: 0x06006BA9 RID: 27561 RVA: 0x00258549 File Offset: 0x00256749
		public CaravanArrivalAction_VisitPeaceTalks()
		{
		}

		// Token: 0x06006BAA RID: 27562 RVA: 0x0025909B File Offset: 0x0025729B
		public CaravanArrivalAction_VisitPeaceTalks(PeaceTalks peaceTalks)
		{
			this.peaceTalks = peaceTalks;
		}

		// Token: 0x06006BAB RID: 27563 RVA: 0x002590AC File Offset: 0x002572AC
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.peaceTalks != null && this.peaceTalks.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, this.peaceTalks);
		}

		// Token: 0x06006BAC RID: 27564 RVA: 0x002590F5 File Offset: 0x002572F5
		public override void Arrived(Caravan caravan)
		{
			this.peaceTalks.Notify_CaravanArrived(caravan);
		}

		// Token: 0x06006BAD RID: 27565 RVA: 0x00259103 File Offset: 0x00257303
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PeaceTalks>(ref this.peaceTalks, "peaceTalks", false);
		}

		// Token: 0x06006BAE RID: 27566 RVA: 0x0025911C File Offset: 0x0025731C
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, PeaceTalks peaceTalks)
		{
			return peaceTalks != null && peaceTalks.Spawned;
		}

		// Token: 0x06006BAF RID: 27567 RVA: 0x00259130 File Offset: 0x00257330
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, PeaceTalks peaceTalks)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitPeaceTalks>(() => CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, peaceTalks), () => new CaravanArrivalAction_VisitPeaceTalks(peaceTalks), "VisitPeaceTalks".Translate(peaceTalks.Label), caravan, peaceTalks.Tile, peaceTalks, null);
		}

		// Token: 0x04004337 RID: 17207
		private PeaceTalks peaceTalks;
	}
}
