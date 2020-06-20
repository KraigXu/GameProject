using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000576 RID: 1398
	public class Pawn_PathFollower : IExposable
	{
		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002775 RID: 10101 RVA: 0x000E71B8 File Offset: 0x000E53B8
		public LocalTargetInfo Destination
		{
			get
			{
				return this.destination;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06002776 RID: 10102 RVA: 0x000E71C0 File Offset: 0x000E53C0
		public bool Moving
		{
			get
			{
				return this.moving;
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06002777 RID: 10103 RVA: 0x000E71C8 File Offset: 0x000E53C8
		public bool MovingNow
		{
			get
			{
				return this.Moving && !this.WillCollideWithPawnOnNextPathCell();
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06002778 RID: 10104 RVA: 0x000E71E0 File Offset: 0x000E53E0
		public IntVec3 LastPassableCellInPath
		{
			get
			{
				if (!this.Moving || this.curPath == null)
				{
					return IntVec3.Invalid;
				}
				if (!this.Destination.Cell.Impassable(this.pawn.Map))
				{
					return this.Destination.Cell;
				}
				List<IntVec3> nodesReversed = this.curPath.NodesReversed;
				for (int i = 0; i < nodesReversed.Count; i++)
				{
					if (!nodesReversed[i].Impassable(this.pawn.Map))
					{
						return nodesReversed[i];
					}
				}
				if (!this.pawn.Position.Impassable(this.pawn.Map))
				{
					return this.pawn.Position;
				}
				return IntVec3.Invalid;
			}
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x000E72A0 File Offset: 0x000E54A0
		public Pawn_PathFollower(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x000E72F4 File Offset: 0x000E54F4
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.moving, "moving", true, false);
			Scribe_Values.Look<IntVec3>(ref this.nextCell, "nextCell", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.nextCellCostLeft, "nextCellCostLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.nextCellCostTotal, "nextCellCostInitial", 0f, false);
			Scribe_Values.Look<PathEndMode>(ref this.peMode, "peMode", PathEndMode.None, false);
			Scribe_Values.Look<int>(ref this.cellsUntilClamor, "cellsUntilClamor", 0, false);
			Scribe_Values.Look<int>(ref this.lastMovedTick, "lastMovedTick", -999999, false);
			if (this.moving)
			{
				Scribe_TargetInfo.Look(ref this.destination, "destination");
			}
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x000E73AC File Offset: 0x000E55AC
		public void StartPath(LocalTargetInfo dest, PathEndMode peMode)
		{
			dest = (LocalTargetInfo)GenPath.ResolvePathMode(this.pawn, dest.ToTargetInfo(this.pawn.Map), ref peMode);
			if (dest.HasThing && dest.ThingDestroyed)
			{
				Log.Error(this.pawn + " pathing to destroyed thing " + dest.Thing, false);
				this.PatherFailed();
				return;
			}
			if (!this.PawnCanOccupy(this.pawn.Position) && !this.TryRecoverFromUnwalkablePosition(true))
			{
				return;
			}
			if (this.moving && this.curPath != null && this.destination == dest && this.peMode == peMode)
			{
				return;
			}
			if (!this.pawn.Map.reachability.CanReach(this.pawn.Position, dest, peMode, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
			{
				this.PatherFailed();
				return;
			}
			this.peMode = peMode;
			this.destination = dest;
			if (!this.IsNextCellWalkable() || this.NextCellDoorToWaitForOrManuallyOpen() != null || this.nextCellCostLeft == this.nextCellCostTotal)
			{
				this.ResetToCurrentPosition();
			}
			PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation = this.pawn.Map.pawnDestinationReservationManager.MostRecentReservationFor(this.pawn);
			if (pawnDestinationReservation != null && ((this.destination.HasThing && pawnDestinationReservation.target != this.destination.Cell) || (pawnDestinationReservation.job != this.pawn.CurJob && pawnDestinationReservation.target != this.destination.Cell)))
			{
				this.pawn.Map.pawnDestinationReservationManager.ObsoleteAllClaimedBy(this.pawn);
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return;
			}
			if (this.pawn.Downed)
			{
				Log.Error(this.pawn.LabelCap + " tried to path while downed. This should never happen. curJob=" + this.pawn.CurJob.ToStringSafe<Job>(), false);
				this.PatherFailed();
				return;
			}
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = true;
			this.pawn.jobs.posture = PawnPosture.Standing;
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x000E75C9 File Offset: 0x000E57C9
		public void StopDead()
		{
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = false;
			this.nextCell = this.pawn.Position;
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000E7600 File Offset: 0x000E5800
		public void PatherTick()
		{
			if (this.WillCollideWithPawnAt(this.pawn.Position))
			{
				if (!this.FailedToFindCloseUnoccupiedCellRecently())
				{
					IntVec3 intVec;
					if (CellFinder.TryFindBestPawnStandCell(this.pawn, out intVec, true) && intVec != this.pawn.Position)
					{
						this.pawn.Position = intVec;
						this.ResetToCurrentPosition();
						if (this.moving && this.TrySetNewPath())
						{
							this.TryEnterNextPathCell();
							return;
						}
					}
					else
					{
						this.failedToFindCloseUnoccupiedCellTicks = Find.TickManager.TicksGame;
					}
				}
				return;
			}
			if (this.pawn.stances.FullBodyBusy)
			{
				return;
			}
			if (this.moving && this.WillCollideWithPawnOnNextPathCell())
			{
				this.nextCellCostLeft = this.nextCellCostTotal;
				if (((this.curPath != null && this.curPath.NodesLeftCount < 30) || PawnUtility.AnyPawnBlockingPathAt(this.nextCell, this.pawn, false, true, false)) && !this.BestPathHadPawnsInTheWayRecently() && this.TrySetNewPath())
				{
					this.ResetToCurrentPosition();
					this.TryEnterNextPathCell();
					return;
				}
				if (Find.TickManager.TicksGame - this.lastMovedTick >= 180)
				{
					Pawn pawn = PawnUtility.PawnBlockingPathAt(this.nextCell, this.pawn, false, false, false);
					if (pawn != null && this.pawn.HostileTo(pawn) && this.pawn.TryGetAttackVerb(pawn, false) != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, pawn);
						job.maxNumMeleeAttacks = 1;
						job.expiryInterval = 300;
						this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, null, false, false);
						return;
					}
				}
				return;
			}
			else
			{
				this.lastMovedTick = Find.TickManager.TicksGame;
				if (this.nextCellCostLeft > 0f)
				{
					this.nextCellCostLeft -= this.CostToPayThisTick();
					return;
				}
				if (this.moving)
				{
					this.TryEnterNextPathCell();
				}
				return;
			}
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000E77DB File Offset: 0x000E59DB
		public void TryResumePathingAfterLoading()
		{
			if (this.moving)
			{
				this.StartPath(this.destination, this.peMode);
			}
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000E77F7 File Offset: 0x000E59F7
		public void Notify_Teleported_Int()
		{
			this.StopDead();
			this.ResetToCurrentPosition();
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000E7805 File Offset: 0x000E5A05
		public void ResetToCurrentPosition()
		{
			this.nextCell = this.pawn.Position;
			this.nextCellCostLeft = 0f;
			this.nextCellCostTotal = 1f;
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000E7830 File Offset: 0x000E5A30
		private bool PawnCanOccupy(IntVec3 c)
		{
			if (!c.Walkable(this.pawn.Map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(this.pawn.Map);
			if (edifice != null)
			{
				Building_Door building_Door = edifice as Building_Door;
				if (building_Door != null && !building_Door.PawnCanOpen(this.pawn) && !building_Door.Open)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000E788C File Offset: 0x000E5A8C
		public Building BuildingBlockingNextPathCell()
		{
			Building edifice = this.nextCell.GetEdifice(this.pawn.Map);
			if (edifice != null && edifice.BlocksPawn(this.pawn))
			{
				return edifice;
			}
			return null;
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000E78C4 File Offset: 0x000E5AC4
		public bool WillCollideWithPawnOnNextPathCell()
		{
			return this.WillCollideWithPawnAt(this.nextCell);
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000E78D2 File Offset: 0x000E5AD2
		private bool IsNextCellWalkable()
		{
			return this.nextCell.Walkable(this.pawn.Map) && !this.WillCollideWithPawnAt(this.nextCell);
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x000E78FF File Offset: 0x000E5AFF
		private bool WillCollideWithPawnAt(IntVec3 c)
		{
			return PawnUtility.ShouldCollideWithPawns(this.pawn) && PawnUtility.AnyPawnBlockingPathAt(c, this.pawn, false, false, false);
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000E7920 File Offset: 0x000E5B20
		public Building_Door NextCellDoorToWaitForOrManuallyOpen()
		{
			Building_Door building_Door = this.pawn.Map.thingGrid.ThingAt<Building_Door>(this.nextCell);
			if (building_Door != null && building_Door.SlowsPawns && (!building_Door.Open || building_Door.TicksTillFullyOpened > 0) && building_Door.PawnCanOpen(this.pawn))
			{
				return building_Door;
			}
			return null;
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x000E7976 File Offset: 0x000E5B76
		public void PatherDraw()
		{
			if (DebugViewSettings.drawPaths && this.curPath != null && Find.Selector.IsSelected(this.pawn))
			{
				this.curPath.DrawPath(this.pawn);
			}
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x000E79AA File Offset: 0x000E5BAA
		public bool MovedRecently(int ticks)
		{
			return Find.TickManager.TicksGame - this.lastMovedTick <= ticks;
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000E79C4 File Offset: 0x000E5BC4
		public bool TryRecoverFromUnwalkablePosition(bool error = true)
		{
			bool flag = false;
			int i = 0;
			while (i < GenRadial.RadialPattern.Length)
			{
				IntVec3 intVec = this.pawn.Position + GenRadial.RadialPattern[i];
				if (this.PawnCanOccupy(intVec))
				{
					if (intVec == this.pawn.Position)
					{
						return true;
					}
					if (error)
					{
						Log.Warning(string.Concat(new object[]
						{
							this.pawn,
							" on unwalkable cell ",
							this.pawn.Position,
							". Teleporting to ",
							intVec
						}), false);
					}
					this.pawn.Position = intVec;
					this.pawn.Notify_Teleported(true, false);
					flag = true;
					break;
				}
				else
				{
					i++;
				}
			}
			if (!flag)
			{
				this.pawn.Destroy(DestroyMode.Vanish);
				Log.Error(string.Concat(new object[]
				{
					this.pawn.ToStringSafe<Pawn>(),
					" on unwalkable cell ",
					this.pawn.Position,
					". Could not find walkable position nearby. Destroyed."
				}), false);
			}
			return flag;
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x000E7ADD File Offset: 0x000E5CDD
		private void PatherArrived()
		{
			this.StopDead();
			if (this.pawn.jobs.curJob != null)
			{
				this.pawn.jobs.curDriver.Notify_PatherArrived();
			}
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x000E7B0C File Offset: 0x000E5D0C
		private void PatherFailed()
		{
			this.StopDead();
			this.pawn.jobs.curDriver.Notify_PatherFailed();
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x000E7B2C File Offset: 0x000E5D2C
		private void TryEnterNextPathCell()
		{
			Building building = this.BuildingBlockingNextPathCell();
			if (building != null)
			{
				Building_Door building_Door = building as Building_Door;
				if (building_Door == null || !building_Door.FreePassage)
				{
					if ((this.pawn.CurJob != null && this.pawn.CurJob.canBash) || this.pawn.HostileTo(building))
					{
						Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, building);
						job.expiryInterval = 300;
						this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, null, false, false);
						return;
					}
					this.PatherFailed();
					return;
				}
			}
			Building_Door building_Door2 = this.NextCellDoorToWaitForOrManuallyOpen();
			if (building_Door2 != null)
			{
				if (!building_Door2.Open)
				{
					building_Door2.StartManualOpenBy(this.pawn);
				}
				Stance_Cooldown stance_Cooldown = new Stance_Cooldown(building_Door2.TicksTillFullyOpened, building_Door2, null);
				stance_Cooldown.neverAimWeapon = true;
				this.pawn.stances.SetStance(stance_Cooldown);
				building_Door2.CheckFriendlyTouched(this.pawn);
				return;
			}
			this.lastCell = this.pawn.Position;
			this.pawn.Position = this.nextCell;
			if (this.pawn.RaceProps.Humanlike)
			{
				this.cellsUntilClamor--;
				if (this.cellsUntilClamor <= 0)
				{
					GenClamor.DoClamor(this.pawn, 7f, ClamorDefOf.Movement);
					this.cellsUntilClamor = 12;
				}
			}
			this.pawn.filth.Notify_EnteredNewCell();
			if (this.pawn.BodySize > 0.9f)
			{
				this.pawn.Map.snowGrid.AddDepth(this.pawn.Position, -0.001f);
			}
			Building_Door building_Door3 = this.pawn.Map.thingGrid.ThingAt<Building_Door>(this.lastCell);
			if (building_Door3 != null && !this.pawn.HostileTo(building_Door3))
			{
				building_Door3.CheckFriendlyTouched(this.pawn);
				if (!building_Door3.BlockedOpenMomentary && !building_Door3.HoldOpen && building_Door3.SlowsPawns && building_Door3.PawnCanOpen(this.pawn))
				{
					building_Door3.StartManualCloseBy(this.pawn);
					return;
				}
			}
			if (this.NeedNewPath() && !this.TrySetNewPath())
			{
				return;
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return;
			}
			this.SetupMoveIntoNextCell();
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x000E7D68 File Offset: 0x000E5F68
		private void SetupMoveIntoNextCell()
		{
			if (this.curPath.NodesLeftCount <= 1)
			{
				Log.Error(string.Concat(new object[]
				{
					this.pawn,
					" at ",
					this.pawn.Position,
					" ran out of path nodes while pathing to ",
					this.destination,
					"."
				}), false);
				this.PatherFailed();
				return;
			}
			this.nextCell = this.curPath.ConsumeNextNode();
			if (!this.nextCell.Walkable(this.pawn.Map))
			{
				Log.Error(string.Concat(new object[]
				{
					this.pawn,
					" entering ",
					this.nextCell,
					" which is unwalkable."
				}), false);
			}
			int num = this.CostToMoveIntoCell(this.nextCell);
			this.nextCellCostTotal = (float)num;
			this.nextCellCostLeft = (float)num;
			Building_Door building_Door = this.pawn.Map.thingGrid.ThingAt<Building_Door>(this.nextCell);
			if (building_Door != null)
			{
				building_Door.Notify_PawnApproaching(this.pawn, num);
			}
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000E7E87 File Offset: 0x000E6087
		private int CostToMoveIntoCell(IntVec3 c)
		{
			return Pawn_PathFollower.CostToMoveIntoCell(this.pawn, c);
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x000E7E98 File Offset: 0x000E6098
		private static int CostToMoveIntoCell(Pawn pawn, IntVec3 c)
		{
			int num;
			if (c.x == pawn.Position.x || c.z == pawn.Position.z)
			{
				num = pawn.TicksPerMoveCardinal;
			}
			else
			{
				num = pawn.TicksPerMoveDiagonal;
			}
			num += pawn.Map.pathGrid.CalculatedCostAt(c, false, pawn.Position);
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice != null)
			{
				num += (int)edifice.PathWalkCostFor(pawn);
			}
			if (num > 450)
			{
				num = 450;
			}
			if (pawn.CurJob != null)
			{
				Pawn locomotionUrgencySameAs = pawn.jobs.curDriver.locomotionUrgencySameAs;
				if (locomotionUrgencySameAs != null && locomotionUrgencySameAs != pawn && locomotionUrgencySameAs.Spawned)
				{
					int num2 = Pawn_PathFollower.CostToMoveIntoCell(locomotionUrgencySameAs, c);
					if (num < num2)
					{
						num = num2;
					}
				}
				else
				{
					switch (pawn.jobs.curJob.locomotionUrgency)
					{
					case LocomotionUrgency.Amble:
						num *= 3;
						if (num < 60)
						{
							num = 60;
						}
						break;
					case LocomotionUrgency.Walk:
						num *= 2;
						if (num < 50)
						{
							num = 50;
						}
						break;
					case LocomotionUrgency.Jog:
						num = num;
						break;
					case LocomotionUrgency.Sprint:
						num = Mathf.RoundToInt((float)num * 0.75f);
						break;
					}
				}
			}
			return Mathf.Max(num, 1);
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x000E7FBC File Offset: 0x000E61BC
		private float CostToPayThisTick()
		{
			float num = 1f;
			if (this.pawn.stances.Staggered)
			{
				num *= 0.17f;
			}
			if (num < this.nextCellCostTotal / 450f)
			{
				num = this.nextCellCostTotal / 450f;
			}
			return num;
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000E8008 File Offset: 0x000E6208
		private bool TrySetNewPath()
		{
			PawnPath pawnPath = this.GenerateNewPath();
			if (!pawnPath.Found)
			{
				this.PatherFailed();
				return false;
			}
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = pawnPath;
			int num = 0;
			while (num < 20 && num < this.curPath.NodesLeftCount)
			{
				IntVec3 c = this.curPath.Peek(num);
				if (PawnUtility.ShouldCollideWithPawns(this.pawn) && PawnUtility.AnyPawnBlockingPathAt(c, this.pawn, false, false, false))
				{
					this.foundPathWhichCollidesWithPawns = Find.TickManager.TicksGame;
				}
				if (PawnUtility.KnownDangerAt(c, this.pawn.Map, this.pawn))
				{
					this.foundPathWithDanger = Find.TickManager.TicksGame;
				}
				if (this.foundPathWhichCollidesWithPawns == Find.TickManager.TicksGame && this.foundPathWithDanger == Find.TickManager.TicksGame)
				{
					break;
				}
				num++;
			}
			return true;
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000E80F0 File Offset: 0x000E62F0
		private PawnPath GenerateNewPath()
		{
			this.lastPathedTargetPosition = this.destination.Cell;
			return this.pawn.Map.pathFinder.FindPath(this.pawn.Position, this.destination, this.pawn, this.peMode);
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x000E8140 File Offset: 0x000E6340
		private bool AtDestinationPosition()
		{
			return this.pawn.CanReachImmediate(this.destination, this.peMode);
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000E815C File Offset: 0x000E635C
		private bool NeedNewPath()
		{
			if (!this.destination.IsValid || this.curPath == null || !this.curPath.Found || this.curPath.NodesLeftCount == 0)
			{
				return true;
			}
			if (this.destination.HasThing && this.destination.Thing.Map != this.pawn.Map)
			{
				return true;
			}
			if ((this.pawn.Position.InHorDistOf(this.curPath.LastNode, 15f) || this.pawn.Position.InHorDistOf(this.destination.Cell, 15f)) && !ReachabilityImmediate.CanReachImmediate(this.curPath.LastNode, this.destination, this.pawn.Map, this.peMode, this.pawn))
			{
				return true;
			}
			if (this.curPath.UsedRegionHeuristics && this.curPath.NodesConsumedCount >= 75)
			{
				return true;
			}
			if (this.lastPathedTargetPosition != this.destination.Cell)
			{
				float num = (float)(this.pawn.Position - this.destination.Cell).LengthHorizontalSquared;
				float num2;
				if (num > 900f)
				{
					num2 = 10f;
				}
				else if (num > 289f)
				{
					num2 = 5f;
				}
				else if (num > 100f)
				{
					num2 = 3f;
				}
				else if (num > 49f)
				{
					num2 = 2f;
				}
				else
				{
					num2 = 0.5f;
				}
				if ((float)(this.lastPathedTargetPosition - this.destination.Cell).LengthHorizontalSquared > num2 * num2)
				{
					return true;
				}
			}
			bool flag = PawnUtility.ShouldCollideWithPawns(this.pawn);
			bool flag2 = this.curPath.NodesLeftCount < 30;
			IntVec3 intVec = IntVec3.Invalid;
			int num3 = 0;
			while (num3 < 20 && num3 < this.curPath.NodesLeftCount)
			{
				IntVec3 intVec2 = this.curPath.Peek(num3);
				if (!intVec2.Walkable(this.pawn.Map))
				{
					return true;
				}
				if (flag && !this.BestPathHadPawnsInTheWayRecently() && (PawnUtility.AnyPawnBlockingPathAt(intVec2, this.pawn, false, true, false) || (flag2 && PawnUtility.AnyPawnBlockingPathAt(intVec2, this.pawn, false, false, false))))
				{
					return true;
				}
				if (!this.BestPathHadDangerRecently() && PawnUtility.KnownDangerAt(intVec2, this.pawn.Map, this.pawn))
				{
					return true;
				}
				Building_Door building_Door = intVec2.GetEdifice(this.pawn.Map) as Building_Door;
				if (building_Door != null)
				{
					if (!building_Door.CanPhysicallyPass(this.pawn) && !this.pawn.HostileTo(building_Door))
					{
						return true;
					}
					if (building_Door.IsForbiddenToPass(this.pawn))
					{
						return true;
					}
				}
				if (num3 != 0 && intVec2.AdjacentToDiagonal(intVec) && (PathFinder.BlocksDiagonalMovement(intVec2.x, intVec.z, this.pawn.Map) || PathFinder.BlocksDiagonalMovement(intVec.x, intVec2.z, this.pawn.Map)))
				{
					return true;
				}
				intVec = intVec2;
				num3++;
			}
			return false;
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x000E847C File Offset: 0x000E667C
		private bool BestPathHadPawnsInTheWayRecently()
		{
			return this.foundPathWhichCollidesWithPawns + 240 > Find.TickManager.TicksGame;
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000E8496 File Offset: 0x000E6696
		private bool BestPathHadDangerRecently()
		{
			return this.foundPathWithDanger + 240 > Find.TickManager.TicksGame;
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000E84B0 File Offset: 0x000E66B0
		private bool FailedToFindCloseUnoccupiedCellRecently()
		{
			return this.failedToFindCloseUnoccupiedCellTicks + 100 > Find.TickManager.TicksGame;
		}

		// Token: 0x04001791 RID: 6033
		protected Pawn pawn;

		// Token: 0x04001792 RID: 6034
		private bool moving;

		// Token: 0x04001793 RID: 6035
		public IntVec3 nextCell;

		// Token: 0x04001794 RID: 6036
		private IntVec3 lastCell;

		// Token: 0x04001795 RID: 6037
		public float nextCellCostLeft;

		// Token: 0x04001796 RID: 6038
		public float nextCellCostTotal = 1f;

		// Token: 0x04001797 RID: 6039
		private int cellsUntilClamor;

		// Token: 0x04001798 RID: 6040
		private int lastMovedTick = -999999;

		// Token: 0x04001799 RID: 6041
		private LocalTargetInfo destination;

		// Token: 0x0400179A RID: 6042
		private PathEndMode peMode;

		// Token: 0x0400179B RID: 6043
		public PawnPath curPath;

		// Token: 0x0400179C RID: 6044
		public IntVec3 lastPathedTargetPosition;

		// Token: 0x0400179D RID: 6045
		private int foundPathWhichCollidesWithPawns = -999999;

		// Token: 0x0400179E RID: 6046
		private int foundPathWithDanger = -999999;

		// Token: 0x0400179F RID: 6047
		private int failedToFindCloseUnoccupiedCellTicks = -999999;

		// Token: 0x040017A0 RID: 6048
		private const int MaxMoveTicks = 450;

		// Token: 0x040017A1 RID: 6049
		private const int MaxCheckAheadNodes = 20;

		// Token: 0x040017A2 RID: 6050
		private const float SnowReductionFromWalking = 0.001f;

		// Token: 0x040017A3 RID: 6051
		private const int ClamorCellsInterval = 12;

		// Token: 0x040017A4 RID: 6052
		private const int MinCostWalk = 50;

		// Token: 0x040017A5 RID: 6053
		private const int MinCostAmble = 60;

		// Token: 0x040017A6 RID: 6054
		private const float StaggerMoveSpeedFactor = 0.17f;

		// Token: 0x040017A7 RID: 6055
		private const int CheckForMovingCollidingPawnsIfCloserToTargetThanX = 30;

		// Token: 0x040017A8 RID: 6056
		private const int AttackBlockingHostilePawnAfterTicks = 180;
	}
}
