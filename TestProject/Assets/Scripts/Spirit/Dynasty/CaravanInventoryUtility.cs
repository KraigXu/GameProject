using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001236 RID: 4662
	public static class CaravanInventoryUtility
	{
		// Token: 0x06006C90 RID: 27792 RVA: 0x0025E0EC File Offset: 0x0025C2EC
		public static List<Thing> AllInventoryItems(Caravan caravan)
		{
			CaravanInventoryUtility.inventoryItems.Clear();
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				for (int j = 0; j < pawn.inventory.innerContainer.Count; j++)
				{
					Thing item = pawn.inventory.innerContainer[j];
					CaravanInventoryUtility.inventoryItems.Add(item);
				}
			}
			return CaravanInventoryUtility.inventoryItems;
		}

		// Token: 0x06006C91 RID: 27793 RVA: 0x0025E162 File Offset: 0x0025C362
		public static void CaravanInventoryUtilityStaticUpdate()
		{
			CaravanInventoryUtility.inventoryItems.Clear();
		}

		// Token: 0x06006C92 RID: 27794 RVA: 0x0025E170 File Offset: 0x0025C370
		public static Pawn GetOwnerOf(Caravan caravan, Thing item)
		{
			IThingHolder parentHolder = item.ParentHolder;
			if (parentHolder is Pawn_InventoryTracker)
			{
				Pawn pawn = (Pawn)parentHolder.ParentHolder;
				if (caravan.ContainsPawn(pawn))
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06006C93 RID: 27795 RVA: 0x0025E1A4 File Offset: 0x0025C3A4
		public static bool TryGetBestFood(Caravan caravan, Pawn forPawn, out Thing food, out Pawn owner)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			Thing thing = null;
			float num = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (CaravanPawnsNeedsUtility.CanEatForNutritionNow(thing2, forPawn))
				{
					float foodScore = CaravanPawnsNeedsUtility.GetFoodScore(thing2, forPawn);
					if (thing == null || foodScore > num)
					{
						thing = thing2;
						num = foodScore;
					}
				}
			}
			if (thing != null)
			{
				food = thing;
				owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
				return true;
			}
			food = null;
			owner = null;
			return false;
		}

		// Token: 0x06006C94 RID: 27796 RVA: 0x0025E214 File Offset: 0x0025C414
		public static bool TryGetDrugToSatisfyChemicalNeed(Caravan caravan, Pawn forPawn, Need_Chemical chemical, out Thing drug, out Pawn owner)
		{
			Hediff_Addiction addictionHediff = chemical.AddictionHediff;
			if (addictionHediff == null)
			{
				drug = null;
				owner = null;
				return false;
			}
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			Thing thing = null;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2.IngestibleNow && thing2.def.IsDrug)
				{
					CompDrug compDrug = thing2.TryGetComp<CompDrug>();
					if (compDrug != null && compDrug.Props.chemical != null && compDrug.Props.chemical.addictionHediff == addictionHediff.def && (forPawn.drugs == null || forPawn.drugs.CurrentPolicy[thing2.def].allowedForAddiction || forPawn.story == null || forPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0))
					{
						thing = thing2;
						break;
					}
				}
			}
			if (thing != null)
			{
				drug = thing;
				owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
				return true;
			}
			drug = null;
			owner = null;
			return false;
		}

		// Token: 0x06006C95 RID: 27797 RVA: 0x0025E310 File Offset: 0x0025C510
		public static bool TryGetBestMedicine(Caravan caravan, Pawn patient, out Medicine medicine, out Pawn owner)
		{
			if (patient.playerSettings == null || patient.playerSettings.medCare <= MedicalCareCategory.NoMeds)
			{
				medicine = null;
				owner = null;
				return false;
			}
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			Medicine medicine2 = null;
			float num = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.IsMedicine && patient.playerSettings.medCare.AllowsMedicine(thing.def))
				{
					float statValue = thing.GetStatValue(StatDefOf.MedicalPotency, true);
					if (statValue > num || medicine2 == null)
					{
						num = statValue;
						medicine2 = (Medicine)thing;
					}
				}
			}
			if (medicine2 != null)
			{
				medicine = medicine2;
				owner = CaravanInventoryUtility.GetOwnerOf(caravan, medicine2);
				return true;
			}
			medicine = null;
			owner = null;
			return false;
		}

		// Token: 0x06006C96 RID: 27798 RVA: 0x0025E3C8 File Offset: 0x0025C5C8
		public static bool TryGetThingOfDef(Caravan caravan, ThingDef thingDef, out Thing thing, out Pawn owner)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2.def == thingDef)
				{
					thing = thing2;
					owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing2);
					return true;
				}
			}
			thing = null;
			owner = null;
			return false;
		}

		// Token: 0x06006C97 RID: 27799 RVA: 0x0025E414 File Offset: 0x0025C614
		public static void MoveAllInventoryToSomeoneElse(Pawn from, List<Pawn> candidates, List<Pawn> ignoreCandidates = null)
		{
			CaravanInventoryUtility.inventoryToMove.Clear();
			CaravanInventoryUtility.inventoryToMove.AddRange(from.inventory.innerContainer);
			for (int i = 0; i < CaravanInventoryUtility.inventoryToMove.Count; i++)
			{
				CaravanInventoryUtility.MoveInventoryToSomeoneElse(from, CaravanInventoryUtility.inventoryToMove[i], candidates, ignoreCandidates, CaravanInventoryUtility.inventoryToMove[i].stackCount);
			}
			CaravanInventoryUtility.inventoryToMove.Clear();
		}

		// Token: 0x06006C98 RID: 27800 RVA: 0x0025E484 File Offset: 0x0025C684
		public static void MoveInventoryToSomeoneElse(Pawn itemOwner, Thing item, List<Pawn> candidates, List<Pawn> ignoreCandidates, int numToMove)
		{
			if (numToMove < 0 || numToMove > item.stackCount)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to move item ",
					item,
					" with numToMove=",
					numToMove,
					" (item stack count = ",
					item.stackCount,
					")"
				}), false);
				return;
			}
			Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, candidates, ignoreCandidates, itemOwner);
			if (pawn == null)
			{
				return;
			}
			itemOwner.inventory.innerContainer.TryTransferToContainer(item, pawn.inventory.innerContainer, numToMove, true);
		}

		// Token: 0x06006C99 RID: 27801 RVA: 0x0025E51C File Offset: 0x0025C71C
		public static Pawn FindPawnToMoveInventoryTo(Thing item, List<Pawn> candidates, List<Pawn> ignoreCandidates, Pawn currentItemOwner = null)
		{
			if (item is Pawn)
			{
				Log.Error("Called FindPawnToMoveInventoryTo but the item is a pawn.", false);
				return null;
			}
			Pawn result;
			if ((from x in candidates
			where CaravanInventoryUtility.CanMoveInventoryTo(x) && (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner && !MassUtility.IsOverEncumbered(x)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			if ((from x in candidates
			where CaravanInventoryUtility.CanMoveInventoryTo(x) && (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner
			select x).TryRandomElement(out result))
			{
				return result;
			}
			if ((from x in candidates
			where (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06006C9A RID: 27802 RVA: 0x0025E5AC File Offset: 0x0025C7AC
		public static void MoveAllApparelToSomeonesInventory(Pawn moveFrom, List<Pawn> candidates, bool moveLocked = true)
		{
			if (moveFrom.apparel == null)
			{
				return;
			}
			CaravanInventoryUtility.tmpApparel.Clear();
			if (moveLocked)
			{
				CaravanInventoryUtility.tmpApparel.AddRange(moveFrom.apparel.WornApparel);
			}
			else
			{
				for (int i = 0; i < moveFrom.apparel.WornApparel.Count; i++)
				{
					Apparel apparel = moveFrom.apparel.WornApparel[i];
					if (!moveFrom.apparel.IsLocked(apparel))
					{
						CaravanInventoryUtility.tmpApparel.Add(apparel);
					}
				}
			}
			for (int j = 0; j < CaravanInventoryUtility.tmpApparel.Count; j++)
			{
				moveFrom.apparel.Remove(CaravanInventoryUtility.tmpApparel[j]);
				Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(CaravanInventoryUtility.tmpApparel[j], candidates, null, moveFrom);
				if (pawn != null)
				{
					pawn.inventory.innerContainer.TryAdd(CaravanInventoryUtility.tmpApparel[j], true);
				}
			}
			CaravanInventoryUtility.tmpApparel.Clear();
		}

		// Token: 0x06006C9B RID: 27803 RVA: 0x0025E698 File Offset: 0x0025C898
		public static void MoveAllEquipmentToSomeonesInventory(Pawn moveFrom, List<Pawn> candidates)
		{
			if (moveFrom.equipment == null)
			{
				return;
			}
			CaravanInventoryUtility.tmpEquipment.Clear();
			CaravanInventoryUtility.tmpEquipment.AddRange(moveFrom.equipment.AllEquipmentListForReading);
			for (int i = 0; i < CaravanInventoryUtility.tmpEquipment.Count; i++)
			{
				moveFrom.equipment.Remove(CaravanInventoryUtility.tmpEquipment[i]);
				Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(CaravanInventoryUtility.tmpEquipment[i], candidates, null, moveFrom);
				if (pawn != null)
				{
					pawn.inventory.innerContainer.TryAdd(CaravanInventoryUtility.tmpEquipment[i], true);
				}
			}
			CaravanInventoryUtility.tmpEquipment.Clear();
		}

		// Token: 0x06006C9C RID: 27804 RVA: 0x0025E736 File Offset: 0x0025C936
		private static bool CanMoveInventoryTo(Pawn pawn)
		{
			return MassUtility.CanEverCarryAnything(pawn);
		}

		// Token: 0x06006C9D RID: 27805 RVA: 0x0025E740 File Offset: 0x0025C940
		public static List<Thing> TakeThings(Caravan caravan, Func<Thing, int> takeQuantity)
		{
			List<Thing> list = new List<Thing>();
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan).ToList<Thing>())
			{
				int num = takeQuantity(thing);
				if (num > 0)
				{
					list.Add(thing.holdingOwner.Take(thing, num));
				}
			}
			return list;
		}

		// Token: 0x06006C9E RID: 27806 RVA: 0x0025E7B8 File Offset: 0x0025C9B8
		public static void GiveThing(Caravan caravan, Thing thing)
		{
			if (CaravanInventoryUtility.AllInventoryItems(caravan).Contains(thing))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to give the same item twice (",
					thing,
					") to a caravan (",
					caravan,
					")."
				}), false);
				return;
			}
			Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
			if (pawn == null)
			{
				Log.Error(string.Format("Failed to give item {0} to caravan {1}; item was lost", thing, caravan), false);
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!pawn.inventory.innerContainer.TryAdd(thing, true))
			{
				Log.Error(string.Format("Failed to give item {0} to caravan {1}; item was lost", thing, caravan), false);
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
		}

		// Token: 0x06006C9F RID: 27807 RVA: 0x0025E860 File Offset: 0x0025CA60
		public static bool HasThings(Caravan caravan, ThingDef thingDef, int count, Func<Thing, bool> validator = null)
		{
			int num = 0;
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def == thingDef && (validator == null || validator(thing)))
				{
					num += thing.stackCount;
				}
			}
			return num >= count;
		}

		// Token: 0x0400438E RID: 17294
		private static List<Thing> inventoryItems = new List<Thing>();

		// Token: 0x0400438F RID: 17295
		private static List<Thing> inventoryToMove = new List<Thing>();

		// Token: 0x04004390 RID: 17296
		private static List<Apparel> tmpApparel = new List<Apparel>();

		// Token: 0x04004391 RID: 17297
		private static List<ThingWithComps> tmpEquipment = new List<ThingWithComps>();
	}
}
