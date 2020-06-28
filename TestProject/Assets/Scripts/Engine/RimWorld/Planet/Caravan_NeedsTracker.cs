using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200124E RID: 4686
	public class Caravan_NeedsTracker : IExposable
	{
		// Token: 0x06006D38 RID: 27960 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public Caravan_NeedsTracker()
		{
		}

		// Token: 0x06006D39 RID: 27961 RVA: 0x00263ADF File Offset: 0x00261CDF
		public Caravan_NeedsTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06006D3A RID: 27962 RVA: 0x00002681 File Offset: 0x00000881
		public void ExposeData()
		{
		}

		// Token: 0x06006D3B RID: 27963 RVA: 0x00263AEE File Offset: 0x00261CEE
		public void NeedsTrackerTick()
		{
			this.TrySatisfyPawnsNeeds();
		}

		// Token: 0x06006D3C RID: 27964 RVA: 0x00263AF8 File Offset: 0x00261CF8
		public void TrySatisfyPawnsNeeds()
		{
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int i = pawnsListForReading.Count - 1; i >= 0; i--)
			{
				this.TrySatisfyPawnNeeds(pawnsListForReading[i]);
			}
		}

		// Token: 0x06006D3D RID: 27965 RVA: 0x00263B34 File Offset: 0x00261D34
		private void TrySatisfyPawnNeeds(Pawn pawn)
		{
			if (pawn.Dead)
			{
				return;
			}
			List<Need> allNeeds = pawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				Need need = allNeeds[i];
				Need_Rest need_Rest = need as Need_Rest;
				Need_Food need_Food = need as Need_Food;
				Need_Chemical need_Chemical = need as Need_Chemical;
				Need_Joy need_Joy = need as Need_Joy;
				if (need_Rest != null)
				{
					this.TrySatisfyRestNeed(pawn, need_Rest);
				}
				else if (need_Food != null)
				{
					this.TrySatisfyFoodNeed(pawn, need_Food);
				}
				else if (need_Chemical != null)
				{
					this.TrySatisfyChemicalNeed(pawn, need_Chemical);
				}
				else if (need_Joy != null)
				{
					this.TrySatisfyJoyNeed(pawn, need_Joy);
				}
			}
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			if (psychicEntropy.Psylink != null)
			{
				this.TryGainPsyfocus(psychicEntropy);
			}
		}

		// Token: 0x06006D3E RID: 27966 RVA: 0x00263BDC File Offset: 0x00261DDC
		private void TrySatisfyRestNeed(Pawn pawn, Need_Rest rest)
		{
			if (!this.caravan.pather.MovingNow || pawn.InCaravanBed() || pawn.CarriedByCaravan())
			{
				Building_Bed building_Bed = pawn.CurrentCaravanBed();
				float restEffectiveness = (building_Bed != null) ? building_Bed.GetStatValue(StatDefOf.BedRestEffectiveness, true) : StatDefOf.BedRestEffectiveness.valueIfMissing;
				rest.TickResting(restEffectiveness);
			}
		}

		// Token: 0x06006D3F RID: 27967 RVA: 0x00263C38 File Offset: 0x00261E38
		private void TrySatisfyFoodNeed(Pawn pawn, Need_Food food)
		{
			if (food.CurCategory < HungerCategory.Hungry)
			{
				return;
			}
			if (VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
			{
				VirtualPlantsUtility.EatVirtualPlants(pawn);
				return;
			}
			Thing thing;
			Pawn pawn2;
			if (CaravanInventoryUtility.TryGetBestFood(this.caravan, pawn, out thing, out pawn2))
			{
				food.CurLevel += thing.Ingested(pawn, food.NutritionWanted);
				if (thing.Destroyed)
				{
					if (pawn2 != null)
					{
						pawn2.inventory.innerContainer.Remove(thing);
						this.caravan.RecacheImmobilizedNow();
						this.caravan.RecacheDaysWorthOfFood();
					}
					if (!this.caravan.notifiedOutOfFood && !CaravanInventoryUtility.TryGetBestFood(this.caravan, pawn, out thing, out pawn2))
					{
						Messages.Message("MessageCaravanRanOutOfFood".Translate(this.caravan.LabelCap, pawn.Label, pawn.Named("PAWN")), this.caravan, MessageTypeDefOf.ThreatBig, true);
						this.caravan.notifiedOutOfFood = true;
					}
				}
			}
		}

		// Token: 0x06006D40 RID: 27968 RVA: 0x00263D3C File Offset: 0x00261F3C
		private void TrySatisfyChemicalNeed(Pawn pawn, Need_Chemical chemical)
		{
			if (chemical.CurCategory >= DrugDesireCategory.Satisfied)
			{
				return;
			}
			Thing drug;
			Pawn drugOwner;
			if (CaravanInventoryUtility.TryGetDrugToSatisfyChemicalNeed(this.caravan, pawn, chemical, out drug, out drugOwner))
			{
				this.IngestDrug(pawn, drug, drugOwner);
			}
		}

		// Token: 0x06006D41 RID: 27969 RVA: 0x00263D70 File Offset: 0x00261F70
		public void IngestDrug(Pawn pawn, Thing drug, Pawn drugOwner)
		{
			float num = drug.Ingested(pawn, 0f);
			Need_Food food = pawn.needs.food;
			if (food != null)
			{
				food.CurLevel += num;
			}
			if (drug.Destroyed && drugOwner != null)
			{
				drugOwner.inventory.innerContainer.Remove(drug);
				this.caravan.RecacheImmobilizedNow();
				this.caravan.RecacheDaysWorthOfFood();
			}
		}

		// Token: 0x06006D42 RID: 27970 RVA: 0x00263DDC File Offset: 0x00261FDC
		private void TrySatisfyJoyNeed(Pawn pawn, Need_Joy joy)
		{
			if (pawn.IsHashIntervalTick(1250))
			{
				float num = this.GetCurrentJoyGainPerTick(pawn);
				if (num <= 0f)
				{
					return;
				}
				num *= 1250f;
				Caravan_NeedsTracker.tmpAvailableJoyKinds.Clear();
				this.GetAvailableJoyKindsFor(pawn, Caravan_NeedsTracker.tmpAvailableJoyKinds);
				JoyKindDef joyKind;
				if (!Caravan_NeedsTracker.tmpAvailableJoyKinds.TryRandomElementByWeight((JoyKindDef x) => 1f - Mathf.Clamp01(pawn.needs.joy.tolerances[x]), out joyKind))
				{
					return;
				}
				joy.GainJoy(num, joyKind);
				Caravan_NeedsTracker.tmpAvailableJoyKinds.Clear();
			}
		}

		// Token: 0x06006D43 RID: 27971 RVA: 0x00263E6E File Offset: 0x0026206E
		public float GetCurrentJoyGainPerTick(Pawn pawn)
		{
			if (this.caravan.pather.MovingNow)
			{
				return 0f;
			}
			return 4E-05f;
		}

		// Token: 0x06006D44 RID: 27972 RVA: 0x00263E8D File Offset: 0x0026208D
		public void TryGainPsyfocus(Pawn_PsychicEntropyTracker tracker)
		{
			if (!this.caravan.pather.MovingNow && !this.caravan.NightResting)
			{
				tracker.GainPsyfocus(null);
			}
		}

		// Token: 0x06006D45 RID: 27973 RVA: 0x00263EB8 File Offset: 0x002620B8
		public bool AnyPawnOutOfFood(out string malnutritionHediff)
		{
			Caravan_NeedsTracker.tmpInvFood.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.IsNutritionGivingIngestible)
				{
					Caravan_NeedsTracker.tmpInvFood.Add(list[i]);
				}
			}
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int j = 0; j < pawnsListForReading.Count; j++)
			{
				Pawn pawn = pawnsListForReading[j];
				if (pawn.RaceProps.EatsFood && !VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
				{
					bool flag = false;
					for (int k = 0; k < Caravan_NeedsTracker.tmpInvFood.Count; k++)
					{
						if (CaravanPawnsNeedsUtility.CanEatForNutritionEver(Caravan_NeedsTracker.tmpInvFood[k].def, pawn))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						int num = -1;
						string text = null;
						for (int l = 0; l < pawnsListForReading.Count; l++)
						{
							Hediff firstHediffOfDef = pawnsListForReading[l].health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
							if (firstHediffOfDef != null && (text == null || firstHediffOfDef.CurStageIndex > num))
							{
								num = firstHediffOfDef.CurStageIndex;
								text = firstHediffOfDef.LabelCap;
							}
						}
						malnutritionHediff = text;
						Caravan_NeedsTracker.tmpInvFood.Clear();
						return true;
					}
				}
			}
			malnutritionHediff = null;
			Caravan_NeedsTracker.tmpInvFood.Clear();
			return false;
		}

		// Token: 0x06006D46 RID: 27974 RVA: 0x00264018 File Offset: 0x00262218
		private void GetAvailableJoyKindsFor(Pawn p, List<JoyKindDef> outJoyKinds)
		{
			outJoyKinds.Clear();
			if (!p.needs.joy.tolerances.BoredOf(JoyKindDefOf.Meditative))
			{
				outJoyKinds.Add(JoyKindDefOf.Meditative);
			}
			if (!p.needs.joy.tolerances.BoredOf(JoyKindDefOf.Social))
			{
				int num = 0;
				for (int i = 0; i < this.caravan.pawns.Count; i++)
				{
					if (this.caravan.pawns[i].RaceProps.Humanlike && !this.caravan.pawns[i].Downed && !this.caravan.pawns[i].InMentalState)
					{
						num++;
					}
				}
				if (num >= 2)
				{
					outJoyKinds.Add(JoyKindDefOf.Social);
				}
			}
		}

		// Token: 0x040043D8 RID: 17368
		public Caravan caravan;

		// Token: 0x040043D9 RID: 17369
		private static List<JoyKindDef> tmpAvailableJoyKinds = new List<JoyKindDef>();

		// Token: 0x040043DA RID: 17370
		private static List<Thing> tmpInvFood = new List<Thing>();
	}
}
