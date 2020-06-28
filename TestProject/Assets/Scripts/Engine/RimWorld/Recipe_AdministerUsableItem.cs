using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF3 RID: 2803
	public class Recipe_AdministerUsableItem : Recipe_Surgery
	{
		// Token: 0x0600423D RID: 16957 RVA: 0x00161D4D File Offset: 0x0015FF4D
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			ingredients[0].TryGetComp<CompUsable>().UsedBy(pawn);
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x00002681 File Offset: 0x00000881
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}
	}
}
