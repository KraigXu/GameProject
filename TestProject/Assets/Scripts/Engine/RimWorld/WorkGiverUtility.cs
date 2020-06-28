using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000767 RID: 1895
	public static class WorkGiverUtility
	{
		// Token: 0x06003185 RID: 12677 RVA: 0x00113A50 File Offset: 0x00111C50
		public static Job HaulStuffOffBillGiverJob(Pawn pawn, IBillGiver giver, Thing thingToIgnore)
		{
			foreach (IntVec3 c in giver.IngredientStackCells)
			{
				Thing thing = pawn.Map.thingGrid.ThingAt(c, ThingCategory.Item);
				if (thing != null && thing != thingToIgnore)
				{
					return HaulAIUtility.HaulAsideJobFor(pawn, thing);
				}
			}
			return null;
		}
	}
}
