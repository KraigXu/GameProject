using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	[StaticConstructorOnStartup]
	public class WorldObject : IExposable, ILoadReferenceable, ISelectable
	{
		
		
		public List<WorldObjectComp> AllComps
		{
			get
			{
				return this.comps;
			}
		}

		
		
		public virtual bool ShowRelatedQuests
		{
			get
			{
				return true;
			}
		}

		
		
		public bool Destroyed
		{
			get
			{
				return this.destroyed;
			}
		}

		
		
		
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

		
		
		public bool Spawned
		{
			get
			{
				return Find.WorldObjects.Contains(this);
			}
		}

		
		
		public virtual Vector3 DrawPos
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.Tile);
			}
		}

		
		
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		
		
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		
		
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		
		
		public virtual string LabelShort
		{
			get
			{
				return this.Label;
			}
		}

		
		
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst(this.def);
			}
		}

		
		
		public virtual bool HasName
		{
			get
			{
				return false;
			}
		}

		
		
		public virtual Material Material
		{
			get
			{
				return this.def.Material;
			}
		}

		
		
		public virtual bool SelectableNow
		{
			get
			{
				return this.def.selectable;
			}
		}

		
		
		public virtual bool NeverMultiSelect
		{
			get
			{
				return this.def.neverMultiSelect;
			}
		}

		
		
		public virtual Texture2D ExpandingIcon
		{
			get
			{
				return this.def.ExpandingIconTexture ?? ((Texture2D)this.Material.mainTexture);
			}
		}

		
		
		public virtual Color ExpandingIconColor
		{
			get
			{
				return this.Material.color;
			}
		}

		
		
		public virtual float ExpandingIconPriority
		{
			get
			{
				return this.def.expandingIconPriority;
			}
		}

		
		
		public virtual bool ExpandMore
		{
			get
			{
				return this.def.expandMore;
			}
		}

		
		
		public virtual bool AppendFactionToInspectString
		{
			get
			{
				return true;
			}
		}

		
		
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

		
		
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				yield break;
			}
		}

		
		
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

		
		public virtual void Tick()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTick();
			}
		}

		
		public virtual void ExtraSelectionOverlaysOnGUI()
		{
		}

		
		public virtual void DrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDrawExtraSelectionOverlays();
			}
		}

		
		public virtual void PostMake()
		{
			this.InitializeComps();
		}

		
		public virtual void PostAdd()
		{
			QuestUtility.SendQuestTargetSignals(this.questTags, "Spawned", this.Named("SUBJECT"));
		}

		
		protected virtual void PositionChanged()
		{
		}

		
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

		
		public virtual void Print(LayerSubMesh subMesh)
		{
			float averageTileSize = Find.WorldGrid.averageTileSize;
			WorldRendererUtility.PrintQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, subMesh, false, true, true);
		}

		
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

		
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num)
			{
				foreach (FloatMenuOption floatMenuOption in this.comps[i].GetFloatMenuOptions(caravan))
				{
					
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (FloatMenuOption floatMenuOption in this.comps[i].GetTransportPodsFloatMenuOptions(pods, representative))
				{
					
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		
		public virtual bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			return this.Faction == other.Faction;
		}

		
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

		
		public override int GetHashCode()
		{
			return this.ID;
		}

		
		public string GetUniqueLoadID()
		{
			return "WorldObject_" + this.ID;
		}

		
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

		
		public WorldObjectDef def;

		
		public int ID = -1;

		
		private int tileInt = -1;

		
		private Faction factionInt;

		
		public int creationGameTicks = -1;

		
		public List<string> questTags;

		
		private bool destroyed;

		
		public ThingOwner<Thing> rewards;

		
		private List<WorldObjectComp> comps = new List<WorldObjectComp>();

		
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		
		private const float BaseDrawSize = 0.7f;

		
		private static readonly Texture2D ViewQuestCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewQuest", true);
	}
}
