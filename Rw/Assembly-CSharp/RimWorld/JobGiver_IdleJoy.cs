using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F7 RID: 1783
	public class JobGiver_IdleJoy : JobGiver_GetJoy
	{
		// Token: 0x06002F37 RID: 12087 RVA: 0x0010986C File Offset: 0x00107A6C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.needs.joy == null)
			{
				return null;
			}
			if (Find.TickManager.TicksGame < 60000)
			{
				return null;
			}
			if (JoyUtility.LordPreventsGettingJoy(pawn) || JoyUtility.TimetablePreventsGettingJoy(pawn))
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x04001AAC RID: 6828
		private const int GameStartNoIdleJoyTicks = 60000;
	}
}
