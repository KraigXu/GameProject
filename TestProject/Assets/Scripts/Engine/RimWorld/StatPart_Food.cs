using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFE RID: 4094
	public class StatPart_Food : StatPart
	{
		// Token: 0x06006213 RID: 25107 RVA: 0x0022081C File Offset: 0x0021EA1C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.food != null)
				{
					val *= this.FoodMultiplier(pawn.needs.food.CurCategory);
				}
			}
		}

		// Token: 0x06006214 RID: 25108 RVA: 0x0022086C File Offset: 0x0021EA6C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.food != null)
				{
					return pawn.needs.food.CurCategory.GetLabel() + ": x" + this.FoodMultiplier(pawn.needs.food.CurCategory).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x06006215 RID: 25109 RVA: 0x002208DB File Offset: 0x0021EADB
		private float FoodMultiplier(HungerCategory hunger)
		{
			switch (hunger)
			{
			case HungerCategory.Fed:
				return this.factorFed;
			case HungerCategory.Hungry:
				return this.factorHungry;
			case HungerCategory.UrgentlyHungry:
				return this.factorUrgentlyHungry;
			case HungerCategory.Starving:
				return this.factorStarving;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x04003BDB RID: 15323
		public float factorStarving = 1f;

		// Token: 0x04003BDC RID: 15324
		public float factorUrgentlyHungry = 1f;

		// Token: 0x04003BDD RID: 15325
		public float factorHungry = 1f;

		// Token: 0x04003BDE RID: 15326
		public float factorFed = 1f;
	}
}
