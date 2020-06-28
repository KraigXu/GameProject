using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D8 RID: 2008
	public class ThinkNode_ConditionalAnimalWrongSeason : ThinkNode_Conditional
	{
		// Token: 0x060033A3 RID: 13219 RVA: 0x0011DB89 File Offset: 0x0011BD89
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.Animal && !pawn.Map.mapTemperature.SeasonAcceptableFor(pawn.def);
		}
	}
}
