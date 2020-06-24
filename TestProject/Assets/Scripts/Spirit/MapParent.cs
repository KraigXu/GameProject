using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spirit;

namespace Heroic
{
    [StaticConstructorOnStartup]
    public class MapParent : WorldObject, IThingHolder
    {

        public bool HasMap
        {
            get
            {
                return this.Map != null;
            }
        }

        // Token: 0x17001244 RID: 4676
        // (get) Token: 0x06006D5E RID: 27998 RVA: 0x0001028D File Offset: 0x0000E48D
        protected virtual bool UseGenericEnterMapFloatMenuOption
        {
            get
            {
                return true;
            }
        }

        // Token: 0x17001245 RID: 4677
        // (get) Token: 0x06006D5F RID: 27999 RVA: 0x00264559 File Offset: 0x00262759
        public Map Map
        {
            get
            {
                return Current.Game.FindMap(this);
            }
        }

        // Token: 0x17001246 RID: 4678
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

        // Token: 0x17001247 RID: 4679
        // (get) Token: 0x06006D61 RID: 28001 RVA: 0x00264586 File Offset: 0x00262786
        public virtual IEnumerable<GenStepWithParams> ExtraGenStepDefs
        {
            get
            {
                yield break;
            }
        }

        // Token: 0x17001248 RID: 4680
        // (get) Token: 0x06006D62 RID: 28002 RVA: 0x0026458F File Offset: 0x0026278F
        public override bool ExpandMore
        {
            get
            {
                return base.ExpandMore || this.HasMap;
            }
        }

        // Token: 0x17001249 RID: 4681
        // (get) Token: 0x06006D63 RID: 28003 RVA: 0x00010306 File Offset: 0x0000E506
        public virtual bool HandlesConditionCausers
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06006D64 RID: 28004 RVA: 0x002645A4 File Offset: 0x002627A4
        public virtual void PostMapGenerate()
        {
            List<WorldObjectComp> allComps = base.AllComps;
            for (int i = 0; i < allComps.Count; i++)
            {
                allComps[i].PostMapGenerate();
            }
            QuestUtility.SendQuestTargetSignals(this.questTags, "MapGenerated", this.Named("SUBJECT"));
        }

        // Token: 0x06006D65 RID: 28005 RVA: 0x00002681 File Offset: 0x00000881
        public virtual void Notify_MyMapAboutToBeRemoved()
        {
        }

        // Token: 0x06006D66 RID: 28006 RVA: 0x002645F0 File Offset: 0x002627F0
        public virtual void Notify_MyMapRemoved(Map map)
        {
            List<WorldObjectComp> allComps = base.AllComps;
            for (int i = 0; i < allComps.Count; i++)
            {
                allComps[i].PostMyMapRemoved();
            }
            QuestUtility.SendQuestTargetSignals(this.questTags, "MapRemoved", this.Named("SUBJECT"));
        }

        // Token: 0x06006D67 RID: 28007 RVA: 0x0026463C File Offset: 0x0026283C
        public virtual void Notify_CaravanFormed(Caravan caravan)
        {
            List<WorldObjectComp> allComps = base.AllComps;
            for (int i = 0; i < allComps.Count; i++)
            {
                allComps[i].PostCaravanFormed(caravan);
            }
        }

        // Token: 0x06006D68 RID: 28008 RVA: 0x0026466E File Offset: 0x0026286E
        public virtual void Notify_HibernatableChanged()
        {
            this.RecalculateHibernatableIncidentTargets();
        }

        // Token: 0x06006D69 RID: 28009 RVA: 0x0026466E File Offset: 0x0026286E
        public virtual void FinalizeLoading()
        {
            this.RecalculateHibernatableIncidentTargets();
        }

        // Token: 0x06006D6A RID: 28010 RVA: 0x001857E8 File Offset: 0x001839E8
        public virtual bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
        {
            alsoRemoveWorldObject = false;
            return false;
        }

        // Token: 0x06006D6B RID: 28011 RVA: 0x00264676 File Offset: 0x00262876
        public override void PostRemove()
        {
            base.PostRemove();
            if (this.HasMap)
            {
                Current.Game.DeinitAndRemoveMap(this.Map);
            }
        }

        // Token: 0x06006D6C RID: 28012 RVA: 0x00264696 File Offset: 0x00262896
        public override void Tick()
        {
            base.Tick();
            this.CheckRemoveMapNow();
        }

        // Token: 0x06006D6D RID: 28013 RVA: 0x002646A4 File Offset: 0x002628A4
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.forceRemoveWorldObjectWhenMapRemoved, "forceRemoveWorldObjectWhenMapRemoved", false, false);
        }

        // Token: 0x06006D6E RID: 28014 RVA: 0x002646BE File Offset: 0x002628BE
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in this.<> n__0())
            {
                yield return gizmo;
            }
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

        // Token: 0x06006D6F RID: 28015 RVA: 0x002646CE File Offset: 0x002628CE
        public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
        {
            foreach (IncidentTargetTagDef incidentTargetTagDef in this.<> n__1())
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

        // Token: 0x06006D70 RID: 28016 RVA: 0x002646DE File Offset: 0x002628DE
        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
        {
            foreach (FloatMenuOption floatMenuOption in this.<> n__2(caravan))
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

        // Token: 0x06006D71 RID: 28017 RVA: 0x002646F5 File Offset: 0x002628F5
        public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
        {
            foreach (FloatMenuOption floatMenuOption in this.<> n__3(pods, representative))
            {
                yield return floatMenuOption;
            }
            IEnumerator<FloatMenuOption> enumerator = null;
            if (TransportPodsArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this))
            {
                Action<LocalTargetInfo> <> 9__1;
                yield return new FloatMenuOption("LandInExistingMap".Translate(this.Label), delegate
                {
                    Map myMap = representative.parent.Map;
                    Map map = this.Map;
                    Current.Game.CurrentMap = map;
                    CameraJumper.TryHideWorld();
                    Targeter targeter = Find.Targeter;
                    TargetingParameters targetParams = TargetingParameters.ForDropPodsDestination();
                    Action<LocalTargetInfo> action;
                    if ((action = <> 9__1) == null)
                    {
                        action = (<> 9__1 = delegate (LocalTargetInfo x)
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

        // Token: 0x06006D72 RID: 28018 RVA: 0x00264714 File Offset: 0x00262914
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

        // Token: 0x06006D73 RID: 28019 RVA: 0x00264758 File Offset: 0x00262958
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

        // Token: 0x06006D74 RID: 28020 RVA: 0x0026484C File Offset: 0x00262A4C
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

        // Token: 0x06006D75 RID: 28021 RVA: 0x00019EA1 File Offset: 0x000180A1
        public ThingOwner GetDirectlyHeldThings()
        {
            return null;
        }

        // Token: 0x06006D76 RID: 28022 RVA: 0x002648C2 File Offset: 0x00262AC2
        public virtual void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
            if (this.HasMap)
            {
                outChildren.Add(this.Map);
            }
        }

        // Token: 0x06006D77 RID: 28023 RVA: 0x002648E4 File Offset: 0x00262AE4
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

        // Token: 0x040043DE RID: 17374
        public bool forceRemoveWorldObjectWhenMapRemoved;

        // Token: 0x040043DF RID: 17375
        private HashSet<IncidentTargetTagDef> hibernatableIncidentTargets;

        // Token: 0x040043E0 RID: 17376
        private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap", true);
    }
}
