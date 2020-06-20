using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007FC RID: 2044
	public class ThoughtWorker_NeedRest : ThoughtWorker
	{
		// Token: 0x060033FF RID: 13311 RVA: 0x0011E58C File Offset: 0x0011C78C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.rest == null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.rest.CurCategory)
			{
			case RestCategory.Rested:
				return ThoughtState.Inactive;
			case RestCategory.Tired:
				return ThoughtState.ActiveAtStage(0);
			case RestCategory.VeryTired:
				return ThoughtState.ActiveAtStage(1);
			case RestCategory.Exhausted:
				return ThoughtState.ActiveAtStage(2);
			default:
				throw new NotImplementedException();
			}
		}
	}
}
