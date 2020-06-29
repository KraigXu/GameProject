using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	
	public class MentalBreakWorker_WildDecree : MentalBreakWorker
	{
		
		public override bool BreakCanOccur(Pawn pawn)
		{
			return base.BreakCanOccur(pawn) && pawn.IsColonist && !pawn.IsPrisoner && pawn.royalty != null && pawn.royalty.PossibleDecreeQuests.Any<QuestScriptDef>();
		}

		
		public override float CommonalityFor(Pawn pawn, bool moodCaused = false)
		{
			if (pawn.royalty == null)
			{
				return 0f;
			}
			float num = 0f;
			List<RoyalTitle> allTitlesInEffectForReading = pawn.royalty.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				num = Mathf.Max(num, allTitlesInEffectForReading[i].def.decreeMentalBreakCommonality);
			}
			return num;
		}

		
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			pawn.royalty.IssueDecree(true, reason);
			if (MentalStateDefOf.Wander_OwnRoom.Worker.StateCanOccur(pawn))
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_OwnRoom, null, false, causedByMood, null, true);
			}
			else if (MentalStateDefOf.Wander_Sad.Worker.StateCanOccur(pawn))
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, causedByMood, null, true);
			}
			return true;
		}
	}
}
