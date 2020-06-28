using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF6 RID: 3830
	public class PawnTable_Wildlife : PawnTable
	{
		// Token: 0x06005DFD RID: 24061 RVA: 0x0020763C File Offset: 0x0020583C
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.baseBodySize descending, p.def.label
			select p;
		}

		// Token: 0x06005DFE RID: 24062 RVA: 0x0020761A File Offset: 0x0020581A
		public PawnTable_Wildlife(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
