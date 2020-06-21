using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001284 RID: 4740
	public class WorldObjectsHolder : IExposable
	{
		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x06006F33 RID: 28467 RVA: 0x0026B2F4 File Offset: 0x002694F4
		public List<WorldObject> AllWorldObjects
		{
			get
			{
				return this.worldObjects;
			}
		}

		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x06006F34 RID: 28468 RVA: 0x0026B2FC File Offset: 0x002694FC
		public List<Caravan> Caravans
		{
			get
			{
				return this.caravans;
			}
		}

		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06006F35 RID: 28469 RVA: 0x0026B304 File Offset: 0x00269504
		public List<Settlement> Settlements
		{
			get
			{
				return this.settlements;
			}
		}

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06006F36 RID: 28470 RVA: 0x0026B30C File Offset: 0x0026950C
		public List<TravelingTransportPods> TravelingTransportPods
		{
			get
			{
				return this.travelingTransportPods;
			}
		}

		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x06006F37 RID: 28471 RVA: 0x0026B314 File Offset: 0x00269514
		public List<Settlement> SettlementBases
		{
			get
			{
				return this.settlementBases;
			}
		}

		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x06006F38 RID: 28472 RVA: 0x0026B31C File Offset: 0x0026951C
		public List<DestroyedSettlement> DestroyedSettlements
		{
			get
			{
				return this.destroyedSettlements;
			}
		}

		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x06006F39 RID: 28473 RVA: 0x0026B324 File Offset: 0x00269524
		public List<RoutePlannerWaypoint> RoutePlannerWaypoints
		{
			get
			{
				return this.routePlannerWaypoints;
			}
		}

		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x06006F3A RID: 28474 RVA: 0x0026B32C File Offset: 0x0026952C
		public List<MapParent> MapParents
		{
			get
			{
				return this.mapParents;
			}
		}

		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x06006F3B RID: 28475 RVA: 0x0026B334 File Offset: 0x00269534
		public List<Site> Sites
		{
			get
			{
				return this.sites;
			}
		}

		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x06006F3C RID: 28476 RVA: 0x0026B33C File Offset: 0x0026953C
		public List<PeaceTalks> PeaceTalks
		{
			get
			{
				return this.peaceTalks;
			}
		}

		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x06006F3D RID: 28477 RVA: 0x0026B344 File Offset: 0x00269544
		public int WorldObjectsCount
		{
			get
			{
				return this.worldObjects.Count;
			}
		}

		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x06006F3E RID: 28478 RVA: 0x0026B351 File Offset: 0x00269551
		public int CaravansCount
		{
			get
			{
				return this.caravans.Count;
			}
		}

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x06006F3F RID: 28479 RVA: 0x0026B35E File Offset: 0x0026955E
		public int RoutePlannerWaypointsCount
		{
			get
			{
				return this.routePlannerWaypoints.Count;
			}
		}

		// Token: 0x170012BC RID: 4796
		// (get) Token: 0x06006F40 RID: 28480 RVA: 0x0026B36C File Offset: 0x0026956C
		public int PlayerControlledCaravansCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.caravans.Count; i++)
				{
					if (this.caravans[i].IsPlayerControlled)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06006F41 RID: 28481 RVA: 0x0026B3AC File Offset: 0x002695AC
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				WorldObjectsHolder.tmpUnsavedWorldObjects.Clear();
				for (int i = this.worldObjects.Count - 1; i >= 0; i--)
				{
					if (!this.worldObjects[i].def.saved)
					{
						WorldObjectsHolder.tmpUnsavedWorldObjects.Add(this.worldObjects[i]);
						this.worldObjects.RemoveAt(i);
					}
				}
			}
			Scribe_Collections.Look<WorldObject>(ref this.worldObjects, "worldObjects", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.worldObjects.AddRange(WorldObjectsHolder.tmpUnsavedWorldObjects);
				WorldObjectsHolder.tmpUnsavedWorldObjects.Clear();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.worldObjects.RemoveAll((WorldObject wo) => wo == null);
				this.Recache();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.worldObjects.RemoveAll((WorldObject wo) => wo == null || wo.def == null) != 0)
				{
					Log.Error("Some WorldObjects had null def after loading.", false);
				}
				for (int j = this.worldObjects.Count - 1; j >= 0; j--)
				{
					try
					{
						this.worldObjects[j].SpawnSetup();
					}
					catch (Exception arg)
					{
						Log.Error("Exception spawning WorldObject: " + arg, false);
						this.worldObjects.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x06006F42 RID: 28482 RVA: 0x0026B52C File Offset: 0x0026972C
		public void Add(WorldObject o)
		{
			if (this.worldObjects.Contains(o))
			{
				Log.Error("Tried to add world object " + o + " to world, but it's already here.", false);
				return;
			}
			if (o.Tile < 0)
			{
				Log.Error("Tried to add world object " + o + " but its tile is not set. Setting to 0.", false);
				o.Tile = 0;
			}
			this.worldObjects.Add(o);
			this.AddToCache(o);
			o.SpawnSetup();
			o.PostAdd();
		}

		// Token: 0x06006F43 RID: 28483 RVA: 0x0026B5A4 File Offset: 0x002697A4
		public void Remove(WorldObject o)
		{
			if (!this.worldObjects.Contains(o))
			{
				Log.Error("Tried to remove world object " + o + " from world, but it's not here.", false);
				return;
			}
			this.worldObjects.Remove(o);
			this.RemoveFromCache(o);
			o.PostRemove();
		}

		// Token: 0x06006F44 RID: 28484 RVA: 0x0026B5F0 File Offset: 0x002697F0
		public void WorldObjectsHolderTick()
		{
			WorldObjectsHolder.tmpWorldObjects.Clear();
			WorldObjectsHolder.tmpWorldObjects.AddRange(this.worldObjects);
			for (int i = 0; i < WorldObjectsHolder.tmpWorldObjects.Count; i++)
			{
				WorldObjectsHolder.tmpWorldObjects[i].Tick();
			}
		}

		// Token: 0x06006F45 RID: 28485 RVA: 0x0026B63C File Offset: 0x0026983C
		private void AddToCache(WorldObject o)
		{
			this.worldObjectsHashSet.Add(o);
			if (o is Caravan)
			{
				this.caravans.Add((Caravan)o);
			}
			if (o is Settlement)
			{
				this.settlements.Add((Settlement)o);
			}
			if (o is TravelingTransportPods)
			{
				this.travelingTransportPods.Add((TravelingTransportPods)o);
			}
			if (o is Settlement)
			{
				this.settlementBases.Add((Settlement)o);
			}
			if (o is DestroyedSettlement)
			{
				this.destroyedSettlements.Add((DestroyedSettlement)o);
			}
			if (o is RoutePlannerWaypoint)
			{
				this.routePlannerWaypoints.Add((RoutePlannerWaypoint)o);
			}
			if (o is MapParent)
			{
				this.mapParents.Add((MapParent)o);
			}
			if (o is Site)
			{
				this.sites.Add((Site)o);
			}
			if (o is PeaceTalks)
			{
				this.peaceTalks.Add((PeaceTalks)o);
			}
		}

		// Token: 0x06006F46 RID: 28486 RVA: 0x0026B738 File Offset: 0x00269938
		private void RemoveFromCache(WorldObject o)
		{
			this.worldObjectsHashSet.Remove(o);
			if (o is Caravan)
			{
				this.caravans.Remove((Caravan)o);
			}
			if (o is Settlement)
			{
				this.settlements.Remove((Settlement)o);
			}
			if (o is TravelingTransportPods)
			{
				this.travelingTransportPods.Remove((TravelingTransportPods)o);
			}
			if (o is Settlement)
			{
				this.settlementBases.Remove((Settlement)o);
			}
			if (o is DestroyedSettlement)
			{
				this.destroyedSettlements.Remove((DestroyedSettlement)o);
			}
			if (o is RoutePlannerWaypoint)
			{
				this.routePlannerWaypoints.Remove((RoutePlannerWaypoint)o);
			}
			if (o is MapParent)
			{
				this.mapParents.Remove((MapParent)o);
			}
			if (o is Site)
			{
				this.sites.Remove((Site)o);
			}
			if (o is PeaceTalks)
			{
				this.peaceTalks.Remove((PeaceTalks)o);
			}
		}

		// Token: 0x06006F47 RID: 28487 RVA: 0x0026B83C File Offset: 0x00269A3C
		private void Recache()
		{
			this.worldObjectsHashSet.Clear();
			this.caravans.Clear();
			this.settlements.Clear();
			this.travelingTransportPods.Clear();
			this.settlementBases.Clear();
			this.destroyedSettlements.Clear();
			this.routePlannerWaypoints.Clear();
			this.mapParents.Clear();
			this.sites.Clear();
			this.peaceTalks.Clear();
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				this.AddToCache(this.worldObjects[i]);
			}
		}

		// Token: 0x06006F48 RID: 28488 RVA: 0x0026B8DF File Offset: 0x00269ADF
		public bool Contains(WorldObject o)
		{
			return o != null && this.worldObjectsHashSet.Contains(o);
		}

		// Token: 0x06006F49 RID: 28489 RVA: 0x0026B8F2 File Offset: 0x00269AF2
		public IEnumerable<WorldObject> ObjectsAt(int tileID)
		{
			if (tileID < 0)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.worldObjects.Count; i = num + 1)
			{
				if (this.worldObjects[i].Tile == tileID)
				{
					yield return this.worldObjects[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06006F4A RID: 28490 RVA: 0x0026B90C File Offset: 0x00269B0C
		public bool AnyWorldObjectAt(int tile)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006F4B RID: 28491 RVA: 0x0026B946 File Offset: 0x00269B46
		public bool AnyWorldObjectAt<T>(int tile) where T : WorldObject
		{
			return this.WorldObjectAt<T>(tile) != null;
		}

		// Token: 0x06006F4C RID: 28492 RVA: 0x0026B958 File Offset: 0x00269B58
		public T WorldObjectAt<T>(int tile) where T : WorldObject
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile && this.worldObjects[i] is T)
				{
					return this.worldObjects[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x06006F4D RID: 28493 RVA: 0x0026B9C2 File Offset: 0x00269BC2
		public bool AnyWorldObjectAt(int tile, WorldObjectDef def)
		{
			return this.WorldObjectAt(tile, def) != null;
		}

		// Token: 0x06006F4E RID: 28494 RVA: 0x0026B9D0 File Offset: 0x00269BD0
		public WorldObject WorldObjectAt(int tile, WorldObjectDef def)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile && this.worldObjects[i].def == def)
				{
					return this.worldObjects[i];
				}
			}
			return null;
		}

		// Token: 0x06006F4F RID: 28495 RVA: 0x0026BA29 File Offset: 0x00269C29
		public bool AnySettlementAt(int tile)
		{
			return this.SettlementAt(tile) != null;
		}

		// Token: 0x06006F50 RID: 28496 RVA: 0x0026BA38 File Offset: 0x00269C38
		public Settlement SettlementAt(int tile)
		{
			for (int i = 0; i < this.settlements.Count; i++)
			{
				if (this.settlements[i].Tile == tile)
				{
					return this.settlements[i];
				}
			}
			return null;
		}

		// Token: 0x06006F51 RID: 28497 RVA: 0x0026BA7D File Offset: 0x00269C7D
		public bool AnySettlementBaseAt(int tile)
		{
			return this.SettlementBaseAt(tile) != null;
		}

		// Token: 0x06006F52 RID: 28498 RVA: 0x0026BA8C File Offset: 0x00269C8C
		public Settlement SettlementBaseAt(int tile)
		{
			for (int i = 0; i < this.settlementBases.Count; i++)
			{
				if (this.settlementBases[i].Tile == tile)
				{
					return this.settlementBases[i];
				}
			}
			return null;
		}

		// Token: 0x06006F53 RID: 28499 RVA: 0x0026BAD1 File Offset: 0x00269CD1
		public bool AnySiteAt(int tile)
		{
			return this.SiteAt(tile) != null;
		}

		// Token: 0x06006F54 RID: 28500 RVA: 0x0026BAE0 File Offset: 0x00269CE0
		public Site SiteAt(int tile)
		{
			for (int i = 0; i < this.sites.Count; i++)
			{
				if (this.sites[i].Tile == tile)
				{
					return this.sites[i];
				}
			}
			return null;
		}

		// Token: 0x06006F55 RID: 28501 RVA: 0x0026BB25 File Offset: 0x00269D25
		public bool AnyDestroyedSettlementAt(int tile)
		{
			return this.DestroyedSettlementAt(tile) != null;
		}

		// Token: 0x06006F56 RID: 28502 RVA: 0x0026BB34 File Offset: 0x00269D34
		public DestroyedSettlement DestroyedSettlementAt(int tile)
		{
			for (int i = 0; i < this.destroyedSettlements.Count; i++)
			{
				if (this.destroyedSettlements[i].Tile == tile)
				{
					return this.destroyedSettlements[i];
				}
			}
			return null;
		}

		// Token: 0x06006F57 RID: 28503 RVA: 0x0026BB79 File Offset: 0x00269D79
		public bool AnyMapParentAt(int tile)
		{
			return this.MapParentAt(tile) != null;
		}

		// Token: 0x06006F58 RID: 28504 RVA: 0x0026BB88 File Offset: 0x00269D88
		public MapParent MapParentAt(int tile)
		{
			for (int i = 0; i < this.mapParents.Count; i++)
			{
				if (this.mapParents[i].Tile == tile)
				{
					return this.mapParents[i];
				}
			}
			return null;
		}

		// Token: 0x06006F59 RID: 28505 RVA: 0x0026BBCD File Offset: 0x00269DCD
		public bool AnyWorldObjectOfDefAt(WorldObjectDef def, int tile)
		{
			return this.WorldObjectOfDefAt(def, tile) != null;
		}

		// Token: 0x06006F5A RID: 28506 RVA: 0x0026BBDC File Offset: 0x00269DDC
		public WorldObject WorldObjectOfDefAt(WorldObjectDef def, int tile)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].def == def && this.worldObjects[i].Tile == tile)
				{
					return this.worldObjects[i];
				}
			}
			return null;
		}

		// Token: 0x06006F5B RID: 28507 RVA: 0x0026BC38 File Offset: 0x00269E38
		public Caravan PlayerControlledCaravanAt(int tile)
		{
			for (int i = 0; i < this.caravans.Count; i++)
			{
				if (this.caravans[i].Tile == tile && this.caravans[i].IsPlayerControlled)
				{
					return this.caravans[i];
				}
			}
			return null;
		}

		// Token: 0x06006F5C RID: 28508 RVA: 0x0026BC90 File Offset: 0x00269E90
		public bool AnySettlementBaseAtOrAdjacent(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < this.settlementBases.Count; i++)
			{
				if (worldGrid.IsNeighborOrSame(this.settlementBases[i].Tile, tile))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006F5D RID: 28509 RVA: 0x0026BCD8 File Offset: 0x00269ED8
		public RoutePlannerWaypoint RoutePlannerWaypointAt(int tile)
		{
			for (int i = 0; i < this.routePlannerWaypoints.Count; i++)
			{
				if (this.routePlannerWaypoints[i].Tile == tile)
				{
					return this.routePlannerWaypoints[i];
				}
			}
			return null;
		}

		// Token: 0x06006F5E RID: 28510 RVA: 0x0026BD20 File Offset: 0x00269F20
		public void GetPlayerControlledCaravansAt(int tile, List<Caravan> outCaravans)
		{
			outCaravans.Clear();
			for (int i = 0; i < this.caravans.Count; i++)
			{
				Caravan caravan = this.caravans[i];
				if (caravan.Tile == tile && caravan.IsPlayerControlled)
				{
					outCaravans.Add(caravan);
				}
			}
		}

		// Token: 0x04004456 RID: 17494
		private List<WorldObject> worldObjects = new List<WorldObject>();

		// Token: 0x04004457 RID: 17495
		private HashSet<WorldObject> worldObjectsHashSet = new HashSet<WorldObject>();

		// Token: 0x04004458 RID: 17496
		private List<Caravan> caravans = new List<Caravan>();

		// Token: 0x04004459 RID: 17497
		private List<Settlement> settlements = new List<Settlement>();

		// Token: 0x0400445A RID: 17498
		private List<TravelingTransportPods> travelingTransportPods = new List<TravelingTransportPods>();

		// Token: 0x0400445B RID: 17499
		private List<Settlement> settlementBases = new List<Settlement>();

		// Token: 0x0400445C RID: 17500
		private List<DestroyedSettlement> destroyedSettlements = new List<DestroyedSettlement>();

		// Token: 0x0400445D RID: 17501
		private List<RoutePlannerWaypoint> routePlannerWaypoints = new List<RoutePlannerWaypoint>();

		// Token: 0x0400445E RID: 17502
		private List<MapParent> mapParents = new List<MapParent>();

		// Token: 0x0400445F RID: 17503
		private List<Site> sites = new List<Site>();

		// Token: 0x04004460 RID: 17504
		private List<PeaceTalks> peaceTalks = new List<PeaceTalks>();

		// Token: 0x04004461 RID: 17505
		private static List<WorldObject> tmpUnsavedWorldObjects = new List<WorldObject>();

		// Token: 0x04004462 RID: 17506
		private static List<WorldObject> tmpWorldObjects = new List<WorldObject>();
	}
}
