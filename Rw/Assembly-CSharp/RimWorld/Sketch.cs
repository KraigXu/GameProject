using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA3 RID: 2723
	public class Sketch : IExposable
	{
		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06004020 RID: 16416 RVA: 0x001566DA File Offset: 0x001548DA
		public List<SketchEntity> Entities
		{
			get
			{
				return this.entities;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06004021 RID: 16417 RVA: 0x001566E2 File Offset: 0x001548E2
		public List<SketchThing> Things
		{
			get
			{
				return this.cachedThings;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06004022 RID: 16418 RVA: 0x001566EA File Offset: 0x001548EA
		public List<SketchTerrain> Terrain
		{
			get
			{
				return this.cachedTerrain;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06004023 RID: 16419 RVA: 0x001566F2 File Offset: 0x001548F2
		public List<SketchBuildable> Buildables
		{
			get
			{
				return this.cachedBuildables;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06004024 RID: 16420 RVA: 0x001566FC File Offset: 0x001548FC
		public CellRect OccupiedRect
		{
			get
			{
				if (this.occupiedRectDirty)
				{
					this.cachedOccupiedRect = CellRect.Empty;
					bool flag = false;
					for (int i = 0; i < this.entities.Count; i++)
					{
						if (!flag)
						{
							this.cachedOccupiedRect = this.entities[i].OccupiedRect;
							flag = true;
						}
						else
						{
							CellRect occupiedRect = this.entities[i].OccupiedRect;
							this.cachedOccupiedRect = CellRect.FromLimits(Mathf.Min(this.cachedOccupiedRect.minX, occupiedRect.minX), Mathf.Min(this.cachedOccupiedRect.minZ, occupiedRect.minZ), Mathf.Max(this.cachedOccupiedRect.maxX, occupiedRect.maxX), Mathf.Max(this.cachedOccupiedRect.maxZ, occupiedRect.maxZ));
						}
					}
					this.occupiedRectDirty = false;
				}
				return this.cachedOccupiedRect;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06004025 RID: 16421 RVA: 0x001567E0 File Offset: 0x001549E0
		public IntVec2 OccupiedSize
		{
			get
			{
				return new IntVec2(this.OccupiedRect.Width, this.OccupiedRect.Height);
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06004026 RID: 16422 RVA: 0x00156810 File Offset: 0x00154A10
		public IntVec3 OccupiedCenter
		{
			get
			{
				return this.OccupiedRect.CenterCell;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06004027 RID: 16423 RVA: 0x0015682B File Offset: 0x00154A2B
		public bool Empty
		{
			get
			{
				return !this.Entities.Any<SketchEntity>();
			}
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x0015683C File Offset: 0x00154A3C
		public bool Add(SketchEntity entity, bool wipeIfCollides = true)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			if (this.entities.Contains(entity))
			{
				return true;
			}
			if (wipeIfCollides)
			{
				this.WipeColliding(entity);
			}
			else if (this.WouldCollide(entity))
			{
				return false;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			SketchTerrain entity2;
			if (sketchTerrain != null && this.terrainAt.TryGetValue(sketchTerrain.pos, out entity2))
			{
				this.Remove(entity2);
			}
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable != null)
			{
				for (int i = this.cachedBuildables.Count - 1; i >= 0; i--)
				{
					if (sketchBuildable.OccupiedRect.Overlaps(this.cachedBuildables[i].OccupiedRect) && GenSpawn.SpawningWipes(sketchBuildable.Buildable, this.cachedBuildables[i].Buildable))
					{
						this.Remove(this.cachedBuildables[i]);
					}
				}
			}
			this.entities.Add(entity);
			this.AddToCache(entity);
			return true;
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x00156930 File Offset: 0x00154B30
		public bool AddThing(ThingDef def, IntVec3 pos, Rot4 rot, ThingDef stuff = null, int stackCount = 1, QualityCategory? quality = null, int? hitPoints = null, bool wipeIfCollides = true)
		{
			return this.Add(new SketchThing
			{
				def = def,
				stuff = stuff,
				pos = pos,
				rot = rot,
				stackCount = stackCount,
				quality = quality,
				hitPoints = hitPoints
			}, wipeIfCollides);
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x00156984 File Offset: 0x00154B84
		public bool AddTerrain(TerrainDef def, IntVec3 pos, bool wipeIfCollides = true)
		{
			return this.Add(new SketchTerrain
			{
				def = def,
				pos = pos
			}, wipeIfCollides);
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x001569AD File Offset: 0x00154BAD
		public bool Remove(SketchEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			if (!this.entities.Contains(entity))
			{
				return false;
			}
			this.entities.Remove(entity);
			this.RemoveFromCache(entity);
			return true;
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x001569DC File Offset: 0x00154BDC
		public bool RemoveTerrain(IntVec3 cell)
		{
			SketchTerrain entity;
			return this.terrainAt.TryGetValue(cell, out entity) && this.Remove(entity);
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x00156A02 File Offset: 0x00154C02
		public void Clear()
		{
			this.entities.Clear();
			this.RecacheAll();
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x00156A18 File Offset: 0x00154C18
		public TerrainDef TerrainAt(IntVec3 pos)
		{
			SketchTerrain sketchTerrain = this.SketchTerrainAt(pos);
			if (sketchTerrain == null)
			{
				return null;
			}
			return sketchTerrain.def;
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x00156A38 File Offset: 0x00154C38
		public SketchTerrain SketchTerrainAt(IntVec3 pos)
		{
			SketchTerrain result;
			if (!this.terrainAt.TryGetValue(pos, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x00156A58 File Offset: 0x00154C58
		public bool AnyTerrainAt(int x, int z)
		{
			return this.AnyTerrainAt(new IntVec3(x, 0, z));
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x00156A68 File Offset: 0x00154C68
		public bool AnyTerrainAt(IntVec3 pos)
		{
			return this.TerrainAt(pos) != null;
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x00156A74 File Offset: 0x00154C74
		public IEnumerable<SketchThing> ThingsAt(IntVec3 pos)
		{
			SketchThing val;
			if (this.thingsAt_single.TryGetValue(pos, out val))
			{
				return Gen.YieldSingle<SketchThing>(val);
			}
			List<SketchThing> result;
			if (this.thingsAt_multiple.TryGetValue(pos, out result))
			{
				return result;
			}
			return Sketch.EmptySketchThingList;
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x00156AB0 File Offset: 0x00154CB0
		public void ThingsAt(IntVec3 pos, out SketchThing singleResult, out List<SketchThing> multipleResults)
		{
			SketchThing sketchThing;
			if (this.thingsAt_single.TryGetValue(pos, out sketchThing))
			{
				singleResult = sketchThing;
				multipleResults = null;
				return;
			}
			List<SketchThing> list;
			if (this.thingsAt_multiple.TryGetValue(pos, out list))
			{
				singleResult = null;
				multipleResults = list;
				return;
			}
			singleResult = null;
			multipleResults = null;
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x00156AF4 File Offset: 0x00154CF4
		public SketchThing EdificeAt(IntVec3 pos)
		{
			SketchThing result;
			if (this.edificeAt.TryGetValue(pos, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x00156B14 File Offset: 0x00154D14
		public bool WouldCollide(SketchEntity entity)
		{
			if (this.entities.Contains(entity))
			{
				return false;
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				return this.WouldCollide(sketchThing.def, sketchThing.pos, sketchThing.rot);
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			return sketchTerrain != null && this.WouldCollide(sketchTerrain.def, sketchTerrain.pos);
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x00156B74 File Offset: 0x00154D74
		public bool WouldCollide(ThingDef def, IntVec3 pos, Rot4 rot)
		{
			CellRect cellRect = GenAdj.OccupiedRect(pos, rot, def.size);
			if (def.terrainAffordanceNeeded != null)
			{
				foreach (IntVec3 pos2 in cellRect)
				{
					TerrainDef terrainDef = this.TerrainAt(pos2);
					if (terrainDef != null && !terrainDef.affordances.Contains(def.terrainAffordanceNeeded))
					{
						return true;
					}
				}
			}
			for (int i = 0; i < this.cachedThings.Count; i++)
			{
				if (cellRect.Overlaps(this.cachedThings[i].OccupiedRect))
				{
					if (def.race != null)
					{
						if (this.cachedThings[i].def.passability == Traversability.Impassable)
						{
							return true;
						}
					}
					else if (!GenConstruct.CanPlaceBlueprintOver(def, this.cachedThings[i].def))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x00156C74 File Offset: 0x00154E74
		public bool WouldCollide(TerrainDef def, IntVec3 pos)
		{
			if (!def.layerable && this.TerrainAt(pos) != null)
			{
				return true;
			}
			for (int i = 0; i < this.cachedThings.Count; i++)
			{
				if (this.cachedThings[i].OccupiedRect.Contains(pos) && this.cachedThings[i].def.terrainAffordanceNeeded != null && !def.affordances.Contains(this.cachedThings[i].def.terrainAffordanceNeeded))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x00156D04 File Offset: 0x00154F04
		public void WipeColliding(SketchEntity entity)
		{
			if (!this.WouldCollide(entity))
			{
				return;
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				this.WipeColliding(sketchThing.def, sketchThing.pos, sketchThing.rot);
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null)
			{
				this.WipeColliding(sketchTerrain.def, sketchTerrain.pos);
			}
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x00156D5C File Offset: 0x00154F5C
		public void WipeColliding(ThingDef def, IntVec3 pos, Rot4 rot)
		{
			if (!this.WouldCollide(def, pos, rot))
			{
				return;
			}
			CellRect cellRect = GenAdj.OccupiedRect(pos, rot, def.size);
			if (def.terrainAffordanceNeeded != null)
			{
				foreach (IntVec3 intVec in cellRect)
				{
					TerrainDef terrainDef = this.TerrainAt(intVec);
					if (terrainDef != null && !terrainDef.affordances.Contains(def.terrainAffordanceNeeded))
					{
						this.RemoveTerrain(intVec);
					}
				}
			}
			for (int i = this.cachedThings.Count - 1; i >= 0; i--)
			{
				if (cellRect.Overlaps(this.cachedThings[i].OccupiedRect) && !GenConstruct.CanPlaceBlueprintOver(def, this.cachedThings[i].def))
				{
					this.Remove(this.cachedThings[i]);
				}
			}
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x00156E54 File Offset: 0x00155054
		public void WipeColliding(TerrainDef def, IntVec3 pos)
		{
			if (!this.WouldCollide(def, pos))
			{
				return;
			}
			if (!def.layerable && this.TerrainAt(pos) != null)
			{
				this.RemoveTerrain(pos);
			}
			for (int i = this.cachedThings.Count - 1; i >= 0; i--)
			{
				if (this.cachedThings[i].OccupiedRect.Contains(pos) && this.cachedThings[i].def.terrainAffordanceNeeded != null && !def.affordances.Contains(this.cachedThings[i].def.terrainAffordanceNeeded))
				{
					this.Remove(this.cachedThings[i]);
				}
			}
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x00156F08 File Offset: 0x00155108
		public bool IsSpawningBlocked(Map map, IntVec3 pos, Faction faction, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			for (int i = 0; i < this.entities.Count; i++)
			{
				if (this.entities[i].IsSpawningBlocked(this.entities[i].pos + offset, map, null, false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x00156F68 File Offset: 0x00155168
		public bool AnyThingOutOfBounds(Map map, IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			for (int i = 0; i < this.entities.Count; i++)
			{
				SketchThing sketchThing = this.entities[i] as SketchThing;
				if (sketchThing != null)
				{
					if (sketchThing.def.size == IntVec2.One)
					{
						if (!(this.entities[i].pos + offset).InBounds(map))
						{
							return true;
						}
					}
					else
					{
						using (CellRect.Enumerator enumerator = sketchThing.OccupiedRect.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (!(enumerator.Current + offset).InBounds(map))
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x00157040 File Offset: 0x00155240
		public void Spawn(Map map, IntVec3 pos, Faction faction, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, bool clearEdificeWhereFloor = false, List<Thing> spawnedThings = null, bool dormant = false, bool buildRoofsInstantly = false, Func<SketchEntity, IntVec3, bool> canSpawnThing = null, Action<IntVec3, SketchEntity> onFailedToSpawnThing = null)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			if (clearEdificeWhereFloor)
			{
				for (int i = 0; i < this.cachedTerrain.Count; i++)
				{
					if (this.cachedTerrain[i].def.layerable)
					{
						Building edifice = (this.cachedTerrain[i].pos + offset).GetEdifice(map);
						if (edifice != null)
						{
							edifice.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
			foreach (SketchEntity sketchEntity in from x in this.entities
			orderby x.SpawnOrder
			select x)
			{
				IntVec3 intVec = sketchEntity.pos + offset;
				if ((canSpawnThing == null || canSpawnThing(sketchEntity, intVec)) && !sketchEntity.Spawn(intVec, map, faction, spawnMode, wipeIfCollides, spawnedThings, dormant) && onFailedToSpawnThing != null)
				{
					onFailedToSpawnThing(intVec, sketchEntity);
				}
			}
			if (spawnedThings != null && spawnMode == Sketch.SpawnMode.TransportPod && !wipeIfCollides)
			{
				bool flag = false;
				for (int j = 0; j < spawnedThings.Count; j++)
				{
					for (int k = j + 1; k < spawnedThings.Count; k++)
					{
						CellRect cellRect = GenAdj.OccupiedRect(spawnedThings[j].Position, spawnedThings[j].Rotation, spawnedThings[j].def.size);
						CellRect other = GenAdj.OccupiedRect(spawnedThings[k].Position, spawnedThings[k].Rotation, spawnedThings[k].def.size);
						if (cellRect.Overlaps(other) && (GenSpawn.SpawningWipes(spawnedThings[j].def, spawnedThings[k].def) || GenSpawn.SpawningWipes(spawnedThings[k].def, spawnedThings[j].def)))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					for (int l = 0; l < spawnedThings.Count; l++)
					{
						ActiveDropPodInfo activeDropPodInfo;
						if ((activeDropPodInfo = (spawnedThings[l].ParentHolder as ActiveDropPodInfo)) != null)
						{
							activeDropPodInfo.spawnWipeMode = null;
						}
					}
				}
			}
			if (buildRoofsInstantly && spawnMode == Sketch.SpawnMode.Normal)
			{
				foreach (IntVec3 a in this.GetSuggestedRoofCells())
				{
					IntVec3 c = a + offset;
					if (c.InBounds(map) && !c.Roofed(map))
					{
						map.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
					}
				}
			}
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x0015731C File Offset: 0x0015551C
		public void Merge(Sketch other, bool wipeIfCollides = true)
		{
			foreach (SketchEntity sketchEntity in from x in other.entities
			orderby x.SpawnOrder
			select x)
			{
				this.Add(sketchEntity.DeepCopy(), wipeIfCollides);
			}
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x00157394 File Offset: 0x00155594
		public void MergeAt(Sketch other, IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, bool wipeIfCollides = true)
		{
			Sketch sketch = other.DeepCopy();
			sketch.MoveBy(sketch.GetOffset(pos, posType));
			this.Merge(sketch, wipeIfCollides);
		}

		// Token: 0x06004040 RID: 16448 RVA: 0x001573C0 File Offset: 0x001555C0
		public void Subtract(Sketch other)
		{
			for (int i = 0; i < this.entities.Count; i++)
			{
				for (int j = 0; j < other.entities.Count; j++)
				{
					if (this.entities[i].SameForSubtracting(other.entities[j]))
					{
						this.Remove(this.entities[i]);
						i--;
						break;
					}
				}
			}
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x00157431 File Offset: 0x00155631
		public void MoveBy(IntVec2 by)
		{
			this.MoveBy(by.ToIntVec3);
		}

		// Token: 0x06004042 RID: 16450 RVA: 0x00157440 File Offset: 0x00155640
		public void MoveBy(IntVec3 by)
		{
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				sketchEntity.pos += by;
			}
			this.RecacheAll();
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x001574A4 File Offset: 0x001556A4
		public void MoveOccupiedCenterToZero()
		{
			this.MoveBy(new IntVec3(-this.OccupiedCenter.x, 0, -this.OccupiedCenter.z));
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x001574CC File Offset: 0x001556CC
		public Sketch DeepCopy()
		{
			Sketch sketch = new Sketch();
			foreach (SketchEntity sketchEntity in from x in this.entities
			orderby x.SpawnOrder
			select x)
			{
				sketch.Add(sketchEntity.DeepCopy(), true);
			}
			return sketch;
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x0015754C File Offset: 0x0015574C
		private void AddToCache(SketchEntity entity)
		{
			this.occupiedRectDirty = true;
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable != null)
			{
				this.cachedBuildables.Add(sketchBuildable);
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				if (sketchThing.def.building != null && sketchThing.def.building.isEdifice)
				{
					foreach (IntVec3 key in sketchThing.OccupiedRect)
					{
						this.edificeAt[key] = sketchThing;
					}
				}
				foreach (IntVec3 key2 in sketchThing.OccupiedRect)
				{
					List<SketchThing> list;
					SketchThing item;
					if (this.thingsAt_multiple.TryGetValue(key2, out list))
					{
						list.Add(sketchThing);
					}
					else if (this.thingsAt_single.TryGetValue(key2, out item))
					{
						this.thingsAt_single.Remove(key2);
						List<SketchThing> list2 = new List<SketchThing>();
						list2.Add(item);
						list2.Add(sketchThing);
						this.thingsAt_multiple.Add(key2, list2);
					}
					else
					{
						this.thingsAt_single.Add(key2, sketchThing);
					}
				}
				this.cachedThings.Add(sketchThing);
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null)
			{
				this.terrainAt[sketchTerrain.pos] = sketchTerrain;
				this.cachedTerrain.Add(sketchTerrain);
			}
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x001576E0 File Offset: 0x001558E0
		private void RemoveFromCache(SketchEntity entity)
		{
			this.occupiedRectDirty = true;
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable != null)
			{
				this.cachedBuildables.Remove(sketchBuildable);
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				if (sketchThing.def.building != null && sketchThing.def.building.isEdifice)
				{
					foreach (IntVec3 key in sketchThing.OccupiedRect)
					{
						SketchThing sketchThing2;
						if (this.edificeAt.TryGetValue(key, out sketchThing2) && sketchThing2 == sketchThing)
						{
							this.edificeAt.Remove(key);
						}
					}
				}
				foreach (IntVec3 key2 in sketchThing.OccupiedRect)
				{
					List<SketchThing> list;
					SketchThing sketchThing3;
					if (this.thingsAt_multiple.TryGetValue(key2, out list))
					{
						list.Remove(sketchThing);
					}
					else if (this.thingsAt_single.TryGetValue(key2, out sketchThing3) && sketchThing3 == sketchThing)
					{
						this.thingsAt_single.Remove(key2);
					}
				}
				this.cachedThings.Remove(sketchThing);
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null)
			{
				SketchTerrain sketchTerrain2;
				if (this.terrainAt.TryGetValue(sketchTerrain.pos, out sketchTerrain2) && sketchTerrain2 == sketchTerrain)
				{
					this.terrainAt.Remove(sketchTerrain.pos);
				}
				this.cachedTerrain.Remove(sketchTerrain);
			}
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x00157874 File Offset: 0x00155A74
		private void Recache(SketchEntity entity)
		{
			this.RemoveFromCache(entity);
			this.AddToCache(entity);
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x00157884 File Offset: 0x00155A84
		public void RecacheAll()
		{
			this.terrainAt.Clear();
			this.edificeAt.Clear();
			this.thingsAt_single.Clear();
			this.cachedThings.Clear();
			this.cachedTerrain.Clear();
			this.cachedBuildables.Clear();
			this.occupiedRectDirty = true;
			foreach (KeyValuePair<IntVec3, List<SketchThing>> keyValuePair in this.thingsAt_multiple)
			{
				keyValuePair.Value.Clear();
			}
			foreach (SketchEntity entity in from x in this.entities
			orderby x.SpawnOrder
			select x)
			{
				this.AddToCache(entity);
			}
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x00157988 File Offset: 0x00155B88
		public bool LineOfSight(IntVec3 start, IntVec3 end, bool skipFirstCell = false, Func<IntVec3, bool> validator = null)
		{
			foreach (IntVec3 intVec in GenSight.PointsOnLineOfSight(start, end))
			{
				if (!skipFirstCell || !(intVec == start))
				{
					if (!this.CanBeSeenOver(intVec))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x00157A00 File Offset: 0x00155C00
		public bool LineOfSight(IntVec3 start, IntVec3 end, CellRect startRect, CellRect endRect, Func<IntVec3, bool> validator = null)
		{
			foreach (IntVec3 intVec in GenSight.PointsOnLineOfSight(start, end))
			{
				if (endRect.Contains(intVec))
				{
					return true;
				}
				if (!startRect.Contains(intVec))
				{
					if (!this.CanBeSeenOver(intVec))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x00157A84 File Offset: 0x00155C84
		public bool CanBeSeenOver(IntVec3 c)
		{
			SketchThing sketchThing = this.EdificeAt(c);
			return sketchThing == null || sketchThing.def.Fillage != FillCategory.Full;
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x00157AAF File Offset: 0x00155CAF
		public bool Passable(int x, int z)
		{
			return this.Passable(new IntVec3(x, 0, z));
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x00157AC0 File Offset: 0x00155CC0
		public bool Passable(IntVec3 pos)
		{
			TerrainDef terrainDef = this.TerrainAt(pos);
			if (terrainDef != null && terrainDef.passability == Traversability.Impassable)
			{
				return false;
			}
			using (IEnumerator<SketchThing> enumerator = this.ThingsAt(pos).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.passability == Traversability.Impassable)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x00157B30 File Offset: 0x00155D30
		public void DrawGhost(IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, bool placingMode = false, Thing thingToIgnore = null)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			Map currentMap = Find.CurrentMap;
			bool flag = false;
			using (List<SketchEntity>.Enumerator enumerator = this.Entities.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.OccupiedRect.MovedBy(offset).InBounds(currentMap))
					{
						flag = true;
						break;
					}
				}
			}
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				if ((placingMode || !sketchEntity.IsSameSpawnedOrBlueprintOrFrame(sketchEntity.pos + offset, currentMap)) && sketchEntity.OccupiedRect.MovedBy(offset).InBounds(currentMap))
				{
					Color color = (flag || (sketchEntity.IsSpawningBlocked(sketchEntity.pos + offset, currentMap, thingToIgnore, false) && !sketchEntity.IsSameSpawnedOrBlueprintOrFrame(sketchEntity.pos + offset, currentMap))) ? Sketch.BlockedColor : Sketch.GhostColor;
					sketchEntity.DrawGhost(sketchEntity.pos + offset, color);
				}
			}
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x00157C7C File Offset: 0x00155E7C
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Func<IntVec3, int, bool> processor, int maxCellsToProcess = 2147483647, CellRect? bounds = null, IEnumerable<IntVec3> extraRoots = null)
		{
			if (this.floodFillWorking)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.", false);
			}
			this.floodFillWorking = true;
			if (this.floodFillOpenSet == null)
			{
				this.floodFillOpenSet = new Queue<IntVec3>();
			}
			if (this.floodFillTraversalDistance == null)
			{
				this.floodFillTraversalDistance = new Dictionary<IntVec3, int>();
			}
			this.floodFillTraversalDistance.Clear();
			this.floodFillOpenSet.Clear();
			if (root.IsValid && extraRoots == null && !passCheck(root))
			{
				this.floodFillWorking = false;
				return;
			}
			if (bounds == null)
			{
				bounds = new CellRect?(this.OccupiedRect);
			}
			int area = bounds.Value.Area;
			IntVec3[] cardinalDirectionsAround = GenAdj.CardinalDirectionsAround;
			int num = cardinalDirectionsAround.Length;
			int num2 = 0;
			if (root.IsValid)
			{
				this.floodFillTraversalDistance.Add(root, 0);
				this.floodFillOpenSet.Enqueue(root);
			}
			if (extraRoots == null)
			{
				goto IL_23E;
			}
			IList<IntVec3> list = extraRoots as IList<IntVec3>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.floodFillTraversalDistance.SetOrAdd(list[i], 0);
					this.floodFillOpenSet.Enqueue(list[i]);
				}
				goto IL_23E;
			}
			using (IEnumerator<IntVec3> enumerator = extraRoots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 intVec = enumerator.Current;
					this.floodFillTraversalDistance.SetOrAdd(intVec, 0);
					this.floodFillOpenSet.Enqueue(intVec);
				}
				goto IL_23E;
			}
			IL_16A:
			IntVec3 intVec2 = this.floodFillOpenSet.Dequeue();
			int num3 = this.floodFillTraversalDistance[intVec2];
			if (processor(intVec2, num3))
			{
				goto IL_24F;
			}
			num2++;
			if (num2 == maxCellsToProcess)
			{
				goto IL_24F;
			}
			for (int j = 0; j < num; j++)
			{
				IntVec3 intVec3 = intVec2 + cardinalDirectionsAround[j];
				if (bounds.Value.Contains(intVec3) && !this.floodFillTraversalDistance.ContainsKey(intVec3) && passCheck(intVec3))
				{
					this.floodFillOpenSet.Enqueue(intVec3);
					this.floodFillTraversalDistance.Add(intVec3, num3 + 1);
				}
			}
			if (this.floodFillOpenSet.Count > area)
			{
				Log.Error("Overflow on flood fill (>" + area + " cells). Make sure we're not flooding over the same area after we check it.", false);
				this.floodFillWorking = false;
				return;
			}
			IL_23E:
			if (this.floodFillOpenSet.Count > 0)
			{
				goto IL_16A;
			}
			IL_24F:
			this.floodFillWorking = false;
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x00157EF0 File Offset: 0x001560F0
		private IEnumerable<IntVec3> GetSuggestedRoofCells()
		{
			if (this.Empty)
			{
				yield break;
			}
			CellRect occupiedRect = this.OccupiedRect;
			Sketch.tmpSuggestedRoofCellsVisited.Clear();
			Sketch.tmpYieldedSuggestedRoofCells.Clear();
			foreach (IntVec3 intVec in this.OccupiedRect)
			{
				if (!Sketch.tmpSuggestedRoofCellsVisited.Contains(intVec) && !this.<GetSuggestedRoofCells>g__AnyRoofHolderAt|80_0(intVec))
				{
					Sketch.tmpSuggestedRoofCells.Clear();
					this.FloodFill(intVec, (IntVec3 c) => !this.<GetSuggestedRoofCells>g__AnyRoofHolderAt|80_0(c), delegate(IntVec3 c, int dist)
					{
						Sketch.tmpSuggestedRoofCellsVisited.Add(c);
						Sketch.tmpSuggestedRoofCells.Add(c);
						return false;
					}, int.MaxValue, null, null);
					bool flag = false;
					for (int k = 0; k < Sketch.tmpSuggestedRoofCells.Count; k++)
					{
						if (occupiedRect.IsOnEdge(Sketch.tmpSuggestedRoofCells[k]))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						int num;
						for (int i = 0; i < Sketch.tmpSuggestedRoofCells.Count; i = num + 1)
						{
							for (int j = 0; j < 9; j = num + 1)
							{
								IntVec3 intVec2 = Sketch.tmpSuggestedRoofCells[i] + GenAdj.AdjacentCellsAndInside[j];
								if (!Sketch.tmpYieldedSuggestedRoofCells.Contains(intVec2) && occupiedRect.Contains(intVec2) && (j == 8 || this.<GetSuggestedRoofCells>g__AnyRoofHolderAt|80_0(intVec2)))
								{
									Sketch.tmpYieldedSuggestedRoofCells.Add(intVec2);
									yield return intVec2;
								}
								num = j;
							}
							num = i;
						}
					}
				}
			}
			Sketch.tmpSuggestedRoofCellsVisited.Clear();
			Sketch.tmpYieldedSuggestedRoofCells.Clear();
			yield break;
			yield break;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x00157F00 File Offset: 0x00156100
		private IntVec3 GetOffset(IntVec3 pos, Sketch.SpawnPosType posType)
		{
			IntVec3 intVec;
			switch (posType)
			{
			case Sketch.SpawnPosType.Unchanged:
				intVec = IntVec3.Zero;
				break;
			case Sketch.SpawnPosType.OccupiedCenter:
				intVec = new IntVec3(-this.OccupiedCenter.x, 0, -this.OccupiedCenter.z);
				break;
			case Sketch.SpawnPosType.OccupiedBotLeft:
				intVec = new IntVec3(-this.OccupiedRect.minX, 0, -this.OccupiedRect.minZ);
				break;
			default:
				intVec = default(IntVec3);
				break;
			}
			intVec += pos;
			return intVec;
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x00157F80 File Offset: 0x00156180
		public void Rotate(Rot4 rot)
		{
			if (rot == this.rotation)
			{
				return;
			}
			int num = rot.AsInt - this.rotation.AsInt;
			if (num < 0)
			{
				num += 4;
			}
			Rot4 rot2 = new Rot4(num);
			this.rotation = rot;
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				sketchEntity.pos = sketchEntity.pos.RotatedBy(rot2);
				SketchThing sketchThing;
				if ((sketchThing = (sketchEntity as SketchThing)) != null)
				{
					RotationDirection rotationDirection = RotationDirection.None;
					if (rot2.AsInt == 1)
					{
						rotationDirection = RotationDirection.Clockwise;
					}
					else if (rot2.AsInt == 3)
					{
						rotationDirection = RotationDirection.Counterclockwise;
					}
					if (sketchThing.def.rotatable)
					{
						sketchThing.rot = sketchThing.rot.Rotated(rotationDirection);
					}
					else if (sketchThing.def.size.z % 2 == 0 && rotationDirection == RotationDirection.Clockwise)
					{
						SketchEntity sketchEntity2 = sketchEntity;
						sketchEntity2.pos.z = sketchEntity2.pos.z - 1;
					}
					else if (sketchThing.def.size.x % 2 == 0 && rotationDirection == RotationDirection.Counterclockwise)
					{
						SketchEntity sketchEntity3 = sketchEntity;
						sketchEntity3.pos.x = sketchEntity3.pos.x - 1;
					}
				}
			}
			this.RecacheAll();
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x001580CC File Offset: 0x001562CC
		public void ExposeData()
		{
			Scribe_Collections.Look<SketchEntity>(ref this.entities, "entities", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<Rot4>(ref this.rotation, "rotation", Rot4.North, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.entities.RemoveAll((SketchEntity x) => x == null) != 0)
				{
					Log.Error("Some sketch entities were null after loading.", false);
				}
				if (this.entities.RemoveAll((SketchEntity x) => x.LostImportantReferences) != 0)
				{
					Log.Error("Some sketch entities had null defs after loading.", false);
				}
				this.RecacheAll();
				Sketch.tmpToRemove.Clear();
				for (int i = 0; i < this.cachedThings.Count; i++)
				{
					if (this.cachedThings[i].def.IsDoor)
					{
						for (int j = 0; j < this.cachedThings.Count; j++)
						{
							if (this.cachedThings[j].def == ThingDefOf.Wall && this.cachedThings[j].pos == this.cachedThings[i].pos)
							{
								Sketch.tmpToRemove.Add(this.cachedThings[j]);
							}
						}
					}
				}
				for (int k = 0; k < Sketch.tmpToRemove.Count; k++)
				{
					Log.Error("Sketch has a wall and a door in the same cell. Fixing.", false);
					this.Remove(Sketch.tmpToRemove[k]);
				}
				Sketch.tmpToRemove.Clear();
			}
		}

		// Token: 0x04002551 RID: 9553
		private List<SketchEntity> entities = new List<SketchEntity>();

		// Token: 0x04002552 RID: 9554
		private List<SketchThing> cachedThings = new List<SketchThing>();

		// Token: 0x04002553 RID: 9555
		private List<SketchTerrain> cachedTerrain = new List<SketchTerrain>();

		// Token: 0x04002554 RID: 9556
		private List<SketchBuildable> cachedBuildables = new List<SketchBuildable>();

		// Token: 0x04002555 RID: 9557
		private Dictionary<IntVec3, SketchTerrain> terrainAt = new Dictionary<IntVec3, SketchTerrain>();

		// Token: 0x04002556 RID: 9558
		private Dictionary<IntVec3, SketchThing> edificeAt = new Dictionary<IntVec3, SketchThing>();

		// Token: 0x04002557 RID: 9559
		private Dictionary<IntVec3, SketchThing> thingsAt_single = new Dictionary<IntVec3, SketchThing>();

		// Token: 0x04002558 RID: 9560
		private Dictionary<IntVec3, List<SketchThing>> thingsAt_multiple = new Dictionary<IntVec3, List<SketchThing>>();

		// Token: 0x04002559 RID: 9561
		private bool occupiedRectDirty = true;

		// Token: 0x0400255A RID: 9562
		private CellRect cachedOccupiedRect;

		// Token: 0x0400255B RID: 9563
		private Rot4 rotation = Rot4.North;

		// Token: 0x0400255C RID: 9564
		private bool floodFillWorking;

		// Token: 0x0400255D RID: 9565
		private Queue<IntVec3> floodFillOpenSet;

		// Token: 0x0400255E RID: 9566
		private Dictionary<IntVec3, int> floodFillTraversalDistance;

		// Token: 0x0400255F RID: 9567
		public const float SpawnOrder_Terrain = 1f;

		// Token: 0x04002560 RID: 9568
		public const float SpawnOrder_Thing = 2f;

		// Token: 0x04002561 RID: 9569
		private static readonly List<SketchThing> EmptySketchThingList = new List<SketchThing>();

		// Token: 0x04002562 RID: 9570
		private static readonly Color GhostColor = new Color(0.7f, 0.7f, 0.7f, 0.35f);

		// Token: 0x04002563 RID: 9571
		private static readonly Color BlockedColor = new Color(0.8f, 0.2f, 0.2f, 0.35f);

		// Token: 0x04002564 RID: 9572
		private static HashSet<IntVec3> tmpSuggestedRoofCellsVisited = new HashSet<IntVec3>();

		// Token: 0x04002565 RID: 9573
		private static List<IntVec3> tmpSuggestedRoofCells = new List<IntVec3>();

		// Token: 0x04002566 RID: 9574
		private static HashSet<IntVec3> tmpYieldedSuggestedRoofCells = new HashSet<IntVec3>();

		// Token: 0x04002567 RID: 9575
		private static List<SketchThing> tmpToRemove = new List<SketchThing>();

		// Token: 0x02001A80 RID: 6784
		public enum SpawnPosType
		{
			// Token: 0x040064A1 RID: 25761
			Unchanged,
			// Token: 0x040064A2 RID: 25762
			OccupiedCenter,
			// Token: 0x040064A3 RID: 25763
			OccupiedBotLeft
		}

		// Token: 0x02001A81 RID: 6785
		public enum SpawnMode
		{
			// Token: 0x040064A5 RID: 25765
			Blueprint,
			// Token: 0x040064A6 RID: 25766
			Normal,
			// Token: 0x040064A7 RID: 25767
			TransportPod
		}
	}
}
