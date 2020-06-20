using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AAB RID: 2731
	public class WealthWatcher
	{
		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x060040A5 RID: 16549 RVA: 0x0015A4A6 File Offset: 0x001586A6
		public int HealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.totalHealth;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x060040A6 RID: 16550 RVA: 0x0015A4B4 File Offset: 0x001586B4
		public float WealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems + this.wealthBuildings + this.wealthPawns;
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x060040A7 RID: 16551 RVA: 0x0015A4D0 File Offset: 0x001586D0
		public float WealthItems
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x060040A8 RID: 16552 RVA: 0x0015A4DE File Offset: 0x001586DE
		public float WealthBuildings
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthBuildings;
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x060040A9 RID: 16553 RVA: 0x0015A4EC File Offset: 0x001586EC
		public float WealthFloorsOnly
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthFloorsOnly;
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x060040AA RID: 16554 RVA: 0x0015A4FA File Offset: 0x001586FA
		public float WealthPawns
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthPawns;
			}
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x0015A508 File Offset: 0x00158708
		public static void ResetStaticData()
		{
			int num = -1;
			List<TerrainDef> allDefsListForReading = DefDatabase<TerrainDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				num = Mathf.Max(num, (int)allDefsListForReading[i].index);
			}
			WealthWatcher.cachedTerrainMarketValue = new float[num + 1];
			for (int j = 0; j < allDefsListForReading.Count; j++)
			{
				WealthWatcher.cachedTerrainMarketValue[(int)allDefsListForReading[j].index] = allDefsListForReading[j].GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x0015A583 File Offset: 0x00158783
		public WealthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x0015A5A8 File Offset: 0x001587A8
		private void RecountIfNeeded()
		{
			if ((float)Find.TickManager.TicksGame - this.lastCountTick > 5000f)
			{
				this.ForceRecount(false);
			}
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x0015A5CC File Offset: 0x001587CC
		public void ForceRecount(bool allowDuringInit = false)
		{
			if (!allowDuringInit && Current.ProgramState != ProgramState.Playing)
			{
				Log.Error("WealthWatcher recount in game mode " + Current.ProgramState, false);
				return;
			}
			this.wealthItems = this.CalculateWealthItems();
			this.wealthBuildings = 0f;
			this.wealthPawns = 0f;
			this.wealthFloorsOnly = 0f;
			this.totalHealth = 0;
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.Faction == Faction.OfPlayer)
				{
					this.wealthBuildings += thing.MarketValue;
					this.totalHealth += thing.HitPoints;
				}
			}
			this.wealthFloorsOnly = this.CalculateWealthFloors();
			this.wealthBuildings += this.wealthFloorsOnly;
			foreach (Pawn pawn in this.map.mapPawns.PawnsInFaction(Faction.OfPlayer))
			{
				if (!pawn.IsQuestLodger())
				{
					this.wealthPawns += pawn.MarketValue;
					if (pawn.IsFreeColonist)
					{
						this.totalHealth += Mathf.RoundToInt(pawn.health.summaryHealth.SummaryHealthPercent * 100f);
					}
				}
			}
			this.lastCountTick = (float)Find.TickManager.TicksGame;
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x0015A760 File Offset: 0x00158960
		public static float GetEquipmentApparelAndInventoryWealth(Pawn p)
		{
			float num = 0f;
			if (p.equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = p.equipment.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					num += allEquipmentListForReading[i].MarketValue * (float)allEquipmentListForReading[i].stackCount;
				}
			}
			if (p.apparel != null)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int j = 0; j < wornApparel.Count; j++)
				{
					num += wornApparel[j].MarketValue * (float)wornApparel[j].stackCount;
				}
			}
			if (p.inventory != null)
			{
				ThingOwner<Thing> innerContainer = p.inventory.innerContainer;
				for (int k = 0; k < innerContainer.Count; k++)
				{
					num += innerContainer[k].MarketValue * (float)innerContainer[k].stackCount;
				}
			}
			return num;
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x0015A84C File Offset: 0x00158A4C
		private float CalculateWealthItems()
		{
			this.tmpThings.Clear();
			ThingOwnerUtility.GetAllThingsRecursively<Thing>(this.map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), this.tmpThings, false, delegate(IThingHolder x)
			{
				if (x is PassingShip || x is MapComponent)
				{
					return false;
				}
				Pawn pawn = x as Pawn;
				return (pawn == null || pawn.Faction == Faction.OfPlayer) && (pawn == null || !pawn.IsQuestLodger());
			}, true);
			float num = 0f;
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				if (this.tmpThings[i].SpawnedOrAnyParentSpawned && !this.tmpThings[i].PositionHeld.Fogged(this.map))
				{
					num += this.tmpThings[i].MarketValue * (float)this.tmpThings[i].stackCount;
				}
			}
			this.tmpThings.Clear();
			return num;
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0015A91C File Offset: 0x00158B1C
		private float CalculateWealthFloors()
		{
			TerrainDef[] topGrid = this.map.terrainGrid.topGrid;
			bool[] fogGrid = this.map.fogGrid.fogGrid;
			IntVec3 size = this.map.Size;
			float num = 0f;
			int i = 0;
			int num2 = size.x * size.z;
			while (i < num2)
			{
				if (!fogGrid[i])
				{
					num += WealthWatcher.cachedTerrainMarketValue[(int)topGrid[i].index];
				}
				i++;
			}
			return num;
		}

		// Token: 0x04002580 RID: 9600
		private Map map;

		// Token: 0x04002581 RID: 9601
		private float wealthItems;

		// Token: 0x04002582 RID: 9602
		private float wealthBuildings;

		// Token: 0x04002583 RID: 9603
		private float wealthPawns;

		// Token: 0x04002584 RID: 9604
		private float wealthFloorsOnly;

		// Token: 0x04002585 RID: 9605
		private int totalHealth;

		// Token: 0x04002586 RID: 9606
		private float lastCountTick = -99999f;

		// Token: 0x04002587 RID: 9607
		private static float[] cachedTerrainMarketValue;

		// Token: 0x04002588 RID: 9608
		private const int MinCountInterval = 5000;

		// Token: 0x04002589 RID: 9609
		private List<Thing> tmpThings = new List<Thing>();
	}
}
