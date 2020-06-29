using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class PawnTable_Animals : PawnTable
	{
		
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

		
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.petness descending, p.RaceProps.baseBodySize
			select p;
		}

		
		public PawnTable_Animals(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
