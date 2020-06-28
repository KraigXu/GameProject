using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D73 RID: 3443
	[StaticConstructorOnStartup]
	public class CompTransporter : ThingComp, IThingHolder
	{
		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x060053D1 RID: 21457 RVA: 0x001BFDB9 File Offset: 0x001BDFB9
		public CompProperties_Transporter Props
		{
			get
			{
				return (CompProperties_Transporter)this.props;
			}
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x060053D2 RID: 21458 RVA: 0x001BFDC6 File Offset: 0x001BDFC6
		public Map Map
		{
			get
			{
				return this.parent.MapHeld;
			}
		}

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x060053D3 RID: 21459 RVA: 0x001BFDD3 File Offset: 0x001BDFD3
		public bool AnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoad != null;
			}
		}

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x060053D4 RID: 21460 RVA: 0x001BFDDE File Offset: 0x001BDFDE
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.groupID >= 0;
			}
		}

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x060053D5 RID: 21461 RVA: 0x001BFDEC File Offset: 0x001BDFEC
		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoadInGroup != null;
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x060053D6 RID: 21462 RVA: 0x001BFDF7 File Offset: 0x001BDFF7
		public CompLaunchable Launchable
		{
			get
			{
				if (this.cachedCompLaunchable == null)
				{
					this.cachedCompLaunchable = this.parent.GetComp<CompLaunchable>();
				}
				return this.cachedCompLaunchable;
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x060053D7 RID: 21463 RVA: 0x001BFE18 File Offset: 0x001BE018
		public CompShuttle Shuttle
		{
			get
			{
				if (this.cachedCompShuttle == null)
				{
					this.cachedCompShuttle = this.parent.GetComp<CompShuttle>();
				}
				return this.cachedCompShuttle;
			}
		}

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x060053D8 RID: 21464 RVA: 0x001BFE3C File Offset: 0x001BE03C
		public Thing FirstThingLeftToLoad
		{
			get
			{
				if (this.leftToLoad == null)
				{
					return null;
				}
				for (int i = 0; i < this.leftToLoad.Count; i++)
				{
					if (this.leftToLoad[i].CountToTransfer != 0 && this.leftToLoad[i].HasAnyThing)
					{
						return this.leftToLoad[i].AnyThing;
					}
				}
				return null;
			}
		}

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x060053D9 RID: 21465 RVA: 0x001BFEA4 File Offset: 0x001BE0A4
		public Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				for (int i = 0; i < list.Count; i++)
				{
					Thing firstThingLeftToLoad = list[i].FirstThingLeftToLoad;
					if (firstThingLeftToLoad != null)
					{
						return firstThingLeftToLoad;
					}
				}
				return null;
			}
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x060053DA RID: 21466 RVA: 0x001BFEE8 File Offset: 0x001BE0E8
		public bool AnyInGroupNotifiedCantLoadMore
		{
			get
			{
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].notifiedCantLoadMore)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x060053DB RID: 21467 RVA: 0x001BFF2C File Offset: 0x001BE12C
		public bool AnyPawnCanLoadAnythingNow
		{
			get
			{
				if (!this.AnythingLeftToLoad)
				{
					return false;
				}
				if (!this.parent.Spawned)
				{
					return false;
				}
				List<Pawn> allPawnsSpawned = this.parent.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
					{
						CompTransporter transporter = ((JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter;
						if (transporter != null && transporter.groupID == this.groupID)
						{
							return true;
						}
					}
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.EnterTransporter)
					{
						CompTransporter transporter2 = ((JobDriver_EnterTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter;
						if (transporter2 != null && transporter2.groupID == this.groupID)
						{
							return true;
						}
					}
				}
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				for (int j = 0; j < allPawnsSpawned.Count; j++)
				{
					if (allPawnsSpawned[j].mindState.duty != null && allPawnsSpawned[j].mindState.duty.transportersGroup == this.groupID)
					{
						CompTransporter compTransporter = JobGiver_EnterTransporter.FindMyTransporter(list, allPawnsSpawned[j]);
						if (compTransporter != null && allPawnsSpawned[j].CanReach(compTransporter.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							return true;
						}
					}
				}
				for (int k = 0; k < allPawnsSpawned.Count; k++)
				{
					if (allPawnsSpawned[k].IsColonist)
					{
						for (int l = 0; l < list.Count; l++)
						{
							if (LoadTransportersJobUtility.HasJobOnTransporter(allPawnsSpawned[k], list[l]))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x060053DC RID: 21468 RVA: 0x001C00E8 File Offset: 0x001BE2E8
		public CompTransporter()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x060053DD RID: 21469 RVA: 0x001C0104 File Offset: 0x001BE304
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Collections.Look<TransferableOneWay>(ref this.leftToLoad, "leftToLoad", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.notifiedCantLoadMore, "notifiedCantLoadMore", false, false);
		}

		// Token: 0x060053DE RID: 21470 RVA: 0x001C016B File Offset: 0x001BE36B
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060053DF RID: 21471 RVA: 0x001C0173 File Offset: 0x001BE373
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x001C0184 File Offset: 0x001BE384
		public override void CompTick()
		{
			base.CompTick();
			this.innerContainer.ThingOwnerTick(true);
			if (this.Props.restEffectiveness != 0f)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Pawn pawn = this.innerContainer[i] as Pawn;
					if (pawn != null && !pawn.Dead && pawn.needs.rest != null)
					{
						pawn.needs.rest.TickResting(this.Props.restEffectiveness);
					}
				}
			}
			if (this.parent.IsHashIntervalTick(60) && this.parent.Spawned && this.LoadingInProgressOrReadyToLaunch && this.AnyInGroupHasAnythingLeftToLoad && !this.AnyInGroupNotifiedCantLoadMore && !this.AnyPawnCanLoadAnythingNow && (this.Shuttle == null || !this.Shuttle.Autoload))
			{
				this.notifiedCantLoadMore = true;
				Messages.Message("MessageCantLoadMoreIntoTransporters".Translate(this.FirstThingLeftToLoadInGroup.LabelNoCount, Faction.OfPlayer.def.pawnsPlural, this.FirstThingLeftToLoadInGroup), this.parent, MessageTypeDefOf.CautionInput, true);
			}
		}

		// Token: 0x060053E1 RID: 21473 RVA: 0x001C02C8 File Offset: 0x001BE4C8
		public List<CompTransporter> TransportersInGroup(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return null;
			}
			TransporterUtility.GetTransportersInGroup(this.groupID, map, CompTransporter.tmpTransportersInGroup);
			return CompTransporter.tmpTransportersInGroup;
		}

		// Token: 0x060053E2 RID: 21474 RVA: 0x001C02EA File Offset: 0x001BE4EA
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.Shuttle != null && !this.Shuttle.ShowLoadingGizmos)
			{
				yield break;
			}
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				if (this.Shuttle == null || !this.Shuttle.Autoload)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandCancelLoad".Translate(),
						defaultDesc = "CommandCancelLoadDesc".Translate(),
						icon = CompTransporter.CancelLoadCommandTex,
						action = delegate
						{
							SoundDefOf.Designate_Cancel.PlayOneShotOnCamera(null);
							this.CancelLoad();
						}
					};
				}
				if (!this.Props.max1PerGroup)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandSelectPreviousTransporter".Translate(),
						defaultDesc = "CommandSelectPreviousTransporterDesc".Translate(),
						icon = CompTransporter.SelectPreviousInGroupCommandTex,
						action = delegate
						{
							this.SelectPreviousInGroup();
						}
					};
					yield return new Command_Action
					{
						defaultLabel = "CommandSelectAllTransporters".Translate(),
						defaultDesc = "CommandSelectAllTransportersDesc".Translate(),
						icon = CompTransporter.SelectAllInGroupCommandTex,
						action = delegate
						{
							this.SelectAllInGroup();
						}
					};
					yield return new Command_Action
					{
						defaultLabel = "CommandSelectNextTransporter".Translate(),
						defaultDesc = "CommandSelectNextTransporterDesc".Translate(),
						icon = CompTransporter.SelectNextInGroupCommandTex,
						action = delegate
						{
							this.SelectNextInGroup();
						}
					};
				}
				if (this.Props.canChangeAssignedThingsAfterStarting && (this.Shuttle == null || !this.Shuttle.Autoload))
				{
					yield return new Command_LoadToTransporter
					{
						defaultLabel = "CommandSetToLoadTransporter".Translate(),
						defaultDesc = "CommandSetToLoadTransporterDesc".Translate(),
						icon = CompTransporter.LoadCommandTex,
						transComp = this
					};
				}
			}
			else
			{
				Command_LoadToTransporter command_LoadToTransporter = new Command_LoadToTransporter();
				if (this.Props.max1PerGroup)
				{
					if (this.Props.canChangeAssignedThingsAfterStarting)
					{
						command_LoadToTransporter.defaultLabel = "CommandSetToLoadTransporter".Translate();
						command_LoadToTransporter.defaultDesc = "CommandSetToLoadTransporterDesc".Translate();
					}
					else
					{
						command_LoadToTransporter.defaultLabel = "CommandLoadTransporterSingle".Translate();
						command_LoadToTransporter.defaultDesc = "CommandLoadTransporterSingleDesc".Translate();
					}
				}
				else
				{
					int num = 0;
					for (int i = 0; i < Find.Selector.NumSelected; i++)
					{
						Thing thing = Find.Selector.SelectedObjectsListForReading[i] as Thing;
						if (thing != null && thing.def == this.parent.def)
						{
							CompLaunchable compLaunchable = thing.TryGetComp<CompLaunchable>();
							if (compLaunchable == null || (compLaunchable.FuelingPortSource != null && compLaunchable.FuelingPortSourceHasAnyFuel))
							{
								num++;
							}
						}
					}
					command_LoadToTransporter.defaultLabel = "CommandLoadTransporter".Translate(num.ToString());
					command_LoadToTransporter.defaultDesc = "CommandLoadTransporterDesc".Translate();
				}
				command_LoadToTransporter.icon = CompTransporter.LoadCommandTex;
				command_LoadToTransporter.transComp = this;
				CompLaunchable launchable = this.Launchable;
				if (launchable != null)
				{
					if (!launchable.ConnectedToFuelingPort)
					{
						command_LoadToTransporter.Disable("CommandLoadTransporterFailNotConnectedToFuelingPort".Translate());
					}
					else if (!launchable.FuelingPortSourceHasAnyFuel)
					{
						command_LoadToTransporter.Disable("CommandLoadTransporterFailNoFuel".Translate());
					}
				}
				yield return command_LoadToTransporter;
			}
			yield break;
			yield break;
		}

		// Token: 0x060053E3 RID: 21475 RVA: 0x001C02FC File Offset: 0x001BE4FC
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.CancelLoad(map))
			{
				if (this.Props.max1PerGroup)
				{
					Messages.Message("MessageTransporterSingleLoadCanceled_TransporterDestroyed".Translate(), MessageTypeDefOf.NegativeEvent, true);
				}
				else
				{
					Messages.Message("MessageTransportersLoadCanceled_TransporterDestroyed".Translate(), MessageTypeDefOf.NegativeEvent, true);
				}
			}
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null);
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x001C0377 File Offset: 0x001BE577
		public override string CompInspectStringExtra()
		{
			return "Contents".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst();
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x001C03A8 File Offset: 0x001BE5A8
		public void AddToTheToLoadList(TransferableOneWay t, int count)
		{
			if (!t.HasAnyThing || count <= 0)
			{
				return;
			}
			if (this.leftToLoad == null)
			{
				this.leftToLoad = new List<TransferableOneWay>();
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t.AnyThing, this.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay != null)
			{
				for (int i = 0; i < t.things.Count; i++)
				{
					if (!transferableOneWay.things.Contains(t.things[i]))
					{
						transferableOneWay.things.Add(t.things[i]);
					}
				}
				if (transferableOneWay.CanAdjustBy(count).Accepted)
				{
					transferableOneWay.AdjustBy(count);
					return;
				}
			}
			else
			{
				TransferableOneWay transferableOneWay2 = new TransferableOneWay();
				this.leftToLoad.Add(transferableOneWay2);
				transferableOneWay2.things.AddRange(t.things);
				transferableOneWay2.AdjustTo(count);
			}
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x001C0474 File Offset: 0x001BE674
		public void Notify_ThingAdded(Thing t)
		{
			this.SubtractFromToLoadList(t, t.stackCount, true);
			if (this.Props.pawnLoadedSound != null && t is Pawn)
			{
				this.Props.pawnLoadedSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x001C04D8 File Offset: 0x001BE6D8
		public void Notify_ThingRemoved(Thing t)
		{
			if (this.Props.pawnExitSound != null && t is Pawn)
			{
				this.Props.pawnExitSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x001C052B File Offset: 0x001BE72B
		public void Notify_ThingAddedAndMergedWith(Thing t, int mergedCount)
		{
			this.SubtractFromToLoadList(t, mergedCount, true);
		}

		// Token: 0x060053E9 RID: 21481 RVA: 0x001C0537 File Offset: 0x001BE737
		public bool CancelLoad()
		{
			return this.CancelLoad(this.Map);
		}

		// Token: 0x060053EA RID: 21482 RVA: 0x001C0548 File Offset: 0x001BE748
		public bool CancelLoad(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return false;
			}
			this.TryRemoveLord(map);
			List<CompTransporter> list = this.TransportersInGroup(map);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].CleanUpLoadingVars(map);
			}
			this.CleanUpLoadingVars(map);
			return true;
		}

		// Token: 0x060053EB RID: 21483 RVA: 0x001C0594 File Offset: 0x001BE794
		public void TryRemoveLord(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return;
			}
			Lord lord = TransporterUtility.FindLord(this.groupID, map);
			if (lord != null)
			{
				map.lordManager.RemoveLord(lord);
			}
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x001C05C8 File Offset: 0x001BE7C8
		public void CleanUpLoadingVars(Map map)
		{
			this.groupID = -1;
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null);
			if (this.leftToLoad != null)
			{
				this.leftToLoad.Clear();
			}
			CompShuttle shuttle = this.Shuttle;
			if (shuttle != null)
			{
				shuttle.CleanUpLoadingVars();
			}
		}

		// Token: 0x060053ED RID: 21485 RVA: 0x001C061C File Offset: 0x001BE81C
		public int SubtractFromToLoadList(Thing t, int count, bool sendMessageOnFinished = true)
		{
			if (this.leftToLoad == null)
			{
				return 0;
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(t, this.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay == null)
			{
				return 0;
			}
			if (transferableOneWay.CountToTransfer <= 0)
			{
				return 0;
			}
			int num = Mathf.Min(count, transferableOneWay.CountToTransfer);
			transferableOneWay.AdjustBy(-num);
			if (transferableOneWay.CountToTransfer <= 0)
			{
				this.leftToLoad.Remove(transferableOneWay);
			}
			if (sendMessageOnFinished && !this.AnyInGroupHasAnythingLeftToLoad)
			{
				CompShuttle comp = this.parent.GetComp<CompShuttle>();
				if (comp == null || comp.AllRequiredThingsLoaded)
				{
					if (this.Props.max1PerGroup)
					{
						Messages.Message("MessageFinishedLoadingTransporterSingle".Translate(), this.parent, MessageTypeDefOf.TaskCompletion, true);
					}
					else
					{
						Messages.Message("MessageFinishedLoadingTransporters".Translate(), this.parent, MessageTypeDefOf.TaskCompletion, true);
					}
				}
			}
			return num;
		}

		// Token: 0x060053EE RID: 21486 RVA: 0x001C06F8 File Offset: 0x001BE8F8
		private void SelectPreviousInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect(list[GenMath.PositiveMod(num - 1, list.Count)].parent);
		}

		// Token: 0x060053EF RID: 21487 RVA: 0x001C0740 File Offset: 0x001BE940
		private void SelectAllInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			Selector selector = Find.Selector;
			selector.ClearSelection();
			for (int i = 0; i < list.Count; i++)
			{
				selector.Select(list[i].parent, true, true);
			}
		}

		// Token: 0x060053F0 RID: 21488 RVA: 0x001C078C File Offset: 0x001BE98C
		private void SelectNextInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect(list[(num + 1) % list.Count].parent);
		}

		// Token: 0x04002E47 RID: 11847
		public int groupID = -1;

		// Token: 0x04002E48 RID: 11848
		public ThingOwner innerContainer;

		// Token: 0x04002E49 RID: 11849
		public List<TransferableOneWay> leftToLoad;

		// Token: 0x04002E4A RID: 11850
		private bool notifiedCantLoadMore;

		// Token: 0x04002E4B RID: 11851
		private CompLaunchable cachedCompLaunchable;

		// Token: 0x04002E4C RID: 11852
		private CompShuttle cachedCompShuttle;

		// Token: 0x04002E4D RID: 11853
		private static readonly Texture2D CancelLoadCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04002E4E RID: 11854
		private static readonly Texture2D LoadCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LoadTransporter", true);

		// Token: 0x04002E4F RID: 11855
		private static readonly Texture2D SelectPreviousInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectPreviousTransporter", true);

		// Token: 0x04002E50 RID: 11856
		private static readonly Texture2D SelectAllInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectAllTransporters", true);

		// Token: 0x04002E51 RID: 11857
		private static readonly Texture2D SelectNextInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectNextTransporter", true);

		// Token: 0x04002E52 RID: 11858
		private static List<CompTransporter> tmpTransportersInGroup = new List<CompTransporter>();
	}
}
