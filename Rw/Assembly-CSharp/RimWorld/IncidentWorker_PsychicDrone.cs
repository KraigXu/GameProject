﻿using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020009EC RID: 2540
	public class IncidentWorker_PsychicDrone : IncidentWorker_PsychicEmanation
	{
		// Token: 0x06003C76 RID: 15478 RVA: 0x0013F7D8 File Offset: 0x0013D9D8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (base.TryExecuteWorker(parms))
			{
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera((Map)parms.target);
				return true;
			}
			return false;
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x0013F7FC File Offset: 0x0013D9FC
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

		// Token: 0x0400238E RID: 9102
		private const float MaxPointsDroneLow = 800f;

		// Token: 0x0400238F RID: 9103
		private const float MaxPointsDroneMedium = 2000f;
	}
}
