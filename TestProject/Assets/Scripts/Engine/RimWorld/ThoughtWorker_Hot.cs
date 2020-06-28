using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000827 RID: 2087
	public class ThoughtWorker_Hot : ThoughtWorker
	{
		// Token: 0x0600345B RID: 13403 RVA: 0x0011F9C4 File Offset: 0x0011DBC4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float statValue = p.GetStatValue(StatDefOf.ComfyTemperatureMax, true);
			float num = p.AmbientTemperature - statValue;
			if (num <= 0f)
			{
				return ThoughtState.Inactive;
			}
			if (num < 10f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (num < 20f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (num < 30f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}
	}
}
