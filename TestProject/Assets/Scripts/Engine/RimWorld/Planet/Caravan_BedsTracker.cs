using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200124B RID: 4683
	public class Caravan_BedsTracker : IExposable
	{
		// Token: 0x06006D15 RID: 27925 RVA: 0x00263237 File Offset: 0x00261437
		public Caravan_BedsTracker()
		{
		}

		// Token: 0x06006D16 RID: 27926 RVA: 0x0026324A File Offset: 0x0026144A
		public Caravan_BedsTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06006D17 RID: 27927 RVA: 0x00263264 File Offset: 0x00261464
		public void BedsTrackerTick()
		{
			this.RecalculateUsedBeds();
			foreach (KeyValuePair<Pawn, Building_Bed> keyValuePair in this.usedBeds)
			{
				PawnUtility.GainComfortFromThingIfPossible(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06006D18 RID: 27928 RVA: 0x002632CC File Offset: 0x002614CC
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecalculateUsedBeds();
			}
		}

		// Token: 0x06006D19 RID: 27929 RVA: 0x002632DC File Offset: 0x002614DC
		private void RecalculateUsedBeds()
		{
			this.usedBeds.Clear();
			if (!this.caravan.Spawned)
			{
				return;
			}
			Caravan_BedsTracker.tmpUsableBeds.Clear();
			this.GetUsableBeds(Caravan_BedsTracker.tmpUsableBeds);
			if (!this.caravan.pather.MovingNow)
			{
				Caravan_BedsTracker.tmpUsableBeds.SortByDescending((Building_Bed x) => x.GetStatValue(StatDefOf.BedRestEffectiveness, true));
				for (int i = 0; i < this.caravan.pawns.Count; i++)
				{
					Pawn pawn = this.caravan.pawns[i];
					if (pawn.needs != null && pawn.needs.rest != null)
					{
						Building_Bed andRemoveFirstAvailableBedFor = this.GetAndRemoveFirstAvailableBedFor(pawn, Caravan_BedsTracker.tmpUsableBeds);
						if (andRemoveFirstAvailableBedFor != null)
						{
							this.usedBeds.Add(pawn, andRemoveFirstAvailableBedFor);
						}
					}
				}
			}
			else
			{
				Caravan_BedsTracker.tmpUsableBeds.SortByDescending((Building_Bed x) => x.GetStatValue(StatDefOf.ImmunityGainSpeedFactor, true));
				for (int j = 0; j < this.caravan.pawns.Count; j++)
				{
					Pawn pawn2 = this.caravan.pawns[j];
					if (pawn2.needs != null && pawn2.needs.rest != null && CaravanBedUtility.WouldBenefitFromRestingInBed(pawn2) && (!this.caravan.pather.MovingNow || pawn2.CarriedByCaravan()))
					{
						Building_Bed andRemoveFirstAvailableBedFor2 = this.GetAndRemoveFirstAvailableBedFor(pawn2, Caravan_BedsTracker.tmpUsableBeds);
						if (andRemoveFirstAvailableBedFor2 != null)
						{
							this.usedBeds.Add(pawn2, andRemoveFirstAvailableBedFor2);
						}
					}
				}
			}
			Caravan_BedsTracker.tmpUsableBeds.Clear();
		}

		// Token: 0x06006D1A RID: 27930 RVA: 0x0026347A File Offset: 0x0026167A
		public void Notify_CaravanSpawned()
		{
			this.RecalculateUsedBeds();
		}

		// Token: 0x06006D1B RID: 27931 RVA: 0x0026347A File Offset: 0x0026167A
		public void Notify_PawnRemoved()
		{
			this.RecalculateUsedBeds();
		}

		// Token: 0x06006D1C RID: 27932 RVA: 0x00263484 File Offset: 0x00261684
		public Building_Bed GetBedUsedBy(Pawn p)
		{
			Building_Bed building_Bed;
			if (this.usedBeds.TryGetValue(p, out building_Bed) && !building_Bed.DestroyedOrNull())
			{
				return building_Bed;
			}
			return null;
		}

		// Token: 0x06006D1D RID: 27933 RVA: 0x002634AC File Offset: 0x002616AC
		public bool IsInBed(Pawn p)
		{
			return this.GetBedUsedBy(p) != null;
		}

		// Token: 0x06006D1E RID: 27934 RVA: 0x002634B8 File Offset: 0x002616B8
		public int GetUsedBedCount()
		{
			return this.usedBeds.Count;
		}

		// Token: 0x06006D1F RID: 27935 RVA: 0x002634C8 File Offset: 0x002616C8
		private void GetUsableBeds(List<Building_Bed> outBeds)
		{
			outBeds.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Building_Bed building_Bed = list[i].GetInnerIfMinified() as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_caravansCanUse)
				{
					for (int j = 0; j < list[i].stackCount; j++)
					{
						for (int k = 0; k < building_Bed.SleepingSlotsCount; k++)
						{
							outBeds.Add(building_Bed);
						}
					}
				}
			}
		}

		// Token: 0x06006D20 RID: 27936 RVA: 0x00263554 File Offset: 0x00261754
		private Building_Bed GetAndRemoveFirstAvailableBedFor(Pawn p, List<Building_Bed> beds)
		{
			for (int i = 0; i < beds.Count; i++)
			{
				if (RestUtility.CanUseBedEver(p, beds[i].def))
				{
					Building_Bed result = beds[i];
					beds.RemoveAt(i);
					return result;
				}
			}
			return null;
		}

		// Token: 0x06006D21 RID: 27937 RVA: 0x00263598 File Offset: 0x00261798
		public string GetInBedForMedicalReasonsInspectStringLine()
		{
			if (this.usedBeds.Count == 0)
			{
				return null;
			}
			Caravan_BedsTracker.tmpPawnLabels.Clear();
			foreach (KeyValuePair<Pawn, Building_Bed> keyValuePair in this.usedBeds)
			{
				if (!this.caravan.carryTracker.IsCarried(keyValuePair.Key) && CaravanBedUtility.WouldBenefitFromRestingInBed(keyValuePair.Key))
				{
					Caravan_BedsTracker.tmpPawnLabels.Add(keyValuePair.Key.LabelShort);
				}
			}
			if (!Caravan_BedsTracker.tmpPawnLabels.Any<string>())
			{
				return null;
			}
			string t = (Caravan_BedsTracker.tmpPawnLabels.Count > 5) ? (Caravan_BedsTracker.tmpPawnLabels.Take(5).ToCommaList(false) + "...") : Caravan_BedsTracker.tmpPawnLabels.ToCommaList(true);
			Caravan_BedsTracker.tmpPawnLabels.Clear();
			return "UsingBedrollsDueToIllness".Translate() + ": " + t;
		}

		// Token: 0x040043CD RID: 17357
		public Caravan caravan;

		// Token: 0x040043CE RID: 17358
		private Dictionary<Pawn, Building_Bed> usedBeds = new Dictionary<Pawn, Building_Bed>();

		// Token: 0x040043CF RID: 17359
		private static List<Building_Bed> tmpUsableBeds = new List<Building_Bed>();

		// Token: 0x040043D0 RID: 17360
		private static List<string> tmpPawnLabels = new List<string>();
	}
}
