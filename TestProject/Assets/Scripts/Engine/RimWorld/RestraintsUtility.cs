using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B81 RID: 2945
	public static class RestraintsUtility
	{
		// Token: 0x0600450F RID: 17679 RVA: 0x0017577C File Offset: 0x0017397C
		public static bool InRestraints(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return false;
			}
			if (pawn.HostFaction == null)
			{
				return false;
			}
			Lord lord = pawn.GetLord();
			return (lord == null || lord.LordJob == null || !lord.LordJob.NeverInRestraints) && (pawn.guest == null || !pawn.guest.Released);
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x001757D6 File Offset: 0x001739D6
		public static bool ShouldShowRestraintsInfo(Pawn pawn)
		{
			return pawn.IsPrisonerOfColony && RestraintsUtility.InRestraints(pawn);
		}
	}
}
