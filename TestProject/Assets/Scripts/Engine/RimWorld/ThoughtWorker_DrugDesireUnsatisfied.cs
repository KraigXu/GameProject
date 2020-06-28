using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000839 RID: 2105
	public class ThoughtWorker_DrugDesireUnsatisfied : ThoughtWorker
	{
		// Token: 0x06003481 RID: 13441 RVA: 0x0012008C File Offset: 0x0011E28C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Need_Chemical_Any drugsDesire = p.needs.drugsDesire;
			if (drugsDesire == null)
			{
				return false;
			}
			int moodBuffForCurrentLevel = (int)drugsDesire.MoodBuffForCurrentLevel;
			if (moodBuffForCurrentLevel < 3)
			{
				return ThoughtState.ActiveAtStage(3 - moodBuffForCurrentLevel - 1);
			}
			return false;
		}

		// Token: 0x04001BB8 RID: 7096
		private const int Neutral = 3;
	}
}
