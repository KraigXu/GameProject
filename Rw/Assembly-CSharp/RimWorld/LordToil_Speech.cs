using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007AC RID: 1964
	public class LordToil_Speech : LordToil_Gathering
	{
		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06003305 RID: 13061 RVA: 0x0011B5A9 File Offset: 0x001197A9
		public LordToilData_Speech Data
		{
			get
			{
				return (LordToilData_Speech)this.data;
			}
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x0011B5B6 File Offset: 0x001197B6
		public LordToil_Speech(IntVec3 spot, GatheringDef gatheringDef, Pawn organizer) : base(spot, gatheringDef)
		{
			this.organizer = organizer;
			this.data = new LordToilData_Speech();
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x0011B5D4 File Offset: 0x001197D4
		public override void Init()
		{
			base.Init();
			this.Data.spectateRect = CellRect.CenteredOn(this.spot, 0);
			Rot4 rotation = this.spot.GetFirstThing(this.organizer.MapHeld).Rotation;
			SpectateRectSide asSpectateSide = rotation.Opposite.AsSpectateSide;
			this.Data.spectateRectAllowedSides = (SpectateRectSide.All & ~asSpectateSide);
			this.Data.spectateRectPreferredSide = rotation.AsSpectateSide;
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x0011B64C File Offset: 0x0011984C
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			if (p == this.organizer)
			{
				return DutyDefOf.GiveSpeech.hook;
			}
			return DutyDefOf.Spectate.hook;
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x0011B66C File Offset: 0x0011986C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn == this.organizer)
				{
					Building_Throne firstThing = this.spot.GetFirstThing(base.Map);
					pawn.mindState.duty = new PawnDuty(DutyDefOf.GiveSpeech, this.spot, firstThing, -1f);
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				else
				{
					PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Spectate);
					pawnDuty.spectateRect = this.Data.spectateRect;
					pawnDuty.spectateRectAllowedSides = this.Data.spectateRectAllowedSides;
					pawnDuty.spectateRectPreferredSide = this.Data.spectateRectPreferredSide;
					pawn.mindState.duty = pawnDuty;
				}
			}
		}

		// Token: 0x04001B80 RID: 7040
		public Pawn organizer;
	}
}
