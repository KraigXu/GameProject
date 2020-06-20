using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001155 RID: 4437
	public class QuestNode_IsAnimal : QuestNode_RaceProperty
	{
		// Token: 0x06006776 RID: 26486 RVA: 0x00243328 File Offset: 0x00241528
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.Animal;
		}
	}
}
