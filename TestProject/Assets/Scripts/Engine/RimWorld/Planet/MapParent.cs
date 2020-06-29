using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	
	[StaticConstructorOnStartup]
	public class MapParent : WorldObject, IThingHolder
	{
		
		// (get) Token: 0x06006D5D RID: 27997 RVA: 0x0026454E File Offset: 0x0026274E
		public bool HasMap
		{
			get
			{
				return this.Map != null;
			}
		}

		
		// (get) Token: 0x06006D5E RID: 27998 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06006D5F RID: 27999 RVA: 0x00264559 File Offset: 0x00262759
		public Map Map
		{
			get
			{
				return Current.Game.FindMap(this);
			}
		}

		
		// (get) Token: 0x06006D60 RID: 28000 RVA: 0x00264566 File Offset: 0x00262766
		public virtual MapGeneratorDef MapGeneratorDef
		{
			get
			{
				if (this.def.mapGenerator == null)
				{
					return MapGeneratorDefOf.Encounter;
				}
				return this.def.mapGenerator;
			}
		}

		
		// (get) Token: 0x06006D61 RID: 28001 RVA: 0x00264586 File Offset: 0x00262786
		public virtual IEnumerable<GenStepWithParams> ExtraGenStepDefs
		{
			get
			{
				yield break;
			}
		}

		
		// (get) Token: 0x06006D62 RID: 28002 RVA: 0x0026458F File Offset: 0x0026278F
		public override bool ExpandMore
		{
			get
			{
				return base.ExpandMore || this.HasMap;
			}
		}

		
		// (get) Token: 0x06006D63 RID: 28003 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool HandlesConditionCausers
		{
			get
			{
				return false;
			}
		}

		
		public virtual void PostMapGenerate()
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMapGenerate();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "MapGenerated", this.Named("SUBJECT"));
		}

		
		public virtual void Notify_MyMapAboutToBeRemoved()
		{
		}

		
		public virtual void Notify_MyMapRemoved(Map map)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMyMapRemoved();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "MapRemoved", this.Named("SUBJECT"));
		}

		
		public virtual void Notify_CaravanFormed(Caravan caravan)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostCaravanFormed(caravan);
			}
		}

		
		public virtual void Notify_HibernatableChanged()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		
		public virtual void FinalizeLoading()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		
		public virtual bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return false;
		}

		
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.HasMap)
			{
				Current.Game.DeinitAndRemoveMap(this.Map);
			}
		}

		
		public override void Tick()
		{
			base.Tick();
			this.CheckRemoveMapNow();
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forceRemoveWorldObjectWhenMapRemoved, "forceRemoveWorldObjectWhenMapRemoved", false, false);
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{

			IEnumerator<Gizmo> enumerator = null;
			if (this.HasMap)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandShowMap".Translate(),
					defaultDesc = "CommandShowMapDesc".Translate(),
					icon = MapParent.ShowMapCommand,
					hotKey = KeyBindingDefOf.Misc1,
					action = delegate
					{
						Current.Game.CurrentMap = this.Map;
						if (!CameraJumper.TryHideWorld())
						{
							SoundDefOf.TabClose.PlayOneShotOnCamera(null);
						}
					}
				};
			}
			yield break;
			yield break;
		}

		
		public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			foreach (IncidentTargetTagDef incidentTargetTagDef in this.n__1())
			{
				yield return incidentTargetTagDef;
			}
			IEnumerator<IncidentTargetTagDef> enumerator = null;
			if (this.hibernatableIncidentTargets != null && this.hibernatableIncidentTargets.Count > 0)
			{
				foreach (IncidentTargetTagDef incidentTargetTagDef2 in this.hibernatableIncidentTargets)
				{
					yield return incidentTargetTagDef2;
				}
				HashSet<IncidentTargetTagDef>.Enumerator enumerator2 = default(HashSet<IncidentTargetTagDef>.Enumerator);
			}
			yield break;
			yield break;
		}

		
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in this.n__2(caravan))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.UseGenericEnterMapFloatMenuOption)
			{
				foreach (FloatMenuOption floatMenuOption2 in CaravanArrivalAction_Enter.GetFloatMenuOptions(caravan, this))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		
		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption floatMenuOption in this.n__3(pods, representative))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (TransportPodsArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this))
			{
				Action<LocalTargetInfo> 9__1;
				yield return new FloatMenuOption("LandInExistingMap".Translate(this.Label), delegate
				{
					Map myMap = representative.parent.Map;
					Map map = this.Map;
					Current.Game.CurrentMap = map;
					CameraJumper.TryHideWorld();
					Targeter targeter = Find.Targeter;
					TargetingParameters targetParams = TargetingParameters.ForDropPodsDestination();
					Action<LocalTargetInfo> action;
					if ((action ) == null)
					{
						action = (9__1 = delegate(LocalTargetInfo x)
						{
							representative.TryLaunch(this.Tile, new TransportPodsArrivalAction_LandInSpecificCell(this, x.Cell));
						});
					}
					targeter.BeginTargeting(targetParams, action, null, delegate
					{
						if (Find.Maps.Contains(myMap))
						{
							Current.Game.CurrentMap = myMap;
						}
					}, CompLaunchable.TargeterMouseAttachment);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			yield break;
			yield break;
		}

		
		public void CheckRemoveMapNow()
		{
			bool flag;
			if (this.HasMap && this.ShouldRemoveMapNow(out flag))
			{
				Map map = this.Map;
				Current.Game.DeinitAndRemoveMap(map);
				if (flag || this.forceRemoveWorldObjectWhenMapRemoved)
				{
					this.Destroy();
				}
			}
		}

		
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.EnterCooldownBlocksEntering())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "EnterCooldown".Translate(this.EnterCooldownDaysLeft().ToString("0.#"));
			}
			if (!this.HandlesConditionCausers && this.HasMap)
			{
				List<Thing> list = this.Map.listerThings.ThingsInGroup(ThingRequestGroup.ConditionCauser);
				for (int i = 0; i < list.Count; i++)
				{
					text += "\n" + list[i].LabelShortCap + " (" + "ConditionCauserRadius".Translate(list[i].TryGetComp<CompCauseGameCondition>().Props.worldRange) + ")";
				}
			}
			return text;
		}

		
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (!this.HandlesConditionCausers && this.HasMap)
			{
				int num = 0;
				List<Thing> list = this.Map.listerThings.ThingsInGroup(ThingRequestGroup.ConditionCauser);
				for (int i = 0; i < list.Count; i++)
				{
					num = Mathf.Max(num, list[i].TryGetComp<CompCauseGameCondition>().Props.worldRange);
				}
				if (num > 0)
				{
					GenDraw.DrawWorldRadiusRing(base.Tile, num);
				}
			}
		}

		
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		
		public virtual void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.HasMap)
			{
				outChildren.Add(this.Map);
			}
		}

		
		private void RecalculateHibernatableIncidentTargets()
		{
			this.hibernatableIncidentTargets = null;
			foreach (ThingWithComps thing in this.Map.listerThings.ThingsOfDef(ThingDefOf.Ship_Reactor).OfType<ThingWithComps>())
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Starting && compHibernatable.Props.incidentTargetWhileStarting != null)
				{
					if (this.hibernatableIncidentTargets == null)
					{
						this.hibernatableIncidentTargets = new HashSet<IncidentTargetTagDef>();
					}
					this.hibernatableIncidentTargets.Add(compHibernatable.Props.incidentTargetWhileStarting);
				}
			}
		}

		
		public bool forceRemoveWorldObjectWhenMapRemoved;

		
		private HashSet<IncidentTargetTagDef> hibernatableIncidentTargets;

		
		private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap", true);
	}
}
