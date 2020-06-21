using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001271 RID: 4721
	public class TravelingTransportPods : WorldObject, IThingHolder
	{
		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06006E77 RID: 28279 RVA: 0x0026904F File Offset: 0x0026724F
		private Vector3 Start
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.initialTile);
			}
		}

		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x06006E78 RID: 28280 RVA: 0x00269061 File Offset: 0x00267261
		private Vector3 End
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.destinationTile);
			}
		}

		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x06006E79 RID: 28281 RVA: 0x00269073 File Offset: 0x00267273
		public override Vector3 DrawPos
		{
			get
			{
				return Vector3.Slerp(this.Start, this.End, this.traveledPct);
			}
		}

		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x06006E7A RID: 28282 RVA: 0x0026908C File Offset: 0x0026728C
		private float TraveledPctStepPerTick
		{
			get
			{
				Vector3 start = this.Start;
				Vector3 end = this.End;
				if (start == end)
				{
					return 1f;
				}
				float num = GenMath.SphericalDistance(start.normalized, end.normalized);
				if (num == 0f)
				{
					return 1f;
				}
				return 0.00025f / num;
			}
		}

		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06006E7B RID: 28283 RVA: 0x002690E0 File Offset: 0x002672E0
		private bool PodsHaveAnyPotentialCaravanOwner
		{
			get
			{
				for (int i = 0; i < this.pods.Count; i++)
				{
					ThingOwner innerContainer = this.pods[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && CaravanUtility.IsOwner(pawn, base.Faction))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06006E7C RID: 28284 RVA: 0x00269148 File Offset: 0x00267348
		public bool PodsHaveAnyFreeColonist
		{
			get
			{
				for (int i = 0; i < this.pods.Count; i++)
				{
					ThingOwner innerContainer = this.pods[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && pawn.IsColonist && pawn.HostFaction == null)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001281 RID: 4737
		// (get) Token: 0x06006E7D RID: 28285 RVA: 0x002691B1 File Offset: 0x002673B1
		public IEnumerable<Pawn> Pawns
		{
			get
			{
				int num;
				for (int i = 0; i < this.pods.Count; i = num + 1)
				{
					ThingOwner things = this.pods[i].innerContainer;
					for (int j = 0; j < things.Count; j = num + 1)
					{
						Pawn pawn = things[j] as Pawn;
						if (pawn != null)
						{
							yield return pawn;
						}
						num = j;
					}
					things = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06006E7E RID: 28286 RVA: 0x002691C4 File Offset: 0x002673C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ActiveDropPodInfo>(ref this.pods, "pods", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<TransportPodsArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.arrived, "arrived", false, false);
			Scribe_Values.Look<int>(ref this.initialTile, "initialTile", 0, false);
			Scribe_Values.Look<float>(ref this.traveledPct, "traveledPct", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.pods.Count; i++)
				{
					this.pods[i].parent = this;
				}
			}
		}

		// Token: 0x06006E7F RID: 28287 RVA: 0x0026927E File Offset: 0x0026747E
		public override void PostAdd()
		{
			base.PostAdd();
			this.initialTile = base.Tile;
		}

		// Token: 0x06006E80 RID: 28288 RVA: 0x00269292 File Offset: 0x00267492
		public override void Tick()
		{
			base.Tick();
			this.traveledPct += this.TraveledPctStepPerTick;
			if (this.traveledPct >= 1f)
			{
				this.traveledPct = 1f;
				this.Arrived();
			}
		}

		// Token: 0x06006E81 RID: 28289 RVA: 0x002692CC File Offset: 0x002674CC
		public void AddPod(ActiveDropPodInfo contents, bool justLeftTheMap)
		{
			contents.parent = this;
			this.pods.Add(contents);
			ThingOwner innerContainer = contents.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Pawn pawn = innerContainer[i] as Pawn;
				if (pawn != null && !pawn.IsWorldPawn())
				{
					if (!base.Spawned)
					{
						Log.Warning("Passing pawn " + pawn + " to world, but the TravelingTransportPod is not spawned. This means that WorldPawns can discard this pawn which can cause bugs.", false);
					}
					if (justLeftTheMap)
					{
						pawn.ExitMap(false, Rot4.Invalid);
					}
					else
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
			contents.savePawnsWithReferenceMode = true;
		}

		// Token: 0x06006E82 RID: 28290 RVA: 0x00269360 File Offset: 0x00267560
		public bool ContainsPawn(Pawn p)
		{
			for (int i = 0; i < this.pods.Count; i++)
			{
				if (this.pods[i].innerContainer.Contains(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006E83 RID: 28291 RVA: 0x002693A0 File Offset: 0x002675A0
		private void Arrived()
		{
			if (this.arrived)
			{
				return;
			}
			this.arrived = true;
			if (this.arrivalAction == null || !this.arrivalAction.StillValid(this.pods.Cast<IThingHolder>(), this.destinationTile))
			{
				this.arrivalAction = null;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].Tile == this.destinationTile)
					{
						this.arrivalAction = new TransportPodsArrivalAction_LandInSpecificCell(maps[i].Parent, DropCellFinder.RandomDropSpot(maps[i]));
						break;
					}
				}
				if (this.arrivalAction == null)
				{
					if (TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(this.pods.Cast<IThingHolder>(), this.destinationTile))
					{
						this.arrivalAction = new TransportPodsArrivalAction_FormCaravan();
					}
					else
					{
						List<Caravan> caravans = Find.WorldObjects.Caravans;
						for (int j = 0; j < caravans.Count; j++)
						{
							if (caravans[j].Tile == this.destinationTile && TransportPodsArrivalAction_GiveToCaravan.CanGiveTo(this.pods.Cast<IThingHolder>(), caravans[j]))
							{
								this.arrivalAction = new TransportPodsArrivalAction_GiveToCaravan(caravans[j]);
								break;
							}
						}
					}
				}
			}
			if (this.arrivalAction != null && this.arrivalAction.ShouldUseLongEvent(this.pods, this.destinationTile))
			{
				LongEventHandler.QueueLongEvent(delegate
				{
					this.DoArrivalAction();
				}, "GeneratingMapForNewEncounter", false, null, true);
				return;
			}
			this.DoArrivalAction();
		}

		// Token: 0x06006E84 RID: 28292 RVA: 0x00269518 File Offset: 0x00267718
		private void DoArrivalAction()
		{
			for (int i = 0; i < this.pods.Count; i++)
			{
				this.pods[i].savePawnsWithReferenceMode = false;
				this.pods[i].parent = null;
			}
			if (this.arrivalAction != null)
			{
				try
				{
					this.arrivalAction.Arrived(this.pods, this.destinationTile);
				}
				catch (Exception arg)
				{
					Log.Error("Exception in transport pods arrival action: " + arg, false);
				}
				this.arrivalAction = null;
			}
			else
			{
				for (int j = 0; j < this.pods.Count; j++)
				{
					for (int k = 0; k < this.pods[j].innerContainer.Count; k++)
					{
						Pawn pawn = this.pods[j].innerContainer[k] as Pawn;
						if (pawn != null && (pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer))
						{
							PawnBanishUtility.Banish(pawn, this.destinationTile);
						}
					}
				}
				for (int l = 0; l < this.pods.Count; l++)
				{
					this.pods[l].innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
				}
				Messages.Message("MessageTransportPodsArrivedAndLost".Translate(), new GlobalTargetInfo(this.destinationTile), MessageTypeDefOf.NegativeEvent, true);
			}
			this.pods.Clear();
			this.Destroy();
		}

		// Token: 0x06006E85 RID: 28293 RVA: 0x00019EA1 File Offset: 0x000180A1
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06006E86 RID: 28294 RVA: 0x002696A0 File Offset: 0x002678A0
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			for (int i = 0; i < this.pods.Count; i++)
			{
				outChildren.Add(this.pods[i]);
			}
		}

		// Token: 0x0400442A RID: 17450
		public int destinationTile = -1;

		// Token: 0x0400442B RID: 17451
		public TransportPodsArrivalAction arrivalAction;

		// Token: 0x0400442C RID: 17452
		private List<ActiveDropPodInfo> pods = new List<ActiveDropPodInfo>();

		// Token: 0x0400442D RID: 17453
		private bool arrived;

		// Token: 0x0400442E RID: 17454
		private int initialTile = -1;

		// Token: 0x0400442F RID: 17455
		private float traveledPct;

		// Token: 0x04004430 RID: 17456
		private const float TravelSpeed = 0.00025f;
	}
}
