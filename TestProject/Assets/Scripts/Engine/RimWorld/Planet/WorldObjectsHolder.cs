using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldObjectsHolder : IExposable
	{
		
		// (get) Token: 0x06006F33 RID: 28467 RVA: 0x0026B2F4 File Offset: 0x002694F4
		public List<WorldObject> AllWorldObjects
		{
			get
			{
				return this.worldObjects;
			}
		}

		
		// (get) Token: 0x06006F34 RID: 28468 RVA: 0x0026B2FC File Offset: 0x002694FC
		public List<Caravan> Caravans
		{
			get
			{
				return this.caravans;
			}
		}

		
		// (get) Token: 0x06006F35 RID: 28469 RVA: 0x0026B304 File Offset: 0x00269504
		public List<Settlement> Settlements
		{
			get
			{
				return this.settlements;
			}
		}

		
		// (get) Token: 0x06006F36 RID: 28470 RVA: 0x0026B30C File Offset: 0x0026950C
		public List<TravelingTransportPods> TravelingTransportPods
		{
			get
			{
				return this.travelingTransportPods;
			}
		}

		
		// (get) Token: 0x06006F37 RID: 28471 RVA: 0x0026B314 File Offset: 0x00269514
		public List<Settlement> SettlementBases
		{
			get
			{
				return this.settlementBases;
			}
		}

		
		// (get) Token: 0x06006F38 RID: 28472 RVA: 0x0026B31C File Offset: 0x0026951C
		public List<DestroyedSettlement> DestroyedSettlements
		{
			get
			{
				return this.destroyedSettlements;
			}
		}

		
		// (get) Token: 0x06006F39 RID: 28473 RVA: 0x0026B324 File Offset: 0x00269524
		public List<RoutePlannerWaypoint> RoutePlannerWaypoints
		{
			get
			{
				return this.routePlannerWaypoints;
			}
		}

		
		// (get) Token: 0x06006F3A RID: 28474 RVA: 0x0026B32C File Offset: 0x0026952C
		public List<MapParent> MapParents
		{
			get
			{
				return this.mapParents;
			}
		}

		
		// (get) Token: 0x06006F3B RID: 28475 RVA: 0x0026B334 File Offset: 0x00269534
		public List<Site> Sites
		{
			get
			{
				return this.sites;
			}
		}

		
		// (get) Token: 0x06006F3C RID: 28476 RVA: 0x0026B33C File Offset: 0x0026953C
		public List<PeaceTalks> PeaceTalks
		{
			get
			{
				return this.peaceTalks;
			}
		}

		
		// (get) Token: 0x06006F3D RID: 28477 RVA: 0x0026B344 File Offset: 0x00269544
		public int WorldObjectsCount
		{
			get
			{
				return this.worldObjects.Count;
			}
		}

		
		// (get) Token: 0x06006F3E RID: 28478 RVA: 0x0026B351 File Offset: 0x00269551
		public int CaravansCount
		{
			get
			{
				return this.caravans.Count;
			}
		}

		
		// (get) Token: 0x06006F3F RID: 28479 RVA: 0x0026B35E File Offset: 0x0026955E
		public int RoutePlannerWaypointsCount
		{
			get
			{
				return this.routePlannerWaypoints.Count;
			}
		}

		
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

		
		public void WorldObjectsHolderTick()
		{
			WorldObjectsHolder.tmpWorldObjects.Clear();
			WorldObjectsHolder.tmpWorldObjects.AddRange(this.worldObjects);
			for (int i = 0; i < WorldObjectsHolder.tmpWorldObjects.Count; i++)
			{
				WorldObjectsHolder.tmpWorldObjects[i].Tick();
			}
		}

		
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

		
		public bool Contains(WorldObject o)
		{
			return o != null && this.worldObjectsHashSet.Contains(o);
		}

		
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

		
		public bool AnyWorldObjectAt<T>(int tile) where T : WorldObject
		{
			return this.WorldObjectAt<T>(tile) != null;
		}

		
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

		
		public bool AnyWorldObjectAt(int tile, WorldObjectDef def)
		{
			return this.WorldObjectAt(tile, def) != null;
		}

		
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

		
		public bool AnySettlementAt(int tile)
		{
			return this.SettlementAt(tile) != null;
		}

		
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

		
		public bool AnySettlementBaseAt(int tile)
		{
			return this.SettlementBaseAt(tile) != null;
		}

		
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

		
		public bool AnySiteAt(int tile)
		{
			return this.SiteAt(tile) != null;
		}

		
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

		
		public bool AnyDestroyedSettlementAt(int tile)
		{
			return this.DestroyedSettlementAt(tile) != null;
		}

		
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

		
		public bool AnyMapParentAt(int tile)
		{
			return this.MapParentAt(tile) != null;
		}

		
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

		
		public bool AnyWorldObjectOfDefAt(WorldObjectDef def, int tile)
		{
			return this.WorldObjectOfDefAt(def, tile) != null;
		}

		
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

		
		private List<WorldObject> worldObjects = new List<WorldObject>();

		
		private HashSet<WorldObject> worldObjectsHashSet = new HashSet<WorldObject>();

		
		private List<Caravan> caravans = new List<Caravan>();

		
		private List<Settlement> settlements = new List<Settlement>();

		
		private List<TravelingTransportPods> travelingTransportPods = new List<TravelingTransportPods>();

		
		private List<Settlement> settlementBases = new List<Settlement>();

		
		private List<DestroyedSettlement> destroyedSettlements = new List<DestroyedSettlement>();

		
		private List<RoutePlannerWaypoint> routePlannerWaypoints = new List<RoutePlannerWaypoint>();

		
		private List<MapParent> mapParents = new List<MapParent>();

		
		private List<Site> sites = new List<Site>();

		
		private List<PeaceTalks> peaceTalks = new List<PeaceTalks>();

		
		private static List<WorldObject> tmpUnsavedWorldObjects = new List<WorldObject>();

		
		private static List<WorldObject> tmpWorldObjects = new List<WorldObject>();
	}
}
