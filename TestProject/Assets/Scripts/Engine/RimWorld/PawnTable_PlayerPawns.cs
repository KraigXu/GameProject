using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF4 RID: 3828
	public class PawnTable_PlayerPawns : PawnTable
	{
		// Token: 0x06005DFA RID: 24058 RVA: 0x00207627 File Offset: 0x00205827
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return PlayerPawnsDisplayOrderUtility.InOrder(input);
		}

		// Token: 0x06005DFB RID: 24059 RVA: 0x0020761A File Offset: 0x0020581A
		public PawnTable_PlayerPawns(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
