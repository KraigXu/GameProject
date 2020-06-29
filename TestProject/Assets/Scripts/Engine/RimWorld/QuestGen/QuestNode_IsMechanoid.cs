using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_IsMechanoid : QuestNode_RaceProperty
	{
		
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.IsMechanoid;
		}
	}
}
