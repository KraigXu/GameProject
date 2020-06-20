using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000838 RID: 2104
	public class ThoughtWorker_DrugDesireSatisfied : ThoughtWorker
	{
		// Token: 0x0600347F RID: 13439 RVA: 0x0012004C File Offset: 0x0011E24C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Need_Chemical_Any drugsDesire = p.needs.drugsDesire;
			if (drugsDesire == null)
			{
				return false;
			}
			int moodBuffForCurrentLevel = (int)drugsDesire.MoodBuffForCurrentLevel;
			if (moodBuffForCurrentLevel > 3)
			{
				return ThoughtState.ActiveAtStage(moodBuffForCurrentLevel - 3 - 1);
			}
			return false;
		}

		// Token: 0x04001BB7 RID: 7095
		private const int Neutral = 3;
	}
}
