using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000846 RID: 2118
	public class ThoughtWorker_AlwaysActive : ThoughtWorker
	{
		// Token: 0x0600349C RID: 13468 RVA: 0x001206F4 File Offset: 0x0011E8F4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return true;
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x001206F4 File Offset: 0x0011E8F4
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			return true;
		}
	}
}
