using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007A8 RID: 1960
	public class LordToil_MarriageCeremony : LordToil
	{
		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x060032F3 RID: 13043 RVA: 0x0011B0E7 File Offset: 0x001192E7
		public LordToilData_MarriageCeremony Data
		{
			get
			{
				return (LordToilData_MarriageCeremony)this.data;
			}
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x0011B0F4 File Offset: 0x001192F4
		public LordToil_MarriageCeremony(Pawn firstPawn, Pawn secondPawn, IntVec3 spot)
		{
			this.firstPawn = firstPawn;
			this.secondPawn = secondPawn;
			this.spot = spot;
			this.data = new LordToilData_MarriageCeremony();
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x0011B11C File Offset: 0x0011931C
		public override void Init()
		{
			base.Init();
			this.Data.spectateRect = this.CalculateSpectateRect();
			SpectateRectSide allowedSides = SpectateRectSide.All;
			if (this.Data.spectateRect.Width > this.Data.spectateRect.Height)
			{
				allowedSides = SpectateRectSide.Vertical;
			}
			else if (this.Data.spectateRect.Height > this.Data.spectateRect.Width)
			{
				allowedSides = SpectateRectSide.Horizontal;
			}
			this.Data.spectateRectAllowedSides = SpectatorCellFinder.FindSingleBestSide(this.Data.spectateRect, base.Map, allowedSides, 1);
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x0011B1B1 File Offset: 0x001193B1
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			if (this.IsFiance(p))
			{
				return DutyDefOf.MarryPawn.hook;
			}
			return DutyDefOf.Spectate.hook;
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x0011B1D4 File Offset: 0x001193D4
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (this.IsFiance(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.MarryPawn, this.FianceStandingSpotFor(pawn), -1f);
				}
				else
				{
					PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Spectate);
					pawnDuty.spectateRect = this.Data.spectateRect;
					pawnDuty.spectateRectAllowedSides = this.Data.spectateRectAllowedSides;
					pawn.mindState.duty = pawnDuty;
				}
			}
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x0011B27E File Offset: 0x0011947E
		private bool IsFiance(Pawn p)
		{
			return p == this.firstPawn || p == this.secondPawn;
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x0011B294 File Offset: 0x00119494
		public IntVec3 FianceStandingSpotFor(Pawn pawn)
		{
			Pawn pawn2;
			if (this.firstPawn == pawn)
			{
				pawn2 = this.secondPawn;
			}
			else
			{
				if (this.secondPawn != pawn)
				{
					Log.Warning("Called ExactStandingSpotFor but it's not this pawn's ceremony.", false);
					return IntVec3.Invalid;
				}
				pawn2 = this.firstPawn;
			}
			if (pawn.thingIDNumber < pawn2.thingIDNumber)
			{
				return this.spot;
			}
			if (this.GetMarriageSpotAt(this.spot) != null)
			{
				return this.FindCellForOtherPawnAtMarriageSpot(this.spot);
			}
			return this.spot + LordToil_MarriageCeremony.OtherFianceNoMarriageSpotCellOffset;
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x0011B318 File Offset: 0x00119518
		private Thing GetMarriageSpotAt(IntVec3 cell)
		{
			return cell.GetThingList(base.Map).Find((Thing x) => x.def == ThingDefOf.MarriageSpot);
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x0011B34C File Offset: 0x0011954C
		private IntVec3 FindCellForOtherPawnAtMarriageSpot(IntVec3 cell)
		{
			CellRect cellRect = this.GetMarriageSpotAt(cell).OccupiedRect();
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					if (cell.x != i || cell.z != j)
					{
						return new IntVec3(i, 0, j);
					}
				}
			}
			Log.Warning("Marriage spot is 1x1. There's no place for 2 pawns.", false);
			return IntVec3.Invalid;
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x0011B3C0 File Offset: 0x001195C0
		private CellRect CalculateSpectateRect()
		{
			IntVec3 first = this.FianceStandingSpotFor(this.firstPawn);
			IntVec3 second = this.FianceStandingSpotFor(this.secondPawn);
			return CellRect.FromLimits(first, second);
		}

		// Token: 0x04001B77 RID: 7031
		private Pawn firstPawn;

		// Token: 0x04001B78 RID: 7032
		private Pawn secondPawn;

		// Token: 0x04001B79 RID: 7033
		private IntVec3 spot;

		// Token: 0x04001B7A RID: 7034
		public static readonly IntVec3 OtherFianceNoMarriageSpotCellOffset = new IntVec3(-1, 0, 0);
	}
}
