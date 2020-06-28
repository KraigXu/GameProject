using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF3 RID: 3827
	public class PawnTable_Animals : PawnTable
	{
		// Token: 0x06005DF7 RID: 24055 RVA: 0x00207548 File Offset: 0x00205748
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return (from p in input
			orderby p.Name == null || p.Name.Numerical
			select p).ThenBy(delegate(Pawn p)
			{
				if (!(p.Name is NameSingle))
				{
					return 0;
				}
				return ((NameSingle)p.Name).Number;
			}).ThenBy((Pawn p) => p.def.label);
		}

		// Token: 0x06005DF8 RID: 24056 RVA: 0x002075C4 File Offset: 0x002057C4
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.petness descending, p.RaceProps.baseBodySize
			select p;
		}

		// Token: 0x06005DF9 RID: 24057 RVA: 0x0020761A File Offset: 0x0020581A
		public PawnTable_Animals(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
