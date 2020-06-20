using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C91 RID: 3217
	[StaticConstructorOnStartup]
	public class MonumentMarker : Thing
	{
		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06004D83 RID: 19843 RVA: 0x001A03AC File Offset: 0x0019E5AC
		public override CellRect? CustomRectForSelector
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				return new CellRect?(this.sketch.OccupiedRect.MovedBy(base.Position));
			}
		}

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06004D84 RID: 19844 RVA: 0x001A03EC File Offset: 0x0019E5EC
		public bool AllDone
		{
			get
			{
				if (!base.Spawned)
				{
					return false;
				}
				foreach (SketchEntity sketchEntity in this.sketch.Entities)
				{
					if (!sketchEntity.IsSameSpawned(base.Position + sketchEntity.pos, base.Map))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06004D85 RID: 19845 RVA: 0x001A0470 File Offset: 0x0019E670
		public IntVec2 Size
		{
			get
			{
				return this.sketch.OccupiedSize;
			}
		}

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06004D86 RID: 19846 RVA: 0x001A0480 File Offset: 0x0019E680
		public Thing FirstDisallowedBuilding
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				List<SketchTerrain> terrain = this.sketch.Terrain;
				for (int i = 0; i < terrain.Count; i++)
				{
					MonumentMarker.tmpAllowedBuildings.Clear();
					SketchThing sketchThing;
					List<SketchThing> list;
					this.sketch.ThingsAt(terrain[i].pos, out sketchThing, out list);
					if (sketchThing != null)
					{
						MonumentMarker.tmpAllowedBuildings.Add(sketchThing.def);
					}
					if (list != null)
					{
						for (int j = 0; j < list.Count; j++)
						{
							MonumentMarker.tmpAllowedBuildings.Add(list[j].def);
						}
					}
					List<Thing> thingList = (terrain[i].pos + base.Position).GetThingList(base.Map);
					for (int k = 0; k < thingList.Count; k++)
					{
						if (thingList[k].def.IsBuildingArtificial && !thingList[k].def.IsBlueprint && !thingList[k].def.IsFrame && !MonumentMarker.tmpAllowedBuildings.Contains(thingList[k].def))
						{
							return thingList[k];
						}
					}
				}
				return null;
			}
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06004D87 RID: 19847 RVA: 0x001A05BF File Offset: 0x0019E7BF
		public bool AnyDisallowedBuilding
		{
			get
			{
				return this.FirstDisallowedBuilding != null;
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06004D88 RID: 19848 RVA: 0x001A05CC File Offset: 0x0019E7CC
		public SketchEntity FirstEntityWithMissingBlueprint
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				foreach (SketchEntity sketchEntity in this.sketch.Entities)
				{
					if (!sketchEntity.IsSameSpawnedOrBlueprintOrFrame(base.Position + sketchEntity.pos, base.Map))
					{
						return sketchEntity;
					}
				}
				return null;
			}
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x001A0650 File Offset: 0x0019E850
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.sketch.Rotate(base.Rotation);
			}
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Monuments are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 774341, false);
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06004D8A RID: 19850 RVA: 0x001A068C File Offset: 0x0019E88C
		public override void Tick()
		{
			if (this.IsHashIntervalTick(177))
			{
				bool allDone = this.AllDone;
				if (!this.complete && allDone)
				{
					this.complete = true;
					QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentCompleted", this.Named("SUBJECT"));
				}
				if (this.complete && !allDone)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentDestroyed", this.Named("SUBJECT"));
					if (!base.Destroyed)
					{
						this.Destroy(DestroyMode.Vanish);
					}
					return;
				}
				if (allDone)
				{
					if (this.AnyDisallowedBuilding)
					{
						this.ticksSinceDisallowedBuilding += 177;
						if (this.ticksSinceDisallowedBuilding >= 60000)
						{
							Messages.Message("MessageMonumentDestroyedBecauseOfDisallowedBuilding".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.NegativeEvent, true);
							this.Destroy(DestroyMode.Vanish);
							return;
						}
					}
					else
					{
						this.ticksSinceDisallowedBuilding = 0;
					}
				}
			}
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x001A0780 File Offset: 0x0019E980
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Sketch>(ref this.sketch, "sketch", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksSinceDisallowedBuilding, "ticksSinceDisallowedBuilding", 0, false);
			Scribe_Values.Look<bool>(ref this.complete, "complete", false, false);
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x001A07CC File Offset: 0x0019E9CC
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.DrawGhost_NewTmp(drawLoc.ToIntVec3(), false, base.Rotation);
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x001A07E1 File Offset: 0x0019E9E1
		[Obsolete]
		public void DrawGhost(IntVec3 at, bool placingMode)
		{
			this.DrawGhost_NewTmp(at, placingMode, base.Rotation);
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x001A07F4 File Offset: 0x0019E9F4
		public void DrawGhost_NewTmp(IntVec3 at, bool placingMode, Rot4 rotation)
		{
			CellRect rect = this.sketch.OccupiedRect.MovedBy(at);
			Blueprint_Install thingToIgnore = this.FindMyBlueprint(rect, Find.CurrentMap);
			this.sketch.Rotate(rotation);
			this.sketch.DrawGhost(at, Sketch.SpawnPosType.Unchanged, placingMode, thingToIgnore);
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x001A0840 File Offset: 0x0019EA40
		public Blueprint_Install FindMyBlueprint(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect)
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Blueprint_Install blueprint_Install = thingList[i] as Blueprint_Install;
						if (blueprint_Install != null && blueprint_Install.ThingToInstall == this)
						{
							return blueprint_Install;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x001A08D0 File Offset: 0x0019EAD0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (!this.AllDone)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandCancelMonumentMarker".Translate(),
					defaultDesc = "CommandCancelMonumentMarkerDesc".Translate(),
					icon = MonumentMarker.CancelCommandTex,
					action = delegate
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmCancelMonumentMarker".Translate(), delegate
						{
							QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentCancelled", this.Named("SUBJECT"));
							this.RemovePossiblyRelatedBlueprints();
							this.Uninstall();
						}, true, null));
					}
				};
			}
			bool flag = false;
			foreach (SketchEntity sketchEntity in this.sketch.Entities)
			{
				SketchBuildable sketchBuildable = sketchEntity as SketchBuildable;
				if (sketchBuildable != null && !sketchEntity.IsSameSpawnedOrBlueprintOrFrame(sketchEntity.pos + base.Position, base.Map) && !sketchEntity.IsSpawningBlocked(sketchEntity.pos + base.Position, base.Map, null, false) && BuildCopyCommandUtility.FindAllowedDesignator(sketchBuildable.Buildable, true) != null)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandPlaceBlueprints".Translate(),
					defaultDesc = "CommandPlaceBlueprintsDesc".Translate(),
					icon = MonumentMarker.PlaceBlueprintsCommandTex,
					action = delegate
					{
						IEnumerable<ThingDef> enumerable = this.AllowedStuffs();
						if (!enumerable.Any<ThingDef>())
						{
							this.PlaceAllBlueprints(null);
							SoundDefOf.Click.PlayOneShotOnCamera(null);
							return;
						}
						if (enumerable.Count<ThingDef>() == 1)
						{
							this.PlaceAllBlueprints(enumerable.First<ThingDef>());
							SoundDefOf.Click.PlayOneShotOnCamera(null);
							return;
						}
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						bool flag4 = false;
						foreach (ThingDef def in enumerable)
						{
							if (base.Map.listerThings.ThingsOfDef(def).Count > 0)
							{
								flag4 = true;
								break;
							}
						}
						foreach (ThingDef thingDef in enumerable)
						{
							if (!flag4 || base.Map.listerThings.ThingsOfDef(thingDef).Count != 0)
							{
								ThingDef stuffLocal = thingDef;
								list.Add(new FloatMenuOption(stuffLocal.LabelCap, delegate
								{
									this.PlaceAllBlueprints(stuffLocal);
								}, thingDef, MenuOptionPriority.Default, null, null, 0f, null, null));
							}
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
			}
			foreach (Gizmo gizmo2 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			if (Prefs.DevMode)
			{
				bool flag2 = false;
				foreach (SketchEntity sketchEntity2 in this.sketch.Entities)
				{
					if (!sketchEntity2.IsSameSpawned(sketchEntity2.pos + base.Position, base.Map) && !sketchEntity2.IsSpawningBlocked(sketchEntity2.pos + base.Position, base.Map, null, false))
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					yield return new Command_Action
					{
						defaultLabel = "Dev: Build all",
						action = delegate
						{
							this.DebugBuildAll();
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
					};
				}
			}
			MonumentMarker.tmpUniqueBuildableDefs.Clear();
			foreach (SketchEntity sketchEntity3 in this.sketch.Entities)
			{
				SketchBuildable sketchBuildable2 = sketchEntity3 as SketchBuildable;
				if (sketchBuildable2 != null && !sketchEntity3.IsSameSpawnedOrBlueprintOrFrame(sketchEntity3.pos + base.Position, base.Map) && MonumentMarker.tmpUniqueBuildableDefs.Add(new Pair<BuildableDef, ThingDef>(sketchBuildable2.Buildable, sketchBuildable2.Stuff)))
				{
					SketchTerrain sketchTerrain;
					if ((sketchTerrain = (sketchBuildable2 as SketchTerrain)) != null && sketchTerrain.treatSimilarAsSame)
					{
						TerrainDef terrain = sketchBuildable2.Buildable as TerrainDef;
						if (terrain.designatorDropdown != null)
						{
							Designator designator = BuildCopyCommandUtility.FindAllowedDesignatorRoot(sketchBuildable2.Buildable, true);
							if (designator != null)
							{
								yield return designator;
							}
						}
						else
						{
							IEnumerable<TerrainDef> allDefs = DefDatabase<TerrainDef>.AllDefs;
							foreach (TerrainDef terrainDef in allDefs)
							{
								if (terrainDef.BuildableByPlayer && terrainDef.designatorDropdown == null)
								{
									bool flag3 = true;
									for (int i = 0; i < terrain.affordances.Count; i++)
									{
										if (!terrainDef.affordances.Contains(terrain.affordances[i]))
										{
											flag3 = false;
											break;
										}
									}
									if (flag3)
									{
										Command command = BuildCopyCommandUtility.BuildCommand(terrainDef, null, terrainDef.label, terrainDef.description, false);
										if (command != null)
										{
											yield return command;
										}
									}
								}
							}
							IEnumerator<TerrainDef> enumerator4 = null;
						}
						terrain = null;
					}
					else
					{
						Command command2 = BuildCopyCommandUtility.BuildCommand(sketchBuildable2.Buildable, sketchBuildable2.Stuff, sketchEntity3.Label, sketchBuildable2.Buildable.description, false);
						if (command2 != null)
						{
							yield return command2;
						}
					}
				}
			}
			List<SketchEntity>.Enumerator enumerator3 = default(List<SketchEntity>.Enumerator);
			MonumentMarker.tmpUniqueBuildableDefs.Clear();
			yield break;
			yield break;
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x001A08E0 File Offset: 0x0019EAE0
		public void DebugBuildAll()
		{
			this.sketch.Spawn(base.Map, base.Position, Faction.OfPlayer, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, null, false, false, null, null);
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x001A0914 File Offset: 0x0019EB14
		private void PlaceAllBlueprints(ThingDef preferredStuffIfNone)
		{
			foreach (SketchEntity sketchEntity in this.sketch.Entities)
			{
				SketchBuildable sketchBuildable = sketchEntity as SketchBuildable;
				if (sketchBuildable != null && !sketchEntity.IsSameSpawnedOrBlueprintOrFrame(sketchEntity.pos + base.Position, base.Map) && !sketchEntity.IsSpawningBlocked(sketchEntity.pos + base.Position, base.Map, null, false) && BuildCopyCommandUtility.FindAllowedDesignator(sketchBuildable.Buildable, true) != null)
				{
					SketchThing sketchThing;
					SketchTerrain sketchTerrain;
					if ((sketchThing = (sketchEntity as SketchThing)) != null && sketchThing.def.MadeFromStuff && sketchThing.stuff == null && preferredStuffIfNone != null && preferredStuffIfNone.stuffProps.CanMake(sketchThing.def))
					{
						sketchThing.stuff = preferredStuffIfNone;
						sketchEntity.Spawn(sketchEntity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint, false, null, false);
						sketchThing.stuff = null;
					}
					else if ((sketchTerrain = (sketchEntity as SketchTerrain)) != null && sketchTerrain.stuffForComparingSimilar == null && preferredStuffIfNone != null)
					{
						sketchTerrain.stuffForComparingSimilar = preferredStuffIfNone;
						sketchEntity.Spawn(sketchEntity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint, false, null, false);
						sketchTerrain.stuffForComparingSimilar = null;
					}
					else
					{
						sketchEntity.Spawn(sketchEntity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint, false, null, false);
					}
				}
			}
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x001A0AC0 File Offset: 0x0019ECC0
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Quest quest = Find.QuestManager.QuestsListForReading.FirstOrDefault((Quest q) => q.QuestLookTargets.Contains(this));
			if (quest != null)
			{
				stringBuilder.Append("Quest".Translate() + ": " + quest.name);
			}
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			if (base.Spawned && !this.AllDone)
			{
				MonumentMarker.tmpBuiltParts.Clear();
				foreach (SketchEntity sketchEntity in this.sketch.Entities)
				{
					Pair<int, int> value;
					if (!MonumentMarker.tmpBuiltParts.TryGetValue(sketchEntity.Label, out value))
					{
						value = default(Pair<int, int>);
					}
					if (sketchEntity.IsSameSpawned(sketchEntity.pos + base.Position, base.Map))
					{
						value = new Pair<int, int>(value.First + 1, value.Second + 1);
					}
					else
					{
						value = new Pair<int, int>(value.First, value.Second + 1);
					}
					MonumentMarker.tmpBuiltParts[sketchEntity.Label] = value;
				}
				foreach (KeyValuePair<string, Pair<int, int>> keyValuePair in MonumentMarker.tmpBuiltParts)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(string.Concat(new object[]
					{
						keyValuePair.Key.CapitalizeFirst(),
						": ",
						keyValuePair.Value.First,
						" / ",
						keyValuePair.Value.Second
					}));
				}
				MonumentMarker.tmpBuiltParts.Clear();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x001A0CCC File Offset: 0x0019EECC
		private void RemovePossiblyRelatedBlueprints()
		{
			if (!base.Spawned)
			{
				return;
			}
			foreach (SketchBuildable sketchBuildable in this.sketch.Buildables)
			{
				Blueprint blueprint = sketchBuildable.GetSpawnedBlueprintOrFrame(base.Position + sketchBuildable.pos, base.Map) as Blueprint;
				if (blueprint != null)
				{
					blueprint.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x001A0D54 File Offset: 0x0019EF54
		public bool IsPart(Thing thing)
		{
			MonumentMarker.<>c__DisplayClass34_0 <>c__DisplayClass34_;
			<>c__DisplayClass34_.thing = thing;
			<>c__DisplayClass34_.<>4__this = this;
			if (!base.Spawned)
			{
				return false;
			}
			if (!this.sketch.OccupiedRect.MovedBy(base.Position).Contains(<>c__DisplayClass34_.thing.Position))
			{
				return false;
			}
			SketchThing sketchThing;
			List<SketchThing> list;
			this.sketch.ThingsAt(<>c__DisplayClass34_.thing.Position - base.Position, out sketchThing, out list);
			if (sketchThing != null && this.<IsPart>g__IsPartInternal|34_0(sketchThing, ref <>c__DisplayClass34_))
			{
				return true;
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (this.<IsPart>g__IsPartInternal|34_0(list[i], ref <>c__DisplayClass34_))
					{
						return true;
					}
				}
			}
			if (<>c__DisplayClass34_.thing.def.entityDefToBuild != null)
			{
				SketchTerrain sketchTerrain = this.sketch.SketchTerrainAt(<>c__DisplayClass34_.thing.Position - base.Position);
				if (sketchTerrain != null && this.<IsPart>g__IsPartInternal|34_0(sketchTerrain, ref <>c__DisplayClass34_))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x001A0E54 File Offset: 0x0019F054
		public bool AllowsPlacingBlueprint(BuildableDef buildable, IntVec3 pos, Rot4 rot, ThingDef stuff)
		{
			MonumentMarker.<>c__DisplayClass35_0 <>c__DisplayClass35_;
			<>c__DisplayClass35_.buildable = buildable;
			<>c__DisplayClass35_.<>4__this = this;
			<>c__DisplayClass35_.stuff = stuff;
			<>c__DisplayClass35_.rot = rot;
			if (!base.Spawned)
			{
				return true;
			}
			<>c__DisplayClass35_.newRect = GenAdj.OccupiedRect(pos, <>c__DisplayClass35_.rot, <>c__DisplayClass35_.buildable.Size);
			if (!this.sketch.OccupiedRect.MovedBy(base.Position).Overlaps(<>c__DisplayClass35_.newRect))
			{
				return true;
			}
			<>c__DisplayClass35_.collided = false;
			foreach (IntVec3 a in <>c__DisplayClass35_.newRect)
			{
				SketchThing sketchThing;
				List<SketchThing> list;
				this.sketch.ThingsAt(a - base.Position, out sketchThing, out list);
				if (sketchThing != null && this.<AllowsPlacingBlueprint>g__CheckEntity|35_1(sketchThing, ref <>c__DisplayClass35_))
				{
					return true;
				}
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (this.<AllowsPlacingBlueprint>g__CheckEntity|35_1(list[i], ref <>c__DisplayClass35_))
						{
							return true;
						}
					}
				}
				SketchTerrain sketchTerrain = this.sketch.SketchTerrainAt(a - base.Position);
				if (sketchTerrain != null && this.<AllowsPlacingBlueprint>g__CheckEntity|35_1(sketchTerrain, ref <>c__DisplayClass35_))
				{
					return true;
				}
			}
			return !<>c__DisplayClass35_.collided;
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x001A0FC4 File Offset: 0x0019F1C4
		public IEnumerable<ThingDef> AllowedStuffs()
		{
			MonumentMarker.tmpStuffCategories.Clear();
			bool flag = true;
			List<SketchThing> things = this.sketch.Things;
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].def.MadeFromStuff && things[i].stuff == null)
				{
					if (flag)
					{
						flag = false;
						MonumentMarker.tmpStuffCategories.AddRange(things[i].def.stuffCategories);
					}
					else
					{
						bool flag2 = false;
						for (int j = 0; j < things[i].def.stuffCategories.Count; j++)
						{
							if (MonumentMarker.tmpStuffCategories.Contains(things[i].def.stuffCategories[j]))
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							for (int k = MonumentMarker.tmpStuffCategories.Count - 1; k >= 0; k--)
							{
								if (!things[i].def.stuffCategories.Contains(MonumentMarker.tmpStuffCategories[k]))
								{
									MonumentMarker.tmpStuffCategories.RemoveAt(k);
								}
							}
						}
					}
				}
			}
			return GenStuff.AllowedStuffs(MonumentMarker.tmpStuffCategories, TechLevel.Undefined);
		}

		// Token: 0x04002B65 RID: 11109
		public Sketch sketch = new Sketch();

		// Token: 0x04002B66 RID: 11110
		public int ticksSinceDisallowedBuilding;

		// Token: 0x04002B67 RID: 11111
		public bool complete;

		// Token: 0x04002B68 RID: 11112
		private static readonly Texture2D PlaceBlueprintsCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/PlaceBlueprints", true);

		// Token: 0x04002B69 RID: 11113
		private static readonly Texture2D CancelCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04002B6A RID: 11114
		public const int DestroyAfterTicksSinceDisallowedBuilding = 60000;

		// Token: 0x04002B6B RID: 11115
		private const int MonumentCompletedCheckIntervalTicks = 177;

		// Token: 0x04002B6C RID: 11116
		private static List<ThingDef> tmpAllowedBuildings = new List<ThingDef>();

		// Token: 0x04002B6D RID: 11117
		private static HashSet<Pair<BuildableDef, ThingDef>> tmpUniqueBuildableDefs = new HashSet<Pair<BuildableDef, ThingDef>>();

		// Token: 0x04002B6E RID: 11118
		private static Dictionary<string, Pair<int, int>> tmpBuiltParts = new Dictionary<string, Pair<int, int>>();

		// Token: 0x04002B6F RID: 11119
		private static List<StuffCategoryDef> tmpStuffCategories = new List<StuffCategoryDef>();
	}
}
