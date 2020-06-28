using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000828 RID: 2088
	public class ThoughtWorker_Expectations : ThoughtWorker
	{
		// Token: 0x0600345D RID: 13405 RVA: 0x0011FA28 File Offset: 0x0011DC28
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p);
			if (expectationDef == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(expectationDef.thoughtStage);
		}
	}
}
