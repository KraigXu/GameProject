using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF5 RID: 3829
	public class PawnTable_PlayerPawnsTimetable : PawnTable_PlayerPawns
	{
		// Token: 0x06005DFC RID: 24060 RVA: 0x0020762F File Offset: 0x0020582F
		public PawnTable_PlayerPawnsTimetable(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
