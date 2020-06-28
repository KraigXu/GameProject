using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA7 RID: 2727
	public class SketchThing : SketchBuildable
	{
		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06004081 RID: 16513 RVA: 0x00158947 File Offset: 0x00156B47
		public override BuildableDef Buildable
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06004082 RID: 16514 RVA: 0x0015894F File Offset: 0x00156B4F
		public override ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06004083 RID: 16515 RVA: 0x00158957 File Offset: 0x00156B57
		public override string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.def, this.stuff, this.stackCount);
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06004084 RID: 16516 RVA: 0x00158970 File Offset: 0x00156B70
		public override CellRect OccupiedRect
		{
			get
			{
				return GenAdj.OccupiedRect(this.pos, this.rot, this.def.size);
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06004085 RID: 16517 RVA: 0x0015898E File Offset: 0x00156B8E
		public override float SpawnOrder
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06004086 RID: 16518 RVA: 0x00158995 File Offset: 0x00156B95
		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.def.GetStatValueAbstract(StatDefOf.MaxHitPoints, this.stuff ?? GenStuff.DefaultStuffFor(this.def)));
			}
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x001589C4 File Offset: 0x00156BC4
		public Thing Instantiate()
		{
			Thing thing = ThingMaker.MakeThing(this.def, this.stuff ?? GenStuff.DefaultStuffFor(this.def));
			thing.stackCount = this.stackCount;
			if (this.quality != null)
			{
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					compQuality.SetQuality(this.quality.Value, ArtGenerationContext.Outsider);
				}
			}
			if (this.hitPoints != null)
			{
				thing.HitPoints = this.hitPoints.Value;
			}
			return thing;
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x00158A46 File Offset: 0x00156C46
		public override void DrawGhost(IntVec3 at, Color color)
		{
			GhostDrawer.DrawGhostThing(at, this.rot, this.def, this.def.graphic, color, AltitudeLayer.Blueprint, null);
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x00158A6C File Offset: 0x00156C6C
		public Thing GetSameSpawned(IntVec3 at, Map map)
		{
			if (!at.InBounds(map))
			{
				return null;
			}
			List<Thing> thingList = at.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				CellRect lhs = GenAdj.OccupiedRect(at, this.rot, thingList[i].def.Size);
				CellRect lhs2 = GenAdj.OccupiedRect(at, this.rot.Opposite, thingList[i].def.Size);
				CellRect rhs = thingList[i].OccupiedRect();
				if ((lhs == rhs || lhs2 == rhs) && thingList[i].def == this.def && (this.stuff == null || thingList[i].Stuff == this.stuff) && (thingList[i].Rotation == this.rot || thingList[i].Rotation == this.rot.Opposite || !this.def.rotatable))
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x00158B7E File Offset: 0x00156D7E
		public override bool IsSameSpawned(IntVec3 at, Map map)
		{
			return this.GetSameSpawned(at, map) != null;
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x00158B8C File Offset: 0x00156D8C
		public override Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map)
		{
			if (!at.InBounds(map))
			{
				return null;
			}
			List<Thing> thingList = at.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				CellRect lhs = GenAdj.OccupiedRect(at, this.rot, thingList[i].def.Size);
				CellRect lhs2 = GenAdj.OccupiedRect(at, this.rot.Opposite, thingList[i].def.Size);
				CellRect rhs = thingList[i].OccupiedRect();
				if ((lhs == rhs || lhs2 == rhs) && thingList[i].def.entityDefToBuild == this.def && (this.stuff == null || ((IConstructible)thingList[i]).EntityToBuildStuff() == this.stuff) && (thingList[i].Rotation == this.rot || thingList[i].Rotation == this.rot.Opposite || !this.def.rotatable))
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x00158CAC File Offset: 0x00156EAC
		public override bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return this.IsSpawningBlockedPermanently(at, map, thingToIgnore, wipeIfCollides) || !at.InBounds(map) || !GenConstruct.CanPlaceBlueprintAt(this.def, at, this.rot, map, wipeIfCollides, thingToIgnore, null, this.stuff ?? GenStuff.DefaultStuffFor(this.def)).Accepted;
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x00158D0C File Offset: 0x00156F0C
		public override bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			if (!at.InBounds(map))
			{
				return true;
			}
			if (!this.CanBuildOnTerrain(at, map))
			{
				return true;
			}
			foreach (IntVec3 c in GenAdj.OccupiedRect(at, this.rot, this.def.Size))
			{
				if (!c.InBounds(map))
				{
					return true;
				}
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!thingList[i].def.destroyable && !GenConstruct.CanPlaceBlueprintOver(this.def, thingList[i].def))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x00158DE8 File Offset: 0x00156FE8
		public override bool CanBuildOnTerrain(IntVec3 at, Map map)
		{
			return GenConstruct.CanBuildOnTerrain(this.def, at, map, this.rot, null, this.stuff ?? GenStuff.DefaultStuffFor(this.def));
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x00158E14 File Offset: 0x00157014
		public override bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, List<Thing> spawnedThings = null, bool dormant = false)
		{
			if (this.IsSpawningBlocked(at, map, null, wipeIfCollides))
			{
				return false;
			}
			switch (spawnMode)
			{
			case Sketch.SpawnMode.Blueprint:
				GenConstruct.PlaceBlueprintForBuild(this.def, at, map, this.rot, faction, this.stuff ?? GenStuff.DefaultStuffFor(this.def));
				break;
			case Sketch.SpawnMode.Normal:
			{
				Thing thing = this.Instantiate();
				if (spawnedThings != null)
				{
					spawnedThings.Add(thing);
				}
				if (faction != null)
				{
					thing.SetFactionDirect(faction);
				}
				this.SetDormant(thing, dormant);
				GenSpawn.Spawn(thing, at, map, this.rot, WipeMode.VanishOrMoveAside, false);
				break;
			}
			case Sketch.SpawnMode.TransportPod:
			{
				Thing thing2 = this.Instantiate();
				thing2.Position = at;
				thing2.Rotation = this.rot;
				if (spawnedThings != null)
				{
					spawnedThings.Add(thing2);
				}
				if (faction != null)
				{
					thing2.SetFactionDirect(faction);
				}
				this.SetDormant(thing2, dormant);
				ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
				activeDropPodInfo.innerContainer.TryAdd(thing2, 1, true);
				activeDropPodInfo.openDelay = 60;
				activeDropPodInfo.leaveSlag = false;
				activeDropPodInfo.despawnPodBeforeSpawningThing = true;
				activeDropPodInfo.spawnWipeMode = (wipeIfCollides ? new WipeMode?(WipeMode.VanishOrMoveAside) : null);
				activeDropPodInfo.moveItemsAsideBeforeSpawning = true;
				activeDropPodInfo.setRotation = new Rot4?(this.rot);
				DropPodUtility.MakeDropPodAt(at, map, activeDropPodInfo);
				break;
			}
			default:
				throw new NotImplementedException("Spawn mode " + spawnMode + " not implemented!");
			}
			return true;
		}

		// Token: 0x06004090 RID: 16528 RVA: 0x00158F74 File Offset: 0x00157174
		private void SetDormant(Thing thing, bool dormant)
		{
			CompCanBeDormant compCanBeDormant = thing.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null)
			{
				if (dormant)
				{
					compCanBeDormant.ToSleep();
					return;
				}
				compCanBeDormant.WakeUp();
			}
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x00158F9C File Offset: 0x0015719C
		public override bool SameForSubtracting(SketchEntity other)
		{
			SketchThing sketchThing = other as SketchThing;
			if (sketchThing == null)
			{
				return false;
			}
			if (sketchThing == this)
			{
				return true;
			}
			if (this.def == sketchThing.def && this.stuff == sketchThing.stuff && this.stackCount == sketchThing.stackCount && this.pos == sketchThing.pos && this.rot == sketchThing.rot)
			{
				QualityCategory? qualityCategory = this.quality;
				QualityCategory? qualityCategory2 = sketchThing.quality;
				if (qualityCategory.GetValueOrDefault() == qualityCategory2.GetValueOrDefault() & qualityCategory != null == (qualityCategory2 != null))
				{
					int? num = this.hitPoints;
					int? num2 = sketchThing.hitPoints;
					return num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null);
				}
			}
			return false;
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x00159078 File Offset: 0x00157278
		public override SketchEntity DeepCopy()
		{
			SketchThing sketchThing = (SketchThing)base.DeepCopy();
			sketchThing.def = this.def;
			sketchThing.stuff = this.stuff;
			sketchThing.stackCount = this.stackCount;
			sketchThing.rot = this.rot;
			sketchThing.quality = this.quality;
			sketchThing.hitPoints = this.hitPoints;
			return sketchThing;
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x001590D8 File Offset: 0x001572D8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
			Scribe_Values.Look<Rot4>(ref this.rot, "rot", default(Rot4), false);
			Scribe_Values.Look<QualityCategory?>(ref this.quality, "quality", null, false);
			Scribe_Values.Look<int?>(ref this.hitPoints, "hitPoints", null, false);
		}

		// Token: 0x0400256C RID: 9580
		public ThingDef def;

		// Token: 0x0400256D RID: 9581
		public ThingDef stuff;

		// Token: 0x0400256E RID: 9582
		public int stackCount;

		// Token: 0x0400256F RID: 9583
		public Rot4 rot;

		// Token: 0x04002570 RID: 9584
		public QualityCategory? quality;

		// Token: 0x04002571 RID: 9585
		public int? hitPoints;
	}
}
