using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class CompShuttle : ThingComp
	{
		
		
		public bool Autoload
		{
			get
			{
				return this.autoload;
			}
		}

		
		
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.Transporter.LoadingInProgressOrReadyToLaunch;
			}
		}

		
		
		public List<CompTransporter> TransportersInGroup
		{
			get
			{
				return this.Transporter.TransportersInGroup(this.parent.Map);
			}
		}

		
		
		public CompTransporter Transporter
		{
			get
			{
				if (this.cachedCompTransporter == null)
				{
					this.cachedCompTransporter = this.parent.GetComp<CompTransporter>();
				}
				return this.cachedCompTransporter;
			}
		}

		
		
		public bool ShowLoadingGizmos
		{
			get
			{
				return this.parent.Faction == null || this.parent.Faction == Faction.OfPlayer;
			}
		}

		
		
		public bool AnyInGroupIsUnderRoof
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					if (transportersInGroup[i].parent.Position.Roofed(this.parent.Map))
					{
						return true;
					}
				}
				return false;
			}
		}

		
		
		private bool Autoloadable
		{
			get
			{
				if (this.cachedTransporterList == null)
				{
					this.cachedTransporterList = new List<CompTransporter>
					{
						this.Transporter
					};
				}
				foreach (Pawn thing in TransporterUtility.AllSendablePawns(this.cachedTransporterList, this.parent.Map))
				{
					if (!this.IsRequired(thing))
					{
						return false;
					}
				}
				foreach (Thing thing2 in TransporterUtility.AllSendableItems(this.cachedTransporterList, this.parent.Map))
				{
					if (!this.IsRequired(thing2))
					{
						return false;
					}
				}
				return true;
			}
		}

		
		
		public bool AllRequiredThingsLoaded
		{
			get
			{
				ThingOwner innerContainer = this.Transporter.innerContainer;
				for (int i = 0; i < this.requiredPawns.Count; i++)
				{
					if (!innerContainer.Contains(this.requiredPawns[i]))
					{
						return false;
					}
				}
				if (this.requiredColonistCount > 0)
				{
					int num = 0;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && pawn.IsFreeColonist)
						{
							num++;
						}
					}
					if (num < this.requiredColonistCount)
					{
						return false;
					}
				}
				CompShuttle.tmpRequiredItemsWithoutDuplicates.Clear();
				for (int k = 0; k < this.requiredItems.Count; k++)
				{
					bool flag = false;
					for (int l = 0; l < CompShuttle.tmpRequiredItemsWithoutDuplicates.Count; l++)
					{
						if (CompShuttle.tmpRequiredItemsWithoutDuplicates[l].ThingDef == this.requiredItems[k].ThingDef)
						{
							CompShuttle.tmpRequiredItemsWithoutDuplicates[l] = CompShuttle.tmpRequiredItemsWithoutDuplicates[l].WithCount(CompShuttle.tmpRequiredItemsWithoutDuplicates[l].Count + this.requiredItems[k].Count);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						CompShuttle.tmpRequiredItemsWithoutDuplicates.Add(this.requiredItems[k]);
					}
				}
				for (int m = 0; m < CompShuttle.tmpRequiredItemsWithoutDuplicates.Count; m++)
				{
					int num2 = 0;
					for (int n = 0; n < innerContainer.Count; n++)
					{
						if (innerContainer[n].def == CompShuttle.tmpRequiredItemsWithoutDuplicates[m].ThingDef)
						{
							num2 += innerContainer[n].stackCount;
						}
					}
					if (num2 < CompShuttle.tmpRequiredItemsWithoutDuplicates[m].Count)
					{
						return false;
					}
				}
				return true;
			}
		}

		
		
		public TaggedString RequiredThingsLabel
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.requiredPawns.Count; i++)
				{
					stringBuilder.AppendLine("  - " + this.requiredPawns[i].NameShortColored.Resolve());
				}
				for (int j = 0; j < this.requiredItems.Count; j++)
				{
					stringBuilder.AppendLine("  - " + this.requiredItems[j].LabelCap);
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shuttle is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 8811221, false);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{

			IEnumerator<Gizmo> enumerator = null;
			if (this.ShowLoadingGizmos)
			{
				if (this.Autoloadable)
				{
					yield return new Command_Toggle
					{
						defaultLabel = "CommandAutoloadTransporters".Translate(),
						defaultDesc = "CommandAutoloadTransportersDesc".Translate(),
						icon = CompShuttle.AutoloadToggleTex,
						isActive = (() => this.autoload),
						toggleAction = delegate
						{
							this.autoload = !this.autoload;
							if (this.autoload && !this.LoadingInProgressOrReadyToLaunch)
							{
								TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
							}
							this.CheckAutoload();
						}
					};
				}
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandSendShuttle".Translate();
				command_Action.defaultDesc = "CommandSendShuttleDesc".Translate();
				command_Action.icon = CompShuttle.SendCommandTex;
				command_Action.alsoClickIfOtherInGroupClicked = false;
				command_Action.action = delegate
				{
					this.Send();
				};
				if (!this.LoadingInProgressOrReadyToLaunch || !this.AllRequiredThingsLoaded)
				{
					command_Action.Disable("CommandSendShuttleFailMissingRequiredThing".Translate());
				}
				yield return command_Action;
			}
			foreach (Gizmo gizmo2 in QuestUtility.GetQuestRelatedGizmos(this.parent))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!selPawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				yield break;
			}
			string text = "EnterShuttle".Translate();
			if (!this.IsAllowed(selPawn))
			{
				yield return new FloatMenuOption(text + " (" + "NotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield break;
			}
			yield return new FloatMenuOption(text, delegate
			{
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
				}
				Job job = JobMaker.MakeJob(JobDefOf.EnterTransporter, this.parent);
				selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield break;
		}

		
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(120))
			{
				this.CheckAutoload();
			}
			if (this.parent.Spawned && this.dropEverythingOnArrival && this.parent.IsHashIntervalTick(60))
			{
				if (this.Transporter.innerContainer.Any<Thing>())
				{
					Thing thing = this.Transporter.innerContainer[0];
					IntVec3 dropLoc = this.parent.Position + IntVec3.South;
					Thing thing2;
					if (this.Transporter.innerContainer.TryDrop_NewTmp(thing, dropLoc, this.parent.Map, ThingPlaceMode.Near, out thing2, null, delegate(IntVec3 c)
					{
						Pawn pawn;
						return (pawn = (this.Transporter.innerContainer[0] as Pawn)) == null || !pawn.Downed || c.GetFirstPawn(this.parent.Map) == null;
					}, false))
					{
						this.Transporter.Notify_ThingRemoved(thing);
					}
				}
				else
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
					this.Send();
				}
			}
			if (this.leaveASAP && this.parent.Spawned)
			{
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
				}
				this.Send();
			}
			if (this.leaveImmediatelyWhenSatisfied && this.AllRequiredThingsLoaded)
			{
				this.Send();
			}
		}

		
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Required".Translate() + ": ");
			CompShuttle.tmpRequiredLabels.Clear();
			if (this.requiredColonistCount > 0)
			{
				CompShuttle.tmpRequiredLabels.Add(this.requiredColonistCount + " " + ((this.requiredColonistCount > 1) ? Faction.OfPlayer.def.pawnsPlural : Faction.OfPlayer.def.pawnSingular));
			}
			for (int i = 0; i < this.requiredPawns.Count; i++)
			{
				CompShuttle.tmpRequiredLabels.Add(this.requiredPawns[i].LabelShort);
			}
			for (int j = 0; j < this.requiredItems.Count; j++)
			{
				CompShuttle.tmpRequiredLabels.Add(this.requiredItems[j].Label);
			}
			if (CompShuttle.tmpRequiredLabels.Any<string>())
			{
				stringBuilder.Append(CompShuttle.tmpRequiredLabels.ToCommaList(true).CapitalizeFirst());
			}
			else
			{
				stringBuilder.Append("Nothing".Translate());
			}
			return stringBuilder.ToString();
		}

		
		public void Send()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shuttle is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 8811221, false);
				return;
			}
			if (this.sending)
			{
				return;
			}
			if (!this.parent.Spawned)
			{
				Log.Error("Tried to send " + this.parent + ", but it's unspawned.", false);
				return;
			}
			List<CompTransporter> transportersInGroup = this.TransportersInGroup;
			if (transportersInGroup == null)
			{
				Log.Error("Tried to send " + this.parent + ", but it's not in any group.", false);
				return;
			}
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return;
			}
			if (!this.AllRequiredThingsLoaded)
			{
				if (this.dropEverythingIfUnsatisfied)
				{
					this.Transporter.CancelLoad();
				}
				else if (this.dropNonRequiredIfUnsatisfied)
				{
					for (int i = 0; i < transportersInGroup.Count; i++)
					{
						for (int j = transportersInGroup[i].innerContainer.Count - 1; j >= 0; j--)
						{
							Thing thing = transportersInGroup[i].innerContainer[j];
							Pawn pawn;
							if (!this.IsRequired(thing) && (this.requiredColonistCount <= 0 || (pawn = (thing as Pawn)) == null || !pawn.IsColonist))
							{
								Thing thing2;
								transportersInGroup[i].innerContainer.TryDrop(thing, ThingPlaceMode.Near, out thing2, null, null);
							}
						}
					}
				}
			}
			this.sending = true;
			bool allRequiredThingsLoaded = this.AllRequiredThingsLoaded;
			Map map = this.parent.Map;
			this.Transporter.TryRemoveLord(map);
			string signalPart = allRequiredThingsLoaded ? "SentSatisfied" : "SentUnsatisfied";
			for (int k = 0; k < transportersInGroup.Count; k++)
			{
				QuestUtility.SendQuestTargetSignals(transportersInGroup[k].parent.questTags, signalPart, transportersInGroup[k].parent.Named("SUBJECT"), transportersInGroup[k].innerContainer.ToList<Thing>().Named("SENT"));
			}
			List<Pawn> list = new List<Pawn>();
			for (int l = 0; l < transportersInGroup.Count; l++)
			{
				CompTransporter compTransporter = transportersInGroup[l];
				for (int m = transportersInGroup[l].innerContainer.Count - 1; m >= 0; m--)
				{
					Pawn pawn2 = transportersInGroup[l].innerContainer[m] as Pawn;
					if (pawn2 != null)
					{
						if (pawn2.IsColonist && !this.requiredPawns.Contains(pawn2))
						{
							list.Add(pawn2);
						}
						pawn2.ExitMap(false, Rot4.Invalid);
					}
				}
				compTransporter.innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
				Thing newThing = ThingMaker.MakeThing(ThingDefOf.ShuttleLeaving, null);
				compTransporter.CleanUpLoadingVars(map);
				compTransporter.parent.Destroy(DestroyMode.QuestLogic);
				GenSpawn.Spawn(newThing, compTransporter.parent.Position, map, WipeMode.Vanish);
			}
			if (list.Count != 0)
			{
				for (int n = 0; n < transportersInGroup.Count; n++)
				{
					QuestUtility.SendQuestTargetSignals(transportersInGroup[n].parent.questTags, "SentWithExtraColonists", transportersInGroup[n].parent.Named("SUBJECT"), list.Named("SENTCOLONISTS"));
				}
			}
			this.sending = false;
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<ThingDefCount>(ref this.requiredItems, "requiredItems", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.requiredPawns, "requiredPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.requiredColonistCount, "requiredColonistCount", 0, false);
			Scribe_Values.Look<bool>(ref this.acceptColonists, "acceptColonists", false, false);
			Scribe_Values.Look<bool>(ref this.onlyAcceptColonists, "onlyAcceptColonists", false, false);
			Scribe_Values.Look<bool>(ref this.leaveImmediatelyWhenSatisfied, "leaveImmediatelyWhenSatisfied", false, false);
			Scribe_Values.Look<bool>(ref this.autoload, "autoload", false, false);
			Scribe_Values.Look<bool>(ref this.dropEverythingIfUnsatisfied, "dropEverythingIfUnsatisfied", false, false);
			Scribe_Values.Look<bool>(ref this.dropNonRequiredIfUnsatisfied, "dropNonRequiredIfUnsatisfied", false, false);
			Scribe_Values.Look<bool>(ref this.leaveASAP, "leaveASAP", false, false);
			Scribe_Values.Look<bool>(ref this.dropEverythingOnArrival, "dropEverythingOnArrival", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.requiredPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		
		private void CheckAutoload()
		{
			if (!this.autoload || !this.LoadingInProgressOrReadyToLaunch || !this.parent.Spawned)
			{
				return;
			}
			CompShuttle.tmpRequiredItems.Clear();
			CompShuttle.tmpRequiredItems.AddRange(this.requiredItems);
			CompShuttle.tmpRequiredPawns.Clear();
			CompShuttle.tmpRequiredPawns.AddRange(this.requiredPawns);
			ThingOwner innerContainer = this.Transporter.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Pawn pawn = innerContainer[i] as Pawn;
				if (pawn != null)
				{
					CompShuttle.tmpRequiredPawns.Remove(pawn);
				}
				else
				{
					int num = innerContainer[i].stackCount;
					for (int j = 0; j < CompShuttle.tmpRequiredItems.Count; j++)
					{
						if (CompShuttle.tmpRequiredItems[j].ThingDef == innerContainer[i].def)
						{
							int num2 = Mathf.Min(CompShuttle.tmpRequiredItems[j].Count, num);
							if (num2 > 0)
							{
								CompShuttle.tmpRequiredItems[j] = CompShuttle.tmpRequiredItems[j].WithCount(CompShuttle.tmpRequiredItems[j].Count - num2);
								num -= num2;
							}
						}
					}
				}
			}
			for (int k = CompShuttle.tmpRequiredItems.Count - 1; k >= 0; k--)
			{
				if (CompShuttle.tmpRequiredItems[k].Count <= 0)
				{
					CompShuttle.tmpRequiredItems.RemoveAt(k);
				}
			}
			if (CompShuttle.tmpRequiredItems.Any<ThingDefCount>() || CompShuttle.tmpRequiredPawns.Any<Pawn>())
			{
				if (this.Transporter.leftToLoad != null)
				{
					this.Transporter.leftToLoad.Clear();
				}
				CompShuttle.tmpAllSendablePawns.Clear();
				CompShuttle.tmpAllSendablePawns.AddRange(TransporterUtility.AllSendablePawns(this.TransportersInGroup, this.parent.Map));
				CompShuttle.tmpAllSendableItems.Clear();
				CompShuttle.tmpAllSendableItems.AddRange(TransporterUtility.AllSendableItems(this.TransportersInGroup, this.parent.Map));
				CompShuttle.tmpAllSendableItems.AddRange(TransporterUtility.ThingsBeingHauledTo(this.TransportersInGroup, this.parent.Map));
				CompShuttle.tmpRequiredPawnsPossibleToSend.Clear();
				for (int l = 0; l < CompShuttle.tmpRequiredPawns.Count; l++)
				{
					if (CompShuttle.tmpAllSendablePawns.Contains(CompShuttle.tmpRequiredPawns[l]))
					{
						TransferableOneWay transferableOneWay = new TransferableOneWay();
						transferableOneWay.things.Add(CompShuttle.tmpRequiredPawns[l]);
						this.Transporter.AddToTheToLoadList(transferableOneWay, 1);
						CompShuttle.tmpRequiredPawnsPossibleToSend.Add(CompShuttle.tmpRequiredPawns[l]);
					}
				}
				for (int m = 0; m < CompShuttle.tmpRequiredItems.Count; m++)
				{
					if (CompShuttle.tmpRequiredItems[m].Count > 0)
					{
						int num3 = 0;
						for (int n = 0; n < CompShuttle.tmpAllSendableItems.Count; n++)
						{
							if (CompShuttle.tmpAllSendableItems[n].def == CompShuttle.tmpRequiredItems[m].ThingDef)
							{
								num3 += CompShuttle.tmpAllSendableItems[n].stackCount;
							}
						}
						if (num3 > 0)
						{
							TransferableOneWay transferableOneWay2 = new TransferableOneWay();
							for (int num4 = 0; num4 < CompShuttle.tmpAllSendableItems.Count; num4++)
							{
								if (CompShuttle.tmpAllSendableItems[num4].def == CompShuttle.tmpRequiredItems[m].ThingDef)
								{
									transferableOneWay2.things.Add(CompShuttle.tmpAllSendableItems[num4]);
								}
							}
							int count = Mathf.Min(CompShuttle.tmpRequiredItems[m].Count, num3);
							this.Transporter.AddToTheToLoadList(transferableOneWay2, count);
						}
					}
				}
				TransporterUtility.MakeLordsAsAppropriate(CompShuttle.tmpRequiredPawnsPossibleToSend, this.TransportersInGroup, this.parent.Map);
				CompShuttle.tmpAllSendablePawns.Clear();
				CompShuttle.tmpAllSendableItems.Clear();
				CompShuttle.tmpRequiredItems.Clear();
				CompShuttle.tmpRequiredPawns.Clear();
				CompShuttle.tmpRequiredPawnsPossibleToSend.Clear();
				return;
			}
			if (this.Transporter.leftToLoad != null)
			{
				this.Transporter.leftToLoad.Clear();
			}
			TransporterUtility.MakeLordsAsAppropriate(CompShuttle.tmpRequiredPawnsPossibleToSend, this.TransportersInGroup, this.parent.Map);
		}

		
		public bool IsRequired(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				return this.requiredPawns.Contains(pawn);
			}
			for (int i = 0; i < this.requiredItems.Count; i++)
			{
				if (this.requiredItems[i].ThingDef == thing.def && this.requiredItems[i].Count != 0)
				{
					return true;
				}
			}
			return false;
		}

		
		public bool IsAllowed(Thing t)
		{
			if (this.IsRequired(t))
			{
				return true;
			}
			if (this.acceptColonists)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null && (pawn.IsColonist || (!this.onlyAcceptColonists && pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer)) && (!this.onlyAcceptColonists || !pawn.IsQuestLodger()) && (!this.onlyAcceptHealthy || this.PawnIsHealthyEnoughForShuttle(pawn)))
				{
					return true;
				}
			}
			return false;
		}

		
		private bool PawnIsHealthyEnoughForShuttle(Pawn p)
		{
			return !p.Downed && !p.InMentalState && p.health.capacities.CanBeAwake && p.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}

		
		public void CleanUpLoadingVars()
		{
			this.autoload = false;
		}

		
		public List<ThingDefCount> requiredItems = new List<ThingDefCount>();

		
		public List<Pawn> requiredPawns = new List<Pawn>();

		
		public int requiredColonistCount;

		
		public bool acceptColonists;

		
		public bool onlyAcceptColonists;

		
		public bool onlyAcceptHealthy;

		
		public bool dropEverythingIfUnsatisfied;

		
		public bool dropNonRequiredIfUnsatisfied = true;

		
		public bool leaveImmediatelyWhenSatisfied;

		
		public bool dropEverythingOnArrival;

		
		private bool autoload;

		
		public bool leaveASAP;

		
		private CompTransporter cachedCompTransporter;

		
		private List<CompTransporter> cachedTransporterList;

		
		private bool sending;

		
		private const int CheckAutoloadIntervalTicks = 120;

		
		private const int DropInterval = 60;

		
		private static readonly Texture2D AutoloadToggleTex = ContentFinder<Texture2D>.Get("UI/Commands/Autoload", true);

		
		private static readonly Texture2D SendCommandTex = CompLaunchable.LaunchCommandTex;

		
		private static List<ThingDefCount> tmpRequiredItemsWithoutDuplicates = new List<ThingDefCount>();

		
		private static List<string> tmpRequiredLabels = new List<string>();

		
		private static List<ThingDefCount> tmpRequiredItems = new List<ThingDefCount>();

		
		private static List<Pawn> tmpRequiredPawns = new List<Pawn>();

		
		private static List<Pawn> tmpAllSendablePawns = new List<Pawn>();

		
		private static List<Thing> tmpAllSendableItems = new List<Thing>();

		
		private static List<Pawn> tmpRequiredPawnsPossibleToSend = new List<Pawn>();
	}
}
