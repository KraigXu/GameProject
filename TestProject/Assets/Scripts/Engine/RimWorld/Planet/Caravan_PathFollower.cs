using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001228 RID: 4648
	public class Caravan_PathFollower : IExposable
	{
		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06006C23 RID: 27683 RVA: 0x0025AECC File Offset: 0x002590CC
		public int Destination
		{
			get
			{
				return this.destTile;
			}
		}

		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06006C24 RID: 27684 RVA: 0x0025AED4 File Offset: 0x002590D4
		public bool Moving
		{
			get
			{
				return this.moving && this.caravan.Spawned;
			}
		}

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06006C25 RID: 27685 RVA: 0x0025AEEB File Offset: 0x002590EB
		public bool MovingNow
		{
			get
			{
				return this.Moving && !this.Paused && !this.caravan.CantMove;
			}
		}

		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06006C26 RID: 27686 RVA: 0x0025AF0D File Offset: 0x0025910D
		public CaravanArrivalAction ArrivalAction
		{
			get
			{
				if (!this.Moving)
				{
					return null;
				}
				return this.arrivalAction;
			}
		}

		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x06006C27 RID: 27687 RVA: 0x0025AF1F File Offset: 0x0025911F
		// (set) Token: 0x06006C28 RID: 27688 RVA: 0x0025AF34 File Offset: 0x00259134
		public bool Paused
		{
			get
			{
				return this.Moving && this.paused;
			}
			set
			{
				if (value == this.paused)
				{
					return;
				}
				if (!value)
				{
					this.paused = false;
				}
				else if (!this.Moving)
				{
					Log.Error("Tried to pause caravan movement of " + this.caravan.ToStringSafe<Caravan>() + " but it's not moving.", false);
				}
				else
				{
					this.paused = true;
				}
				this.caravan.Notify_DestinationOrPauseStatusChanged();
			}
		}

		// Token: 0x06006C29 RID: 27689 RVA: 0x0025AF93 File Offset: 0x00259193
		public Caravan_PathFollower(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06006C2A RID: 27690 RVA: 0x0025AFBC File Offset: 0x002591BC
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.moving, "moving", true, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			Scribe_Values.Look<int>(ref this.nextTile, "nextTile", 0, false);
			Scribe_Values.Look<int>(ref this.previousTileForDrawingIfInDoubt, "previousTileForDrawingIfInDoubt", 0, false);
			Scribe_Values.Look<float>(ref this.nextTileCostLeft, "nextTileCostLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.nextTileCostTotal, "nextTileCostTotal", 0f, false);
			Scribe_Values.Look<int>(ref this.destTile, "destTile", 0, false);
			Scribe_Deep.Look<CaravanArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && Current.ProgramState != ProgramState.Entry && this.moving && !this.StartPath(this.destTile, this.arrivalAction, true, false))
			{
				this.StopDead();
			}
		}

		// Token: 0x06006C2B RID: 27691 RVA: 0x0025B098 File Offset: 0x00259298
		public bool StartPath(int destTile, CaravanArrivalAction arrivalAction, bool repathImmediately = false, bool resetPauseStatus = true)
		{
			this.caravan.autoJoinable = false;
			if (resetPauseStatus)
			{
				this.paused = false;
			}
			if (arrivalAction != null && !arrivalAction.StillValid(this.caravan, destTile))
			{
				return false;
			}
			if (!this.IsPassable(this.caravan.Tile) && !this.TryRecoverFromUnwalkablePosition())
			{
				return false;
			}
			if (this.moving && this.curPath != null && this.destTile == destTile)
			{
				this.arrivalAction = arrivalAction;
				return true;
			}
			if (!this.caravan.CanReach(destTile))
			{
				this.PatherFailed();
				return false;
			}
			this.destTile = destTile;
			this.arrivalAction = arrivalAction;
			this.caravan.Notify_DestinationOrPauseStatusChanged();
			if (this.nextTile < 0 || !this.IsNextTilePassable())
			{
				this.nextTile = this.caravan.Tile;
				this.nextTileCostLeft = 0f;
				this.previousTileForDrawingIfInDoubt = -1;
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return true;
			}
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = true;
			if (repathImmediately && this.TrySetNewPath() && this.nextTileCostLeft <= 0f && this.moving)
			{
				this.TryEnterNextPathTile();
			}
			return true;
		}

		// Token: 0x06006C2C RID: 27692 RVA: 0x0025B1D0 File Offset: 0x002593D0
		public void StopDead()
		{
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = false;
			this.paused = false;
			this.nextTile = this.caravan.Tile;
			this.previousTileForDrawingIfInDoubt = -1;
			this.arrivalAction = null;
			this.nextTileCostLeft = 0f;
			this.caravan.Notify_DestinationOrPauseStatusChanged();
		}

		// Token: 0x06006C2D RID: 27693 RVA: 0x0025B23C File Offset: 0x0025943C
		public void PatherTick()
		{
			if (this.moving && this.arrivalAction != null && !this.arrivalAction.StillValid(this.caravan, this.Destination))
			{
				string failMessage = this.arrivalAction.StillValid(this.caravan, this.Destination).FailMessage;
				Messages.Message("MessageCaravanArrivalActionNoLongerValid".Translate(this.caravan.Name).CapitalizeFirst() + ((failMessage != null) ? (" " + failMessage) : ""), this.caravan, MessageTypeDefOf.NegativeEvent, true);
				this.StopDead();
			}
			if (this.caravan.CantMove || this.paused)
			{
				return;
			}
			if (this.nextTileCostLeft > 0f)
			{
				this.nextTileCostLeft -= this.CostToPayThisTick();
				return;
			}
			if (this.moving)
			{
				this.TryEnterNextPathTile();
			}
		}

		// Token: 0x06006C2E RID: 27694 RVA: 0x0025B33F File Offset: 0x0025953F
		public void Notify_Teleported_Int()
		{
			this.StopDead();
		}

		// Token: 0x06006C2F RID: 27695 RVA: 0x0025B347 File Offset: 0x00259547
		private bool IsPassable(int tile)
		{
			return !Find.World.Impassable(tile);
		}

		// Token: 0x06006C30 RID: 27696 RVA: 0x0025B357 File Offset: 0x00259557
		public bool IsNextTilePassable()
		{
			return this.IsPassable(this.nextTile);
		}

		// Token: 0x06006C31 RID: 27697 RVA: 0x0025B368 File Offset: 0x00259568
		private bool TryRecoverFromUnwalkablePosition()
		{
			int num;
			if (GenWorldClosest.TryFindClosestTile(this.caravan.Tile, (int t) => this.IsPassable(t), out num, 2147483647, true))
			{
				Log.Warning(string.Concat(new object[]
				{
					this.caravan,
					" on unwalkable tile ",
					this.caravan.Tile,
					". Teleporting to ",
					num
				}), false);
				this.caravan.Tile = num;
				this.caravan.Notify_Teleported();
				return true;
			}
			Log.Error(string.Concat(new object[]
			{
				this.caravan,
				" on unwalkable tile ",
				this.caravan.Tile,
				". Could not find walkable position nearby. Removed."
			}), false);
			this.caravan.Destroy();
			return false;
		}

		// Token: 0x06006C32 RID: 27698 RVA: 0x0025B444 File Offset: 0x00259644
		private void PatherArrived()
		{
			CaravanArrivalAction caravanArrivalAction = this.arrivalAction;
			this.StopDead();
			if (caravanArrivalAction != null && caravanArrivalAction.StillValid(this.caravan, this.caravan.Tile))
			{
				caravanArrivalAction.Arrived(this.caravan);
				return;
			}
			if (this.caravan.IsPlayerControlled && !this.caravan.VisibleToCameraNow())
			{
				Messages.Message("MessageCaravanArrivedAtDestination".Translate(this.caravan.Label), this.caravan, MessageTypeDefOf.TaskCompletion, true);
			}
		}

		// Token: 0x06006C33 RID: 27699 RVA: 0x0025B33F File Offset: 0x0025953F
		private void PatherFailed()
		{
			this.StopDead();
		}

		// Token: 0x06006C34 RID: 27700 RVA: 0x0025B4DC File Offset: 0x002596DC
		private void TryEnterNextPathTile()
		{
			if (!this.IsNextTilePassable())
			{
				this.PatherFailed();
				return;
			}
			this.caravan.Tile = this.nextTile;
			if (this.NeedNewPath() && !this.TrySetNewPath())
			{
				return;
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return;
			}
			if (this.curPath.NodesLeftCount == 0)
			{
				Log.Error(this.caravan + " ran out of path nodes. Force-arriving.", false);
				this.PatherArrived();
				return;
			}
			this.SetupMoveIntoNextTile();
		}

		// Token: 0x06006C35 RID: 27701 RVA: 0x0025B55C File Offset: 0x0025975C
		private void SetupMoveIntoNextTile()
		{
			if (this.curPath.NodesLeftCount < 2)
			{
				Log.Error(string.Concat(new object[]
				{
					this.caravan,
					" at ",
					this.caravan.Tile,
					" ran out of path nodes while pathing to ",
					this.destTile,
					"."
				}), false);
				this.PatherFailed();
				return;
			}
			this.nextTile = this.curPath.ConsumeNextNode();
			this.previousTileForDrawingIfInDoubt = -1;
			if (Find.World.Impassable(this.nextTile))
			{
				Log.Error(string.Concat(new object[]
				{
					this.caravan,
					" entering ",
					this.nextTile,
					" which is unwalkable."
				}), false);
			}
			int num = this.CostToMove(this.caravan.Tile, this.nextTile);
			this.nextTileCostTotal = (float)num;
			this.nextTileCostLeft = (float)num;
		}

		// Token: 0x06006C36 RID: 27702 RVA: 0x0025B65C File Offset: 0x0025985C
		private int CostToMove(int start, int end)
		{
			return Caravan_PathFollower.CostToMove(this.caravan, start, end, null);
		}

		// Token: 0x06006C37 RID: 27703 RVA: 0x0025B67F File Offset: 0x0025987F
		public static int CostToMove(Caravan caravan, int start, int end, int? ticksAbs = null)
		{
			return Caravan_PathFollower.CostToMove(caravan.TicksPerMove, start, end, ticksAbs, false, null, null);
		}

		// Token: 0x06006C38 RID: 27704 RVA: 0x0025B694 File Offset: 0x00259894
		public static int CostToMove(int caravanTicksPerMove, int start, int end, int? ticksAbs = null, bool perceivedStatic = false, StringBuilder explanation = null, string caravanTicksPerMoveExplanation = null)
		{
			if (start == end)
			{
				return 0;
			}
			if (explanation != null)
			{
				explanation.Append(caravanTicksPerMoveExplanation);
				explanation.AppendLine();
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float num;
			if (perceivedStatic && explanation == null)
			{
				num = Find.WorldPathGrid.PerceivedMovementDifficultyAt(end);
			}
			else
			{
				num = WorldPathGrid.CalculatedMovementDifficultyAt(end, perceivedStatic, ticksAbs, stringBuilder);
			}
			float roadMovementDifficultyMultiplier = Find.WorldGrid.GetRoadMovementDifficultyMultiplier(start, end, stringBuilder);
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.Append("TileMovementDifficulty".Translate() + ":");
				explanation.AppendLine();
				explanation.Append(stringBuilder.ToString().Indented("  "));
				explanation.AppendLine();
				explanation.Append("  = " + (num * roadMovementDifficultyMultiplier).ToString("0.#"));
			}
			int num2 = (int)((float)caravanTicksPerMove * num * roadMovementDifficultyMultiplier);
			num2 = Mathf.Clamp(num2, 1, 30000);
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.AppendLine();
				explanation.Append("FinalCaravanMovementSpeed".Translate() + ":");
				int num3 = Mathf.CeilToInt((float)num2 / 1f);
				explanation.AppendLine();
				explanation.Append(string.Concat(new string[]
				{
					"  ",
					(60000f / (float)caravanTicksPerMove).ToString("0.#"),
					" / ",
					(num * roadMovementDifficultyMultiplier).ToString("0.#"),
					" = ",
					(60000f / (float)num3).ToString("0.#"),
					" "
				}) + "TilesPerDay".Translate());
			}
			return num2;
		}

		// Token: 0x06006C39 RID: 27705 RVA: 0x0025B864 File Offset: 0x00259A64
		public static bool IsValidFinalPushDestination(int tile)
		{
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (allWorldObjects[i].Tile == tile && !(allWorldObjects[i] is Caravan))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006C3A RID: 27706 RVA: 0x0025B8B0 File Offset: 0x00259AB0
		private float CostToPayThisTick()
		{
			float num = 1f;
			if (DebugSettings.fastCaravans)
			{
				num = 100f;
			}
			if (num < this.nextTileCostTotal / 30000f)
			{
				num = this.nextTileCostTotal / 30000f;
			}
			return num;
		}

		// Token: 0x06006C3B RID: 27707 RVA: 0x0025B8F0 File Offset: 0x00259AF0
		private bool TrySetNewPath()
		{
			WorldPath worldPath = this.GenerateNewPath();
			if (!worldPath.Found)
			{
				this.PatherFailed();
				return false;
			}
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = worldPath;
			return true;
		}

		// Token: 0x06006C3C RID: 27708 RVA: 0x0025B930 File Offset: 0x00259B30
		private WorldPath GenerateNewPath()
		{
			int num = (this.moving && this.nextTile >= 0 && this.IsNextTilePassable()) ? this.nextTile : this.caravan.Tile;
			this.lastPathedTargetTile = this.destTile;
			WorldPath worldPath = Find.WorldPathFinder.FindPath(num, this.destTile, this.caravan, null);
			if (worldPath.Found && num != this.caravan.Tile)
			{
				if (worldPath.NodesLeftCount >= 2 && worldPath.Peek(1) == this.caravan.Tile)
				{
					worldPath.ConsumeNextNode();
					if (this.moving)
					{
						this.previousTileForDrawingIfInDoubt = this.nextTile;
						this.nextTile = this.caravan.Tile;
						this.nextTileCostLeft = this.nextTileCostTotal - this.nextTileCostLeft;
					}
				}
				else
				{
					worldPath.AddNodeAtStart(this.caravan.Tile);
				}
			}
			return worldPath;
		}

		// Token: 0x06006C3D RID: 27709 RVA: 0x0025BA15 File Offset: 0x00259C15
		private bool AtDestinationPosition()
		{
			return this.caravan.Tile == this.destTile;
		}

		// Token: 0x06006C3E RID: 27710 RVA: 0x0025BA2C File Offset: 0x00259C2C
		private bool NeedNewPath()
		{
			if (!this.moving)
			{
				return false;
			}
			if (this.curPath == null || !this.curPath.Found || this.curPath.NodesLeftCount == 0)
			{
				return true;
			}
			int num = 0;
			while (num < 20 && num < this.curPath.NodesLeftCount)
			{
				int tileID = this.curPath.Peek(num);
				if (Find.World.Impassable(tileID))
				{
					return true;
				}
				num++;
			}
			return false;
		}

		// Token: 0x0400435B RID: 17243
		private Caravan caravan;

		// Token: 0x0400435C RID: 17244
		private bool moving;

		// Token: 0x0400435D RID: 17245
		private bool paused;

		// Token: 0x0400435E RID: 17246
		public int nextTile = -1;

		// Token: 0x0400435F RID: 17247
		public int previousTileForDrawingIfInDoubt = -1;

		// Token: 0x04004360 RID: 17248
		public float nextTileCostLeft;

		// Token: 0x04004361 RID: 17249
		public float nextTileCostTotal = 1f;

		// Token: 0x04004362 RID: 17250
		private int destTile;

		// Token: 0x04004363 RID: 17251
		private CaravanArrivalAction arrivalAction;

		// Token: 0x04004364 RID: 17252
		public WorldPath curPath;

		// Token: 0x04004365 RID: 17253
		public int lastPathedTargetTile;

		// Token: 0x04004366 RID: 17254
		public const int MaxMoveTicks = 30000;

		// Token: 0x04004367 RID: 17255
		private const int MaxCheckAheadNodes = 20;

		// Token: 0x04004368 RID: 17256
		private const int MinCostWalk = 50;

		// Token: 0x04004369 RID: 17257
		private const int MinCostAmble = 60;

		// Token: 0x0400436A RID: 17258
		public const float DefaultPathCostToPayPerTick = 1f;

		// Token: 0x0400436B RID: 17259
		public const int FinalNoRestPushMaxDurationTicks = 10000;
	}
}
