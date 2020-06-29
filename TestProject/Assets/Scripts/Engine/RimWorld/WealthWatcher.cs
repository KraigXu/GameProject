using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class WealthWatcher
	{
		
		// (get) Token: 0x060040A5 RID: 16549 RVA: 0x0015A4A6 File Offset: 0x001586A6
		public int HealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.totalHealth;
			}
		}

		
		// (get) Token: 0x060040A6 RID: 16550 RVA: 0x0015A4B4 File Offset: 0x001586B4
		public float WealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems + this.wealthBuildings + this.wealthPawns;
			}
		}

		
		// (get) Token: 0x060040A7 RID: 16551 RVA: 0x0015A4D0 File Offset: 0x001586D0
		public float WealthItems
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems;
			}
		}

		
		// (get) Token: 0x060040A8 RID: 16552 RVA: 0x0015A4DE File Offset: 0x001586DE
		public float WealthBuildings
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthBuildings;
			}
		}

		
		// (get) Token: 0x060040A9 RID: 16553 RVA: 0x0015A4EC File Offset: 0x001586EC
		public float WealthFloorsOnly
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthFloorsOnly;
			}
		}

		
		// (get) Token: 0x060040AA RID: 16554 RVA: 0x0015A4FA File Offset: 0x001586FA
		public float WealthPawns
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthPawns;
			}
		}

		
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

		
		public WealthWatcher(Map map)
		{
			this.map = map;
		}

		
		private void RecountIfNeeded()
		{
			if ((float)Find.TickManager.TicksGame - this.lastCountTick > 5000f)
			{
				this.ForceRecount(false);
			}
		}

		
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

		
		private Map map;

		
		private float wealthItems;

		
		private float wealthBuildings;

		
		private float wealthPawns;

		
		private float wealthFloorsOnly;

		
		private int totalHealth;

		
		private float lastCountTick = -99999f;

		
		private static float[] cachedTerrainMarketValue;

		
		private const int MinCountInterval = 5000;

		
		private List<Thing> tmpThings = new List<Thing>();
	}
}
