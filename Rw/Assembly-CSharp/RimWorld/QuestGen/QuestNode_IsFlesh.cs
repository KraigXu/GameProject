using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001156 RID: 4438
	public class QuestNode_IsFlesh : QuestNode_RaceProperty
	{
		// Token: 0x06006778 RID: 26488 RVA: 0x00243338 File Offset: 0x00241538
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.IsFlesh;
		}
	}
}
