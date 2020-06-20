using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001157 RID: 4439
	public class QuestNode_IsHumanlike : QuestNode_RaceProperty
	{
		// Token: 0x0600677A RID: 26490 RVA: 0x00243340 File Offset: 0x00241540
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.Humanlike;
		}
	}
}
