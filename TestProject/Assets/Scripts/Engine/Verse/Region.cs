using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001B9 RID: 441
	public sealed class Region
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x0004573C File Offset: 0x0004393C
		public Map Map
		{
			get
			{
				if (this.mapIndex >= 0)
				{
					return Find.Maps[(int)this.mapIndex];
				}
				return null;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00045759 File Offset: 0x00043959
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				RegionGrid regions = this.Map.regionGrid;
				int num;
				for (int z = this.extentsClose.minZ; z <= this.extentsClose.maxZ; z = num + 1)
				{
					for (int x = this.extentsClose.minX; x <= this.extentsClose.maxX; x = num + 1)
					{
						IntVec3 intVec = new IntVec3(x, 0, z);
						if (regions.GetRegionAt_NoRebuild_InvalidAllowed(intVec) == this)
						{
							yield return intVec;
						}
						num = x;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0004576C File Offset: 0x0004396C
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					RegionGrid regionGrid = this.Map.regionGrid;
					for (int i = this.extentsClose.minZ; i <= this.extentsClose.maxZ; i++)
					{
						for (int j = this.extentsClose.minX; j <= this.extentsClose.maxX; j++)
						{
							IntVec3 c = new IntVec3(j, 0, i);
							if (regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c) == this)
							{
								this.cachedCellCount++;
							}
						}
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x000457FD File Offset: 0x000439FD
		public IEnumerable<Region> Neighbors
		{
			get
			{
				int num;
				for (int li = 0; li < this.links.Count; li = num + 1)
				{
					RegionLink link = this.links[li];
					for (int ri = 0; ri < 2; ri = num + 1)
					{
						if (link.regions[ri] != null && link.regions[ri] != this && link.regions[ri].valid)
						{
							yield return link.regions[ri];
						}
						num = ri;
					}
					link = null;
					num = li;
				}
				yield break;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x0004580D File Offset: 0x00043A0D
		public IEnumerable<Region> NeighborsOfSameType
		{
			get
			{
				int num;
				for (int li = 0; li < this.links.Count; li = num + 1)
				{
					RegionLink link = this.links[li];
					for (int ri = 0; ri < 2; ri = num + 1)
					{
						if (link.regions[ri] != null && link.regions[ri] != this && link.regions[ri].type == this.type && link.regions[ri].valid)
						{
							yield return link.regions[ri];
						}
						num = ri;
					}
					link = null;
					num = li;
				}
				yield break;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0004581D File Offset: 0x00043A1D
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x00045825 File Offset: 0x00043A25
		public Room Room
		{
			get
			{
				return this.roomInt;
			}
			set
			{
				if (value == this.roomInt)
				{
					return;
				}
				if (this.roomInt != null)
				{
					this.roomInt.RemoveRegion(this);
				}
				this.roomInt = value;
				if (this.roomInt != null)
				{
					this.roomInt.AddRegion(this);
				}
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00045860 File Offset: 0x00043A60
		public IntVec3 RandomCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				for (int i = 0; i < 1000; i++)
				{
					IntVec3 randomCell = this.extentsClose.RandomCell;
					if (directGrid[cellIndices.CellToIndex(randomCell)] == this)
					{
						return randomCell;
					}
				}
				return this.AnyCell;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x000458B8 File Offset: 0x00043AB8
		public IntVec3 AnyCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				foreach (IntVec3 intVec in this.extentsClose)
				{
					if (directGrid[cellIndices.CellToIndex(intVec)] == this)
					{
						return intVec;
					}
				}
				Log.Error("Couldn't find any cell in region " + this.ToString(), false);
				return this.extentsClose.RandomCell;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x00045954 File Offset: 0x00043B54
		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("id: " + this.id);
				stringBuilder.AppendLine("mapIndex: " + this.mapIndex);
				stringBuilder.AppendLine("links count: " + this.links.Count);
				foreach (RegionLink regionLink in this.links)
				{
					stringBuilder.AppendLine("  --" + regionLink.ToString());
				}
				stringBuilder.AppendLine("valid: " + this.valid.ToString());
				stringBuilder.AppendLine("makeTick: " + this.debug_makeTick);
				stringBuilder.AppendLine("roomID: " + ((this.Room != null) ? this.Room.ID.ToString() : "null room!"));
				stringBuilder.AppendLine("extentsClose: " + this.extentsClose);
				stringBuilder.AppendLine("extentsLimit: " + this.extentsLimit);
				stringBuilder.AppendLine("ListerThings:");
				if (this.listerThings.AllThings != null)
				{
					for (int i = 0; i < this.listerThings.AllThings.Count; i++)
					{
						stringBuilder.AppendLine("  --" + this.listerThings.AllThings[i]);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x00045B14 File Offset: 0x00043D14
		public bool DebugIsNew
		{
			get
			{
				return this.debug_makeTick > Find.TickManager.TicksGame - 60;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x00045B2B File Offset: 0x00043D2B
		public ListerThings ListerThings
		{
			get
			{
				return this.listerThings;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x00045B33 File Offset: 0x00043D33
		public bool IsDoorway
		{
			get
			{
				return this.door != null;
			}
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00045B40 File Offset: 0x00043D40
		private Region()
		{
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00045BD0 File Offset: 0x00043DD0
		public static Region MakeNewUnfilled(IntVec3 root, Map map)
		{
			Region region = new Region();
			region.debug_makeTick = Find.TickManager.TicksGame;
			region.id = Region.nextId;
			Region.nextId++;
			region.mapIndex = (sbyte)map.Index;
			region.precalculatedHashCode = Gen.HashCombineInt(region.id, 1295813358);
			region.extentsClose.minX = root.x;
			region.extentsClose.maxX = root.x;
			region.extentsClose.minZ = root.z;
			region.extentsClose.maxZ = root.z;
			region.extentsLimit.minX = root.x - root.x % 12;
			region.extentsLimit.maxX = root.x + 12 - (root.x + 12) % 12 - 1;
			region.extentsLimit.minZ = root.z - root.z % 12;
			region.extentsLimit.maxZ = root.z + 12 - (root.z + 12) % 12 - 1;
			region.extentsLimit.ClipInsideMap(map);
			return region;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00045CFC File Offset: 0x00043EFC
		public bool Allows(TraverseParms tp, bool isDestination)
		{
			if (tp.mode != TraverseMode.PassAllDestroyableThings && tp.mode != TraverseMode.PassAllDestroyableThingsNotWater && !this.type.Passable())
			{
				return false;
			}
			if (tp.maxDanger < Danger.Deadly && tp.pawn != null)
			{
				Danger danger = this.DangerFor(tp.pawn);
				if (isDestination || danger == Danger.Deadly)
				{
					Region region = tp.pawn.GetRegion(RegionType.Set_All);
					if ((region == null || danger > region.DangerFor(tp.pawn)) && danger > tp.maxDanger)
					{
						return false;
					}
				}
			}
			switch (tp.mode)
			{
			case TraverseMode.ByPawn:
			{
				if (this.door == null)
				{
					return true;
				}
				ByteGrid avoidGrid = tp.pawn.GetAvoidGrid(true);
				if (avoidGrid != null && avoidGrid[this.door.Position] == 255)
				{
					return false;
				}
				if (tp.pawn.HostileTo(this.door))
				{
					return this.door.CanPhysicallyPass(tp.pawn) || tp.canBash;
				}
				return this.door.CanPhysicallyPass(tp.pawn) && !this.door.IsForbiddenToPass(tp.pawn);
			}
			case TraverseMode.PassDoors:
				return true;
			case TraverseMode.NoPassClosedDoors:
				return this.door == null || this.door.FreePassage;
			case TraverseMode.PassAllDestroyableThings:
				return true;
			case TraverseMode.NoPassClosedDoorsOrWater:
				return this.door == null || this.door.FreePassage;
			case TraverseMode.PassAllDestroyableThingsNotWater:
				return true;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00045E6C File Offset: 0x0004406C
		public Danger DangerFor(Pawn p)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (this.cachedDangersForFrame != Time.frameCount)
				{
					this.cachedDangers.Clear();
					this.cachedDangersForFrame = Time.frameCount;
				}
				else
				{
					for (int i = 0; i < this.cachedDangers.Count; i++)
					{
						if (this.cachedDangers[i].Key == p)
						{
							return this.cachedDangers[i].Value;
						}
					}
				}
			}
			float temperature = this.Room.Temperature;
			FloatRange value;
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Region.cachedSafeTemperatureRangesForFrame != Time.frameCount)
				{
					Region.cachedSafeTemperatureRanges.Clear();
					Region.cachedSafeTemperatureRangesForFrame = Time.frameCount;
				}
				if (!Region.cachedSafeTemperatureRanges.TryGetValue(p, out value))
				{
					value = p.SafeTemperatureRange();
					Region.cachedSafeTemperatureRanges.Add(p, value);
				}
			}
			else
			{
				value = p.SafeTemperatureRange();
			}
			Danger danger;
			if (value.Includes(temperature))
			{
				danger = Danger.None;
			}
			else if (value.ExpandedBy(80f).Includes(temperature))
			{
				danger = Danger.Some;
			}
			else
			{
				danger = Danger.Deadly;
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.cachedDangers.Add(new KeyValuePair<Pawn, Danger>(p, danger));
			}
			return danger;
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00045F90 File Offset: 0x00044190
		public float GetBaseDesiredPlantsCount(bool allowCache = true)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (allowCache && ticksGame - this.cachedBaseDesiredPlantsCountForTick < 2500)
			{
				return this.cachedBaseDesiredPlantsCount;
			}
			this.cachedBaseDesiredPlantsCount = 0f;
			Map map = this.Map;
			foreach (IntVec3 c in this.Cells)
			{
				this.cachedBaseDesiredPlantsCount += map.wildPlantSpawner.GetBaseDesiredPlantsCountAt(c);
			}
			this.cachedBaseDesiredPlantsCountForTick = ticksGame;
			return this.cachedBaseDesiredPlantsCount;
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00046034 File Offset: 0x00044234
		public AreaOverlap OverlapWith(Area a)
		{
			if (a.TrueCount == 0)
			{
				return AreaOverlap.None;
			}
			if (this.Map != a.Map)
			{
				return AreaOverlap.None;
			}
			if (this.cachedAreaOverlaps == null)
			{
				this.cachedAreaOverlaps = new Dictionary<Area, AreaOverlap>();
			}
			AreaOverlap areaOverlap;
			if (!this.cachedAreaOverlaps.TryGetValue(a, out areaOverlap))
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 c in this.Cells)
				{
					num2++;
					if (a[c])
					{
						num++;
					}
				}
				if (num == 0)
				{
					areaOverlap = AreaOverlap.None;
				}
				else if (num == num2)
				{
					areaOverlap = AreaOverlap.Entire;
				}
				else
				{
					areaOverlap = AreaOverlap.Partial;
				}
				this.cachedAreaOverlaps.Add(a, areaOverlap);
			}
			return areaOverlap;
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x000460F0 File Offset: 0x000442F0
		public void Notify_AreaChanged(Area a)
		{
			if (this.cachedAreaOverlaps == null)
			{
				return;
			}
			if (this.cachedAreaOverlaps.ContainsKey(a))
			{
				this.cachedAreaOverlaps.Remove(a);
			}
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00046118 File Offset: 0x00044318
		public void DecrementMapIndex()
		{
			if (this.mapIndex <= 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to decrement map index for region ",
					this.id,
					", but mapIndex=",
					this.mapIndex
				}), false);
				return;
			}
			this.mapIndex -= 1;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0004617B File Offset: 0x0004437B
		public void Notify_MyMapRemoved()
		{
			this.listerThings.Clear();
			this.mapIndex = -1;
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0004618F File Offset: 0x0004438F
		public static void ClearStaticData()
		{
			Region.cachedSafeTemperatureRanges.Clear();
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0004619C File Offset: 0x0004439C
		public override string ToString()
		{
			string str;
			if (this.door != null)
			{
				str = this.door.ToString();
			}
			else
			{
				str = "null";
			}
			return string.Concat(new object[]
			{
				"Region(id=",
				this.id,
				", mapIndex=",
				this.mapIndex,
				", center=",
				this.extentsClose.CenterCell,
				", links=",
				this.links.Count,
				", cells=",
				this.CellCount,
				(this.door != null) ? (", portal=" + str) : null,
				")"
			});
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00046270 File Offset: 0x00044470
		public void DebugDraw()
		{
			if (DebugViewSettings.drawRegionTraversal && Find.TickManager.TicksGame < this.debug_lastTraverseTick + 60)
			{
				float a = 1f - (float)(Find.TickManager.TicksGame - this.debug_lastTraverseTick) / 60f;
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), new Color(0f, 0f, 1f, a));
			}
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x000462E0 File Offset: 0x000444E0
		public void DebugDrawMouseover()
		{
			int num = Mathf.RoundToInt(Time.realtimeSinceStartup * 2f) % 2;
			if (DebugViewSettings.drawRegions)
			{
				Color color;
				if (!this.valid)
				{
					color = Color.red;
				}
				else if (this.DebugIsNew)
				{
					color = Color.yellow;
				}
				else
				{
					color = Color.green;
				}
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), color);
				foreach (Region region in this.Neighbors)
				{
					GenDraw.DrawFieldEdges(region.Cells.ToList<IntVec3>(), Color.grey);
				}
			}
			if (DebugViewSettings.drawRegionLinks)
			{
				foreach (RegionLink regionLink in this.links)
				{
					if (num == 1)
					{
						foreach (IntVec3 c in regionLink.span.Cells)
						{
							CellRenderer.RenderCell(c, DebugSolidColorMats.MaterialOf(Color.magenta));
						}
					}
				}
			}
			if (DebugViewSettings.drawRegionThings)
			{
				foreach (Thing thing in this.listerThings.AllThings)
				{
					CellRenderer.RenderSpot(thing.TrueCenter(), (float)(thing.thingIDNumber % 256) / 256f);
				}
			}
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00046488 File Offset: 0x00044688
		public void Debug_Notify_Traversed()
		{
			this.debug_lastTraverseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0004649A File Offset: 0x0004469A
		public override int GetHashCode()
		{
			return this.precalculatedHashCode;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x000464A4 File Offset: 0x000446A4
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Region region = obj as Region;
			return region != null && region.id == this.id;
		}

		// Token: 0x040009AE RID: 2478
		public RegionType type = RegionType.Normal;

		// Token: 0x040009AF RID: 2479
		public int id = -1;

		// Token: 0x040009B0 RID: 2480
		public sbyte mapIndex = -1;

		// Token: 0x040009B1 RID: 2481
		private Room roomInt;

		// Token: 0x040009B2 RID: 2482
		public List<RegionLink> links = new List<RegionLink>();

		// Token: 0x040009B3 RID: 2483
		public CellRect extentsClose;

		// Token: 0x040009B4 RID: 2484
		public CellRect extentsLimit;

		// Token: 0x040009B5 RID: 2485
		public Building_Door door;

		// Token: 0x040009B6 RID: 2486
		private int precalculatedHashCode;

		// Token: 0x040009B7 RID: 2487
		public bool touchesMapEdge;

		// Token: 0x040009B8 RID: 2488
		private int cachedCellCount = -1;

		// Token: 0x040009B9 RID: 2489
		public bool valid = true;

		// Token: 0x040009BA RID: 2490
		private ListerThings listerThings = new ListerThings(ListerThingsUse.Region);

		// Token: 0x040009BB RID: 2491
		public uint[] closedIndex = new uint[RegionTraverser.NumWorkers];

		// Token: 0x040009BC RID: 2492
		public uint reachedIndex;

		// Token: 0x040009BD RID: 2493
		public int newRegionGroupIndex = -1;

		// Token: 0x040009BE RID: 2494
		private Dictionary<Area, AreaOverlap> cachedAreaOverlaps;

		// Token: 0x040009BF RID: 2495
		public int mark;

		// Token: 0x040009C0 RID: 2496
		private List<KeyValuePair<Pawn, Danger>> cachedDangers = new List<KeyValuePair<Pawn, Danger>>();

		// Token: 0x040009C1 RID: 2497
		private int cachedDangersForFrame;

		// Token: 0x040009C2 RID: 2498
		private float cachedBaseDesiredPlantsCount;

		// Token: 0x040009C3 RID: 2499
		private int cachedBaseDesiredPlantsCountForTick = -999999;

		// Token: 0x040009C4 RID: 2500
		private static Dictionary<Pawn, FloatRange> cachedSafeTemperatureRanges = new Dictionary<Pawn, FloatRange>();

		// Token: 0x040009C5 RID: 2501
		private static int cachedSafeTemperatureRangesForFrame;

		// Token: 0x040009C6 RID: 2502
		private int debug_makeTick = -1000;

		// Token: 0x040009C7 RID: 2503
		private int debug_lastTraverseTick = -1000;

		// Token: 0x040009C8 RID: 2504
		private static int nextId = 1;

		// Token: 0x040009C9 RID: 2505
		public const int GridSize = 12;
	}
}
