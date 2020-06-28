using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001158 RID: 4440
	public class QuestNode_IsMechanoid : QuestNode_RaceProperty
	{
		// Token: 0x0600677C RID: 26492 RVA: 0x00243348 File Offset: 0x00241548
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.IsMechanoid;
		}
	}
}
