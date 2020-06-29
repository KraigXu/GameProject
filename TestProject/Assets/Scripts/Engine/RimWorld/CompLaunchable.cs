﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class CompLaunchable : ThingComp
	{
		
		// (get) Token: 0x06005189 RID: 20873 RVA: 0x001B5199 File Offset: 0x001B3399
		public Building FuelingPortSource
		{
			get
			{
				return FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(this.parent.Position, this.parent.Map);
			}
		}

		
		// (get) Token: 0x0600518A RID: 20874 RVA: 0x001B51B6 File Offset: 0x001B33B6
		public bool ConnectedToFuelingPort
		{
			get
			{
				return this.FuelingPortSource != null;
			}
		}

		
		// (get) Token: 0x0600518B RID: 20875 RVA: 0x001B51C1 File Offset: 0x001B33C1
		public bool FuelingPortSourceHasAnyFuel
		{
			get
			{
				return this.ConnectedToFuelingPort && this.FuelingPortSource.GetComp<CompRefuelable>().HasFuel;
			}
		}

		
		// (get) Token: 0x0600518C RID: 20876 RVA: 0x001B51DD File Offset: 0x001B33DD
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.Transporter.LoadingInProgressOrReadyToLaunch;
			}
		}

		
		// (get) Token: 0x0600518D RID: 20877 RVA: 0x001B51EA File Offset: 0x001B33EA
		public bool AnythingLeftToLoad
		{
			get
			{
				return this.Transporter.AnythingLeftToLoad;
			}
		}

		
		// (get) Token: 0x0600518E RID: 20878 RVA: 0x001B51F7 File Offset: 0x001B33F7
		public Thing FirstThingLeftToLoad
		{
			get
			{
				return this.Transporter.FirstThingLeftToLoad;
			}
		}

		
		// (get) Token: 0x0600518F RID: 20879 RVA: 0x001B5204 File Offset: 0x001B3404
		public List<CompTransporter> TransportersInGroup
		{
			get
			{
				return this.Transporter.TransportersInGroup(this.parent.Map);
			}
		}

		
		// (get) Token: 0x06005190 RID: 20880 RVA: 0x001B521C File Offset: 0x001B341C
		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.Transporter.AnyInGroupHasAnythingLeftToLoad;
			}
		}

		
		// (get) Token: 0x06005191 RID: 20881 RVA: 0x001B5229 File Offset: 0x001B3429
		public Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				return this.Transporter.FirstThingLeftToLoadInGroup;
			}
		}

		
		// (get) Token: 0x06005192 RID: 20882 RVA: 0x001B5238 File Offset: 0x001B3438
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

		
		// (get) Token: 0x06005193 RID: 20883 RVA: 0x001B5283 File Offset: 0x001B3483
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

		
		// (get) Token: 0x06005194 RID: 20884 RVA: 0x001B52A4 File Offset: 0x001B34A4
		public float FuelingPortSourceFuel
		{
			get
			{
				if (!this.ConnectedToFuelingPort)
				{
					return 0f;
				}
				return this.FuelingPortSource.GetComp<CompRefuelable>().Fuel;
			}
		}

		
		// (get) Token: 0x06005195 RID: 20885 RVA: 0x001B52C4 File Offset: 0x001B34C4
		public bool AllInGroupConnectedToFuelingPort
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					if (!transportersInGroup[i].Launchable.ConnectedToFuelingPort)
					{
						return false;
					}
				}
				return true;
			}
		}

		
		// (get) Token: 0x06005196 RID: 20886 RVA: 0x001B5300 File Offset: 0x001B3500
		public bool AllFuelingPortSourcesInGroupHaveAnyFuel
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					if (!transportersInGroup[i].Launchable.FuelingPortSourceHasAnyFuel)
					{
						return false;
					}
				}
				return true;
			}
		}

		
		// (get) Token: 0x06005197 RID: 20887 RVA: 0x001B533C File Offset: 0x001B353C
		private float FuelInLeastFueledFuelingPortSource
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				float num = 0f;
				bool flag = false;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					float fuelingPortSourceFuel = transportersInGroup[i].Launchable.FuelingPortSourceFuel;
					if (!flag || fuelingPortSourceFuel < num)
					{
						num = fuelingPortSourceFuel;
						flag = true;
					}
				}
				if (!flag)
				{
					return 0f;
				}
				return num;
			}
		}

		
		// (get) Token: 0x06005198 RID: 20888 RVA: 0x001B5393 File Offset: 0x001B3593
		private int MaxLaunchDistance
		{
			get
			{
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					return 0;
				}
				return CompLaunchable.MaxLaunchDistanceAtFuelLevel(this.FuelInLeastFueledFuelingPortSource);
			}
		}

		
		// (get) Token: 0x06005199 RID: 20889 RVA: 0x001B53AC File Offset: 0x001B35AC
		private int MaxLaunchDistanceEverPossible
		{
			get
			{
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					return 0;
				}
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				float num = 0f;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					Building fuelingPortSource = transportersInGroup[i].Launchable.FuelingPortSource;
					if (fuelingPortSource != null)
					{
						num = Mathf.Max(num, fuelingPortSource.GetComp<CompRefuelable>().Props.fuelCapacity);
					}
				}
				return CompLaunchable.MaxLaunchDistanceAtFuelLevel(num);
			}
		}

		
		// (get) Token: 0x0600519A RID: 20890 RVA: 0x001B5414 File Offset: 0x001B3614
		private bool PodsHaveAnyPotentialCaravanOwner
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					ThingOwner innerContainer = transportersInGroup[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && CaravanUtility.IsOwner(pawn, Faction.OfPlayer))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{

			IEnumerator<Gizmo> enumerator = null;
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandLaunchGroup".Translate();
				command_Action.defaultDesc = "CommandLaunchGroupDesc".Translate();
				command_Action.icon = CompLaunchable.LaunchCommandTex;
				command_Action.alsoClickIfOtherInGroupClicked = false;
				command_Action.action = delegate
				{
					if (this.AnyInGroupHasAnythingLeftToLoad)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSendNotCompletelyLoadedPods".Translate(this.FirstThingLeftToLoadInGroup.LabelCapNoCount, this.FirstThingLeftToLoadInGroup), new Action(this.StartChoosingDestination), false, null));
						return;
					}
					this.StartChoosingDestination();
				};
				if (!this.AllInGroupConnectedToFuelingPort)
				{
					command_Action.Disable("CommandLaunchGroupFailNotConnectedToFuelingPort".Translate());
				}
				else if (!this.AllFuelingPortSourcesInGroupHaveAnyFuel)
				{
					command_Action.Disable("CommandLaunchGroupFailNoFuel".Translate());
				}
				else if (this.AnyInGroupIsUnderRoof)
				{
					command_Action.Disable("CommandLaunchGroupFailUnderRoof".Translate());
				}
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		
		public override string CompInspectStringExtra()
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return null;
			}
			if (!this.AllInGroupConnectedToFuelingPort)
			{
				return "NotReadyForLaunch".Translate() + ": " + "NotAllInGroupConnectedToFuelingPort".Translate() + ".";
			}
			if (!this.AllFuelingPortSourcesInGroupHaveAnyFuel)
			{
				return "NotReadyForLaunch".Translate() + ": " + "NotAllFuelingPortSourcesInGroupHaveAnyFuel".Translate() + ".";
			}
			if (this.AnyInGroupHasAnythingLeftToLoad)
			{
				return "NotReadyForLaunch".Translate() + ": " + "TransportPodInGroupHasSomethingLeftToLoad".Translate() + ".";
			}
			return "ReadyForLaunch".Translate();
		}

		
		private void StartChoosingDestination()
		{
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.parent));
			Find.WorldSelector.ClearSelection();
			int tile = this.parent.Map.Tile;
			Find.WorldTargeter.BeginTargeting(new Func<GlobalTargetInfo, bool>(this.ChoseWorldTarget), true, CompLaunchable.TargeterMouseAttachment, true, delegate
			{
				GenDraw.DrawWorldRadiusRing(tile, this.MaxLaunchDistance);
			}, delegate(GlobalTargetInfo target)
			{
				if (!target.IsValid)
				{
					return null;
				}
				int num = Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile, true, int.MaxValue);
				if (num > this.MaxLaunchDistance)
				{
					GUI.color = ColoredText.RedReadable;
					if (num > this.MaxLaunchDistanceEverPossible)
					{
						return "TransportPodDestinationBeyondMaximumRange".Translate();
					}
					return "TransportPodNotEnoughFuel".Translate();
				}
				else
				{
					IEnumerable<FloatMenuOption> transportPodsFloatMenuOptionsAt = this.GetTransportPodsFloatMenuOptionsAt(target.Tile);
					if (!transportPodsFloatMenuOptionsAt.Any<FloatMenuOption>())
					{
						return "";
					}
					if (transportPodsFloatMenuOptionsAt.Count<FloatMenuOption>() == 1)
					{
						if (transportPodsFloatMenuOptionsAt.First<FloatMenuOption>().Disabled)
						{
							GUI.color = ColoredText.RedReadable;
						}
						return transportPodsFloatMenuOptionsAt.First<FloatMenuOption>().Label;
					}
					MapParent mapParent = target.WorldObject as MapParent;
					if (mapParent != null)
					{
						return "ClickToSeeAvailableOrders_WorldObject".Translate(mapParent.LabelCap);
					}
					return "ClickToSeeAvailableOrders_Empty".Translate();
				}
			});
		}

		
		private bool ChoseWorldTarget(GlobalTargetInfo target)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return true;
			}
			if (!target.IsValid)
			{
				Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			int num = Find.WorldGrid.TraversalDistanceBetween(this.parent.Map.Tile, target.Tile, true, int.MaxValue);
			if (num > this.MaxLaunchDistance)
			{
				Messages.Message("MessageTransportPodsDestinationIsTooFar".Translate(CompLaunchable.FuelNeededToLaunchAtDist((float)num).ToString("0.#")), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			IEnumerable<FloatMenuOption> transportPodsFloatMenuOptionsAt = this.GetTransportPodsFloatMenuOptionsAt(target.Tile);
			if (!transportPodsFloatMenuOptionsAt.Any<FloatMenuOption>())
			{
				if (Find.World.Impassable(target.Tile))
				{
					Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				this.TryLaunch(target.Tile, null);
				return true;
			}
			else
			{
				if (transportPodsFloatMenuOptionsAt.Count<FloatMenuOption>() == 1)
				{
					if (!transportPodsFloatMenuOptionsAt.First<FloatMenuOption>().Disabled)
					{
						transportPodsFloatMenuOptionsAt.First<FloatMenuOption>().action();
					}
					return false;
				}
				Find.WindowStack.Add(new FloatMenu(transportPodsFloatMenuOptionsAt.ToList<FloatMenuOption>()));
				return false;
			}
		}

		
		public void TryLaunch(int destinationTile, TransportPodsArrivalAction arrivalAction)
		{
			if (!this.parent.Spawned)
			{
				Log.Error("Tried to launch " + this.parent + ", but it's unspawned.", false);
				return;
			}
			List<CompTransporter> transportersInGroup = this.TransportersInGroup;
			if (transportersInGroup == null)
			{
				Log.Error("Tried to launch " + this.parent + ", but it's not in any group.", false);
				return;
			}
			if (!this.LoadingInProgressOrReadyToLaunch || !this.AllInGroupConnectedToFuelingPort || !this.AllFuelingPortSourcesInGroupHaveAnyFuel)
			{
				return;
			}
			Map map = this.parent.Map;
			int num = Find.WorldGrid.TraversalDistanceBetween(map.Tile, destinationTile, true, int.MaxValue);
			if (num > this.MaxLaunchDistance)
			{
				return;
			}
			this.Transporter.TryRemoveLord(map);
			int groupID = this.Transporter.groupID;
			float amount = Mathf.Max(CompLaunchable.FuelNeededToLaunchAtDist((float)num), 1f);
			for (int i = 0; i < transportersInGroup.Count; i++)
			{
				CompTransporter compTransporter = transportersInGroup[i];
				Building fuelingPortSource = compTransporter.Launchable.FuelingPortSource;
				if (fuelingPortSource != null)
				{
					fuelingPortSource.TryGetComp<CompRefuelable>().ConsumeFuel(amount);
				}
				ThingOwner directlyHeldThings = compTransporter.GetDirectlyHeldThings();
				ActiveDropPod activeDropPod = (ActiveDropPod)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod, null);
				activeDropPod.Contents = new ActiveDropPodInfo();
				activeDropPod.Contents.innerContainer.TryAddRangeOrTransfer(directlyHeldThings, true, true);
				DropPodLeaving dropPodLeaving = (DropPodLeaving)SkyfallerMaker.MakeSkyfaller(ThingDefOf.DropPodLeaving, activeDropPod);
				dropPodLeaving.groupID = groupID;
				dropPodLeaving.destinationTile = destinationTile;
				dropPodLeaving.arrivalAction = arrivalAction;
				compTransporter.CleanUpLoadingVars(map);
				compTransporter.parent.Destroy(DestroyMode.Vanish);
				GenSpawn.Spawn(dropPodLeaving, compTransporter.parent.Position, map, WipeMode.Vanish);
			}
			CameraJumper.TryHideWorld();
		}

		
		public void Notify_FuelingPortSourceDeSpawned()
		{
			if (this.Transporter.CancelLoad())
			{
				Messages.Message("MessageTransportersLoadCanceled_FuelingPortGiverDeSpawned".Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		
		public static int MaxLaunchDistanceAtFuelLevel(float fuelLevel)
		{
			return Mathf.FloorToInt(fuelLevel / 2.25f);
		}

		
		public static float FuelNeededToLaunchAtDist(float dist)
		{
			return 2.25f * dist;
		}

		
		public IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptionsAt(int tile)
		{
			bool anything = false;
			if (TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(this.TransportersInGroup.Cast<IThingHolder>(), tile) && !Find.WorldObjects.AnySettlementBaseAt(tile) && !Find.WorldObjects.AnySiteAt(tile))
			{
				anything = true;
				yield return new FloatMenuOption("FormCaravanHere".Translate(), delegate
				{
					this.TryLaunch(tile, new TransportPodsArrivalAction_FormCaravan());
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			List<WorldObject> worldObjects = Find.WorldObjects.AllWorldObjects;
			int num;
			for (int i = 0; i < worldObjects.Count; i = num + 1)
			{
				if (worldObjects[i].Tile == tile)
				{
					foreach (FloatMenuOption floatMenuOption in worldObjects[i].GetTransportPodsFloatMenuOptions(this.TransportersInGroup.Cast<IThingHolder>(), this))
					{
						anything = true;
						yield return floatMenuOption;
					}
					IEnumerator<FloatMenuOption> enumerator = null;
				}
				num = i;
			}
			if (!anything && !Find.World.Impassable(tile))
			{
				yield return new FloatMenuOption("TransportPodsContentsWillBeLost".Translate(), delegate
				{
					this.TryLaunch(tile, null);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			yield break;
			yield break;
		}

		
		private CompTransporter cachedCompTransporter;

		
		public static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment", true);

		
		public static readonly Texture2D LaunchCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);

		
		private const float FuelPerTile = 2.25f;
	}
}
