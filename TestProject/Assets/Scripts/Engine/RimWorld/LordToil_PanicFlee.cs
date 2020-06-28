using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200079D RID: 1949
	public class LordToil_PanicFlee : LordToil
	{
		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x060032BA RID: 12986 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x060032BB RID: 12987 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x00119F08 File Offset: 0x00118108
		public override void Init()
		{
			base.Init();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!this.HasFleeingDuty(pawn) || pawn.mindState.duty.def == DutyDefOf.ExitMapRandom)
				{
					pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.PanicFlee, null, false, false, null, false);
				}
			}
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x00119F84 File Offset: 0x00118184
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!this.HasFleeingDuty(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapRandom);
				}
			}
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x00119FDC File Offset: 0x001181DC
		private bool HasFleeingDuty(Pawn pawn)
		{
			return pawn.mindState.duty != null && (pawn.mindState.duty.def == DutyDefOf.ExitMapRandom || pawn.mindState.duty.def == DutyDefOf.Steal || pawn.mindState.duty.def == DutyDefOf.Kidnap);
		}
	}
}
