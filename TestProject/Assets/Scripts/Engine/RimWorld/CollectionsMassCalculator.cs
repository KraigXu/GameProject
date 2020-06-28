using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3E RID: 3902
	public static class CollectionsMassCalculator
	{
		// Token: 0x06005FB4 RID: 24500 RVA: 0x00214168 File Offset: 0x00212368
		public static float Capacity(List<ThingCount> thingCounts, StringBuilder explanation = null)
		{
			float num = 0f;
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						num += MassUtility.Capacity(pawn, explanation) * (float)thingCounts[i].Count;
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x06005FB5 RID: 24501 RVA: 0x002141DC File Offset: 0x002123DC
		public static float MassUsage(List<ThingCount> thingCounts, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			float num = 0f;
			for (int i = 0; i < thingCounts.Count; i++)
			{
				int count = thingCounts[i].Count;
				if (count > 0)
				{
					Thing thing = thingCounts[i].Thing;
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						if (includePawnsMass)
						{
							num += pawn.GetStatValue(StatDefOf.Mass, true) * (float)count;
						}
						else
						{
							num += MassUtility.GearAndInventoryMass(pawn) * (float)count;
						}
						if (InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
						{
							num -= MassUtility.InventoryMass(pawn) * (float)count;
						}
					}
					else
					{
						num += thing.GetStatValue(StatDefOf.Mass, true) * (float)count;
						if (ignoreSpawnedCorpsesGearAndInventory)
						{
							Corpse corpse = thing as Corpse;
							if (corpse != null && corpse.Spawned)
							{
								num -= MassUtility.GearAndInventoryMass(corpse.InnerPawn) * (float)count;
							}
						}
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x06005FB6 RID: 24502 RVA: 0x002142C0 File Offset: 0x002124C0
		public static float CapacityTransferables(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				if (transferables[i].HasAnyThing && transferables[i].AnyThing is Pawn)
				{
					TransferableUtility.TransferNoSplit(transferables[i].things, transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
					{
						CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake));
					}, false, false);
				}
			}
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06005FB7 RID: 24503 RVA: 0x0021435C File Offset: 0x0021255C
		public static float CapacityLeftAfterTransfer(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				if (transferables[i].HasAnyThing && transferables[i].AnyThing is Pawn)
				{
					CollectionsMassCalculator.thingsInReverse.Clear();
					CollectionsMassCalculator.thingsInReverse.AddRange(transferables[i].things);
					CollectionsMassCalculator.thingsInReverse.Reverse();
					TransferableUtility.TransferNoSplit(CollectionsMassCalculator.thingsInReverse, transferables[i].MaxCount - transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
					{
						CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake));
					}, false, false);
				}
			}
			CollectionsMassCalculator.thingsInReverse.Clear();
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06005FB8 RID: 24504 RVA: 0x0021443B File Offset: 0x0021263B
		public static float CapacityLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, StringBuilder explanation = null)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, CollectionsMassCalculator.tmpThingCounts);
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06005FB9 RID: 24505 RVA: 0x00214468 File Offset: 0x00212668
		public static float Capacity<T>(List<T> things, StringBuilder explanation = null) where T : Thing
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < things.Count; i++)
			{
				CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(things[i], things[i].stackCount));
			}
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06005FBA RID: 24506 RVA: 0x002144D4 File Offset: 0x002126D4
		public static float MassUsageTransferables(List<TransferableOneWay> transferables, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableUtility.TransferNoSplit(transferables[i].things, transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
				{
					CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake));
				}, false, false);
			}
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06005FBB RID: 24507 RVA: 0x00214554 File Offset: 0x00212754
		public static float MassUsageLeftAfterTransfer(List<TransferableOneWay> transferables, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				CollectionsMassCalculator.thingsInReverse.Clear();
				CollectionsMassCalculator.thingsInReverse.AddRange(transferables[i].things);
				CollectionsMassCalculator.thingsInReverse.Reverse();
				TransferableUtility.TransferNoSplit(CollectionsMassCalculator.thingsInReverse, transferables[i].MaxCount - transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
				{
					CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake));
				}, false, false);
			}
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06005FBC RID: 24508 RVA: 0x00214604 File Offset: 0x00212804
		public static float MassUsage<T>(List<T> things, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false) where T : Thing
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < things.Count; i++)
			{
				CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(things[i], things[i].stackCount));
			}
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06005FBD RID: 24509 RVA: 0x0021466F File Offset: 0x0021286F
		public static float MassUsageLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, CollectionsMassCalculator.tmpThingCounts);
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x040033F4 RID: 13300
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x040033F5 RID: 13301
		private static List<Thing> thingsInReverse = new List<Thing>();
	}
}
