using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class IncidentWorker_PsychicDrone : IncidentWorker_PsychicEmanation
	{
		
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (base.TryExecuteWorker(parms))
			{
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera((Map)parms.target);
				return true;
			}
			return false;
		}

		
		protected override void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points)
		{
			if (points < 0f)
			{
				points = StorytellerUtility.DefaultThreatPointsNow(map);
			}
			PsychicDroneLevel level;
			if (points < 800f)
			{
				level = PsychicDroneLevel.BadLow;
			}
			else if (points < 2000f)
			{
				level = PsychicDroneLevel.BadMedium;
			}
			else
			{
				level = PsychicDroneLevel.BadHigh;
			}
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicDrone, duration);
			gameCondition_PsychicEmanation.gender = gender;
			gameCondition_PsychicEmanation.level = level;
			map.gameConditionManager.RegisterCondition(gameCondition_PsychicEmanation);
			base.SendStandardLetter(gameCondition_PsychicEmanation.LabelCap, gameCondition_PsychicEmanation.LetterText, gameCondition_PsychicEmanation.def.letterDef, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
		}

		
		private const float MaxPointsDroneLow = 800f;

		
		private const float MaxPointsDroneMedium = 2000f;
	}
}
