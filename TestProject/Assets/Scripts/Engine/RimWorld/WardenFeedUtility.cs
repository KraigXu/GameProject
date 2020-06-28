using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000736 RID: 1846
	public static class WardenFeedUtility
	{
		// Token: 0x06003070 RID: 12400 RVA: 0x0010FC00 File Offset: 0x0010DE00
		public static bool ShouldBeFed(Pawn p)
		{
			return p.IsPrisonerOfColony && p.InBed() && p.guest.CanBeBroughtFood && HealthAIUtility.ShouldSeekMedicalRest(p);
		}
	}
}
