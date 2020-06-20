using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F6 RID: 1782
	public class JobGiver_GetJoyInBed : JobGiver_GetJoy
	{
		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002F32 RID: 12082 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool CanDoDuringMedicalRest
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x00109802 File Offset: 0x00107A02
		protected override bool JoyGiverAllowed(JoyGiverDef def)
		{
			return def.canDoWhileInBed;
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x0010980A File Offset: 0x00107A0A
		protected override Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			return def.Worker.TryGiveJobWhileInBed(pawn);
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x00109818 File Offset: 0x00107A18
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.CurJob == null || !pawn.InBed() || !pawn.Awake() || pawn.needs.joy == null)
			{
				return null;
			}
			if (pawn.needs.joy.CurLevel > 0.5f)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x04001AAB RID: 6827
		private const float MaxJoyLevel = 0.5f;
	}
}
