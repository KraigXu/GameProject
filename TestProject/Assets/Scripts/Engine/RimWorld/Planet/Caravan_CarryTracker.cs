using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200124C RID: 4684
	public class Caravan_CarryTracker : IExposable
	{
		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x06006D23 RID: 27939 RVA: 0x002636BE File Offset: 0x002618BE
		public List<Pawn> CarriedPawnsListForReading
		{
			get
			{
				return this.carriedPawns;
			}
		}

		// Token: 0x06006D24 RID: 27940 RVA: 0x002636C6 File Offset: 0x002618C6
		public Caravan_CarryTracker()
		{
		}

		// Token: 0x06006D25 RID: 27941 RVA: 0x002636D9 File Offset: 0x002618D9
		public Caravan_CarryTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06006D26 RID: 27942 RVA: 0x002636F3 File Offset: 0x002618F3
		public void CarryTrackerTick()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x06006D27 RID: 27943 RVA: 0x002636FB File Offset: 0x002618FB
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecalculateCarriedPawns();
			}
		}

		// Token: 0x06006D28 RID: 27944 RVA: 0x0026370B File Offset: 0x0026190B
		public bool IsCarried(Pawn p)
		{
			return this.carriedPawns.Contains(p);
		}

		// Token: 0x06006D29 RID: 27945 RVA: 0x0026371C File Offset: 0x0026191C
		private void RecalculateCarriedPawns()
		{
			this.carriedPawns.Clear();
			if (!this.caravan.Spawned)
			{
				return;
			}
			if (this.caravan.pather.MovingNow)
			{
				Caravan_CarryTracker.tmpPawnsWhoCanCarry.Clear();
				this.CalculatePawnsWhoCanCarry(Caravan_CarryTracker.tmpPawnsWhoCanCarry);
				int num = 0;
				while (num < this.caravan.pawns.Count && Caravan_CarryTracker.tmpPawnsWhoCanCarry.Any<Pawn>())
				{
					Pawn pawn = this.caravan.pawns[num];
					if (this.WantsToBeCarried(pawn) && Caravan_CarryTracker.tmpPawnsWhoCanCarry.Any<Pawn>())
					{
						this.carriedPawns.Add(pawn);
						Caravan_CarryTracker.tmpPawnsWhoCanCarry.RemoveLast<Pawn>();
					}
					num++;
				}
				Caravan_CarryTracker.tmpPawnsWhoCanCarry.Clear();
			}
		}

		// Token: 0x06006D2A RID: 27946 RVA: 0x002636F3 File Offset: 0x002618F3
		public void Notify_CaravanSpawned()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x06006D2B RID: 27947 RVA: 0x002636F3 File Offset: 0x002618F3
		public void Notify_PawnRemoved()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x06006D2C RID: 27948 RVA: 0x002637DC File Offset: 0x002619DC
		private void CalculatePawnsWhoCanCarry(List<Pawn> outPawns)
		{
			outPawns.Clear();
			for (int i = 0; i < this.caravan.pawns.Count; i++)
			{
				Pawn pawn = this.caravan.pawns[i];
				if (pawn.RaceProps.Humanlike && !pawn.Downed && !pawn.InMentalState && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && !this.WantsToBeCarried(pawn))
				{
					outPawns.Add(pawn);
				}
			}
		}

		// Token: 0x06006D2D RID: 27949 RVA: 0x00263860 File Offset: 0x00261A60
		private bool WantsToBeCarried(Pawn p)
		{
			return p.health.beCarriedByCaravanIfSick && CaravanCarryUtility.WouldBenefitFromBeingCarried(p);
		}

		// Token: 0x06006D2E RID: 27950 RVA: 0x00263878 File Offset: 0x00261A78
		public string GetInspectStringLine()
		{
			if (!this.carriedPawns.Any<Pawn>())
			{
				return null;
			}
			Caravan_CarryTracker.tmpPawnLabels.Clear();
			int num = 0;
			for (int i = 0; i < this.carriedPawns.Count; i++)
			{
				Caravan_CarryTracker.tmpPawnLabels.Add(this.carriedPawns[i].LabelShort);
				if (this.caravan.beds.IsInBed(this.carriedPawns[i]))
				{
					num++;
				}
			}
			string str = (Caravan_CarryTracker.tmpPawnLabels.Count > 5) ? (Caravan_CarryTracker.tmpPawnLabels.Take(5).ToCommaList(false) + "...") : Caravan_CarryTracker.tmpPawnLabels.ToCommaList(true);
			string result = CaravanBedUtility.AppendUsingBedsLabel("BeingCarriedDueToIllness".Translate() + ": " + str.CapitalizeFirst(), num);
			Caravan_CarryTracker.tmpPawnLabels.Clear();
			return result;
		}

		// Token: 0x040043D1 RID: 17361
		public Caravan caravan;

		// Token: 0x040043D2 RID: 17362
		private List<Pawn> carriedPawns = new List<Pawn>();

		// Token: 0x040043D3 RID: 17363
		private static List<Pawn> tmpPawnsWhoCanCarry = new List<Pawn>();

		// Token: 0x040043D4 RID: 17364
		private static List<string> tmpPawnLabels = new List<string>();
	}
}
