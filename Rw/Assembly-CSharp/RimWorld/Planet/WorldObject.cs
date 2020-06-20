using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001272 RID: 4722
	[StaticConstructorOnStartup]
	public class WorldObject : IExposable, ILoadReferenceable, ISelectable
	{
		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x06006E89 RID: 28297 RVA: 0x0026970A File Offset: 0x0026790A
		public List<WorldObjectComp> AllComps
		{
			get
			{
				return this.comps;
			}
		}

		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x06006E8A RID: 28298 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool ShowRelatedQuests
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x06006E8B RID: 28299 RVA: 0x00269712 File Offset: 0x00267912
		public bool Destroyed
		{
			get
			{
				return this.destroyed;
			}
		}

		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06006E8C RID: 28300 RVA: 0x0026971A File Offset: 0x0026791A
		// (set) Token: 0x06006E8D RID: 28301 RVA: 0x00269722 File Offset: 0x00267922
		public int Tile
		{
			get
			{
				return this.tileInt;
			}
			set
			{
				if (this.tileInt != value)
				{
					this.tileInt = value;
					if (this.Spawned && !this.def.useDynamicDrawer)
					{
						Find.World.renderer.Notify_StaticWorldObjectPosChanged();
					}
					this.PositionChanged();
				}
			}
		}

		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x06006E8E RID: 28302 RVA: 0x0026975E File Offset: 0x0026795E
		public bool Spawned
		{
			get
			{
				return Find.WorldObjects.Contains(this);
			}
		}

		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06006E8F RID: 28303 RVA: 0x0026976B File Offset: 0x0026796B
		public virtual Vector3 DrawPos
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.Tile);
			}
		}

		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06006E90 RID: 28304 RVA: 0x0026977D File Offset: 0x0026797D
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x06006E91 RID: 28305 RVA: 0x00269785 File Offset: 0x00267985
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x06006E92 RID: 28306 RVA: 0x00269792 File Offset: 0x00267992
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x06006E93 RID: 28307 RVA: 0x002697A5 File Offset: 0x002679A5
		public virtual string LabelShort
		{
			get
			{
				return this.Label;
			}
		}

		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x06006E94 RID: 28308 RVA: 0x002697AD File Offset: 0x002679AD
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x06006E95 RID: 28309 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool HasName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x06006E96 RID: 28310 RVA: 0x002697C0 File Offset: 0x002679C0
		public virtual Material Material
		{
			get
			{
				return this.def.Material;
			}
		}

		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x06006E97 RID: 28311 RVA: 0x002697CD File Offset: 0x002679CD
		public virtual bool SelectableNow
		{
			get
			{
				return this.def.selectable;
			}
		}

		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x06006E98 RID: 28312 RVA: 0x002697DA File Offset: 0x002679DA
		public virtual bool NeverMultiSelect
		{
			get
			{
				return this.def.neverMultiSelect;
			}
		}

		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x06006E99 RID: 28313 RVA: 0x002697E7 File Offset: 0x002679E7
		public virtual Texture2D ExpandingIcon
		{
			get
			{
				return this.def.ExpandingIconTexture ?? ((Texture2D)this.Material.mainTexture);
			}
		}

		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x06006E9A RID: 28314 RVA: 0x00269808 File Offset: 0x00267A08
		public virtual Color ExpandingIconColor
		{
			get
			{
				return this.Material.color;
			}
		}

		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x06006E9B RID: 28315 RVA: 0x00269815 File Offset: 0x00267A15
		public virtual float ExpandingIconPriority
		{
			get
			{
				return this.def.expandingIconPriority;
			}
		}

		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x06006E9C RID: 28316 RVA: 0x00269822 File Offset: 0x00267A22
		public virtual bool ExpandMore
		{
			get
			{
				return this.def.expandMore;
			}
		}

		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x06006E9D RID: 28317 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AppendFactionToInspectString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x06006E9E RID: 28318 RVA: 0x0026982F File Offset: 0x00267A2F
		public IThingHolder ParentHolder
		{
			get
			{
				if (!this.Spawned)
				{
					return null;
				}
				return Find.World;
			}
		}

		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x06006E9F RID: 28319 RVA: 0x00269840 File Offset: 0x00267A40
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x06006EA0 RID: 28320 RVA: 0x00269849 File Offset: 0x00267A49
		public BiomeDef Biome
		{
			get
			{
				if (!this.Spawned)
				{
					return null;
				}
				return Find.WorldGrid[this.Tile].biome;
			}
		}

		// Token: 0x06006EA1 RID: 28321 RVA: 0x0026986A File Offset: 0x00267A6A
		public virtual IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			if (this.def.IncidentTargetTags != null)
			{
				foreach (IncidentTargetTagDef incidentTargetTagDef in this.def.IncidentTargetTags)
				{
					yield return incidentTargetTagDef;
				}
				List<IncidentTargetTagDef>.Enumerator enumerator = default(List<IncidentTargetTagDef>.Enumerator);
			}
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (IncidentTargetTagDef incidentTargetTagDef2 in this.comps[i].IncidentTargetTags())
				{
					yield return incidentTargetTagDef2;
				}
				IEnumerator<IncidentTargetTagDef> enumerator2 = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006EA2 RID: 28322 RVA: 0x0026987C File Offset: 0x00267A7C
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<WorldObjectDef>(ref this.def, "def");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<int>(ref this.tileInt, "tile", -1, false);
			Scribe_References.Look<Faction>(ref this.factionInt, "faction", false);
			Scribe_Values.Look<int>(ref this.creationGameTicks, "creationGameTicks", 0, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.destroyed, "destroyed", false, false);
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Scribe_Deep.Look<ThingOwner<Thing>>(ref this.rewards, "rewards", Array.Empty<object>());
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostExposeData();
			}
		}

		// Token: 0x06006EA3 RID: 28323 RVA: 0x0026995C File Offset: 0x00267B5C
		private void InitializeComps()
		{
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				WorldObjectComp worldObjectComp = null;
				try
				{
					worldObjectComp = (WorldObjectComp)Activator.CreateInstance(this.def.comps[i].compClass);
					worldObjectComp.parent = this;
					this.comps.Add(worldObjectComp);
					worldObjectComp.Initialize(this.def.comps[i]);
				}
				catch (Exception arg)
				{
					Log.Error("Could not instantiate or initialize a WorldObjectComp: " + arg, false);
					this.comps.Remove(worldObjectComp);
				}
			}
		}

		// Token: 0x06006EA4 RID: 28324 RVA: 0x00269A08 File Offset: 0x00267C08
		public virtual void SetFaction(Faction newFaction)
		{
			if (!this.def.canHaveFaction && newFaction != null)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set faction to ",
					newFaction,
					" but this world object (",
					this,
					") cannot have faction."
				}), false);
				return;
			}
			this.factionInt = newFaction;
		}

		// Token: 0x06006EA5 RID: 28325 RVA: 0x00269A60 File Offset: 0x00267C60
		public virtual string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Faction != null && this.AppendFactionToInspectString)
			{
				stringBuilder.Append("Faction".Translate() + ": " + this.Faction.Name);
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				string text = this.comps[i].CompInspectStringExtra();
				if (!text.NullOrEmpty())
				{
					if (Prefs.DevMode && char.IsWhiteSpace(text[text.Length - 1]))
					{
						Log.ErrorOnce(this.comps[i].GetType() + " CompInspectStringExtra ended with whitespace: " + text, 25612, false);
						text = text.TrimEndNewlines();
					}
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			return stringBuilder.ToString();
		}

		// Token: 0x06006EA6 RID: 28326 RVA: 0x00269B54 File Offset: 0x00267D54
		public virtual void Tick()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTick();
			}
		}

		// Token: 0x06006EA7 RID: 28327 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ExtraSelectionOverlaysOnGUI()
		{
		}

		// Token: 0x06006EA8 RID: 28328 RVA: 0x00269B88 File Offset: 0x00267D88
		public virtual void DrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDrawExtraSelectionOverlays();
			}
		}

		// Token: 0x06006EA9 RID: 28329 RVA: 0x00269BBC File Offset: 0x00267DBC
		public virtual void PostMake()
		{
			this.InitializeComps();
		}

		// Token: 0x06006EAA RID: 28330 RVA: 0x00269BC4 File Offset: 0x00267DC4
		public virtual void PostAdd()
		{
			QuestUtility.SendQuestTargetSignals(this.questTags, "Spawned", this.Named("SUBJECT"));
		}

		// Token: 0x06006EAB RID: 28331 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void PositionChanged()
		{
		}

		// Token: 0x06006EAC RID: 28332 RVA: 0x00269BE1 File Offset: 0x00267DE1
		public virtual void SpawnSetup()
		{
			if (!this.def.useDynamicDrawer)
			{
				Find.World.renderer.Notify_StaticWorldObjectPosChanged();
			}
			if (this.def.useDynamicDrawer)
			{
				Find.WorldDynamicDrawManager.RegisterDrawable(this);
			}
		}

		// Token: 0x06006EAD RID: 28333 RVA: 0x00269C18 File Offset: 0x00267E18
		public virtual void PostRemove()
		{
			if (!this.def.useDynamicDrawer)
			{
				Find.World.renderer.Notify_StaticWorldObjectPosChanged();
			}
			if (this.def.useDynamicDrawer)
			{
				Find.WorldDynamicDrawManager.DeRegisterDrawable(this);
			}
			Find.WorldSelector.Deselect(this);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostPostRemove();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Despawned", this.Named("SUBJECT"));
		}

		// Token: 0x06006EAE RID: 28334 RVA: 0x00269CA8 File Offset: 0x00267EA8
		public virtual void Destroy()
		{
			if (this.Destroyed)
			{
				Log.Error("Tried to destroy already-destroyed world object " + this, false);
				return;
			}
			if (this.Spawned)
			{
				Find.WorldObjects.Remove(this);
			}
			this.destroyed = true;
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDestroy();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Destroyed", this.Named("SUBJECT"));
		}

		// Token: 0x06006EAF RID: 28335 RVA: 0x00269D2C File Offset: 0x00267F2C
		public virtual void Print(LayerSubMesh subMesh)
		{
			float averageTileSize = Find.WorldGrid.averageTileSize;
			WorldRendererUtility.PrintQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, subMesh, false, true, true);
		}

		// Token: 0x06006EB0 RID: 28336 RVA: 0x00269D60 File Offset: 0x00267F60
		public virtual void Draw()
		{
			float averageTileSize = Find.WorldGrid.averageTileSize;
			float transitionPct = ExpandableWorldObjectsUtility.TransitionPct;
			if (this.def.expandingIcon && transitionPct > 0f)
			{
				Color color = this.Material.color;
				float num = 1f - transitionPct;
				WorldObject.propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * num));
				WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, this.Material, false, false, WorldObject.propertyBlock);
				return;
			}
			WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, this.Material, false, false, null);
		}

		// Token: 0x06006EB1 RID: 28337 RVA: 0x00269E1C File Offset: 0x0026801C
		public T GetComponent<T>() where T : WorldObjectComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				T t = this.comps[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06006EB2 RID: 28338 RVA: 0x00269E6C File Offset: 0x0026806C
		public WorldObjectComp GetComponent(Type type)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (type.IsAssignableFrom(this.comps[i].GetType()))
				{
					return this.comps[i];
				}
			}
			return null;
		}

		// Token: 0x06006EB3 RID: 28339 RVA: 0x00269EB6 File Offset: 0x002680B6
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			int num;
			if (this.ShowRelatedQuests)
			{
				List<Quest> quests = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < quests.Count; i = num + 1)
				{
					Quest quest = quests[i];
					if (!quest.Historical && !quest.dismissed && quest.QuestLookTargets.Contains(this))
					{
						yield return new Command_Action
						{
							defaultLabel = "CommandViewQuest".Translate(quest.name),
							defaultDesc = "CommandViewQuestDesc".Translate(),
							icon = WorldObject.ViewQuestCommandTex,
							action = delegate
							{
								Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
								((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(quest);
							}
						};
					}
					num = i;
				}
				quests = null;
			}
			for (int i = 0; i < this.comps.Count; i = num)
			{
				foreach (Gizmo gizmo in this.comps[i].GetGizmos())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006EB4 RID: 28340 RVA: 0x00269EC6 File Offset: 0x002680C6
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (Gizmo gizmo in this.comps[i].GetCaravanGizmos(caravan))
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006EB5 RID: 28341 RVA: 0x00269EDD File Offset: 0x002680DD
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num)
			{
				foreach (FloatMenuOption floatMenuOption in this.comps[i].GetFloatMenuOptions(caravan))
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006EB6 RID: 28342 RVA: 0x00269EF4 File Offset: 0x002680F4
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (FloatMenuOption floatMenuOption in this.comps[i].GetTransportPodsFloatMenuOptions(pods, representative))
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006EB7 RID: 28343 RVA: 0x00269F12 File Offset: 0x00268112
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		// Token: 0x06006EB8 RID: 28344 RVA: 0x00269F1F File Offset: 0x0026811F
		public virtual bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			return this.Faction == other.Faction;
		}

		// Token: 0x06006EB9 RID: 28345 RVA: 0x00269F30 File Offset: 0x00268130
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				" ",
				this.LabelCap,
				" (tile=",
				this.Tile,
				")"
			});
		}

		// Token: 0x06006EBA RID: 28346 RVA: 0x00269F85 File Offset: 0x00268185
		public override int GetHashCode()
		{
			return this.ID;
		}

		// Token: 0x06006EBB RID: 28347 RVA: 0x00269F8D File Offset: 0x0026818D
		public string GetUniqueLoadID()
		{
			return "WorldObject_" + this.ID;
		}

		// Token: 0x06006EBC RID: 28348 RVA: 0x00269FA4 File Offset: 0x002681A4
		public virtual string GetDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.def.description);
			for (int i = 0; i < this.comps.Count; i++)
			{
				string descriptionPart = this.comps[i].GetDescriptionPart();
				if (!descriptionPart.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(descriptionPart);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04004431 RID: 17457
		public WorldObjectDef def;

		// Token: 0x04004432 RID: 17458
		public int ID = -1;

		// Token: 0x04004433 RID: 17459
		private int tileInt = -1;

		// Token: 0x04004434 RID: 17460
		private Faction factionInt;

		// Token: 0x04004435 RID: 17461
		public int creationGameTicks = -1;

		// Token: 0x04004436 RID: 17462
		public List<string> questTags;

		// Token: 0x04004437 RID: 17463
		private bool destroyed;

		// Token: 0x04004438 RID: 17464
		public ThingOwner<Thing> rewards;

		// Token: 0x04004439 RID: 17465
		private List<WorldObjectComp> comps = new List<WorldObjectComp>();

		// Token: 0x0400443A RID: 17466
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400443B RID: 17467
		private const float BaseDrawSize = 0.7f;

		// Token: 0x0400443C RID: 17468
		private static readonly Texture2D ViewQuestCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewQuest", true);
	}
}
