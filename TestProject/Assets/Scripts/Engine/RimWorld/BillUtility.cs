using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public static class BillUtility
	{
		
		public static void TryDrawIngredientSearchRadiusOnMap(this Bill bill, IntVec3 center)
		{
			if (bill.ingredientSearchRadius < GenRadial.MaxRadialPatternRadius)
			{
				GenDraw.DrawRadiusRing(center, bill.ingredientSearchRadius);
			}
		}

		
		public static Bill MakeNewBill(this RecipeDef recipe)
		{
			if (recipe.UsesUnfinishedThing)
			{
				return new Bill_ProductionWithUft(recipe);
			}
			return new Bill_Production(recipe);
		}

		
		public static IEnumerable<IBillGiver> GlobalBillGivers()
		{
			foreach (Map map in Find.Maps)
			{
				foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.PotentialBillGiver)))
				{
					IBillGiver billGiver = thing as IBillGiver;
					if (billGiver == null)
					{
						Log.ErrorOnce("Found non-bill-giver tagged as PotentialBillGiver", 13389774, false);
					}
					else
					{
						yield return billGiver;
					}
				}
				List<Thing>.Enumerator enumerator2 = default(List<Thing>.Enumerator);
				foreach (Thing outerThing in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MinifiedThing)))
				{
					IBillGiver billGiver2 = outerThing.GetInnerIfMinified() as IBillGiver;
					if (billGiver2 != null)
					{
						yield return billGiver2;
					}
				}
				enumerator2 = default(List<Thing>.Enumerator);
				map = null;
			}
			List<Map>.Enumerator enumerator = default(List<Map>.Enumerator);
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				foreach (Thing outerThing2 in caravan.AllThings)
				{
					IBillGiver billGiver3 = outerThing2.GetInnerIfMinified() as IBillGiver;
					if (billGiver3 != null)
					{
						yield return billGiver3;
					}
				}
				IEnumerator<Thing> enumerator4 = null;
			}
			List<Caravan>.Enumerator enumerator3 = default(List<Caravan>.Enumerator);
			yield break;
			yield break;
		}

		
		public static IEnumerable<Bill> GlobalBills()
		{
			foreach (IBillGiver billGiver in BillUtility.GlobalBillGivers())
			{
				foreach (Bill bill in billGiver.BillStack)
				{
					yield return bill;
				}
				IEnumerator<Bill> enumerator2 = null;
			}
			IEnumerator<IBillGiver> enumerator = null;
			if (BillUtility.Clipboard != null)
			{
				yield return BillUtility.Clipboard;
			}
			yield break;
			yield break;
		}

		
		public static void Notify_ZoneStockpileRemoved(Zone_Stockpile stockpile)
		{
			foreach (Bill bill in BillUtility.GlobalBills())
			{
				bill.ValidateSettings();
			}
		}

		
		public static void Notify_ColonistUnavailable(Pawn pawn)
		{
			try
			{
				foreach (Bill bill in BillUtility.GlobalBills())
				{
					bill.ValidateSettings();
				}
			}
			catch (Exception arg)
			{
				Log.Error("Could not notify bills: " + arg, false);
			}
		}

		
		public static WorkGiverDef GetWorkgiver(this IBillGiver billGiver)
		{
			Thing thing = billGiver as Thing;
			if (thing == null)
			{
				Log.ErrorOnce(string.Format("Attempting to get the workgiver for a non-Thing IBillGiver {0}", billGiver.ToString()), 96810282, false);
				return null;
			}
			List<WorkGiverDef> allDefsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkGiverDef workGiverDef = allDefsListForReading[i];
				WorkGiver_DoBill workGiver_DoBill = workGiverDef.Worker as WorkGiver_DoBill;
				if (workGiver_DoBill != null && workGiver_DoBill.ThingIsUsableBillGiver(thing))
				{
					return workGiverDef;
				}
			}
			Log.ErrorOnce(string.Format("Can't find a WorkGiver for a BillGiver {0}", thing.ToString()), 57348705, false);
			return null;
		}

		
		public static Bill Clipboard;
	}
}
