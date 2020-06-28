using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020009ED RID: 2541
	public class IncidentWorker_PsychicSoothe : IncidentWorker_PsychicEmanation
	{
		// Token: 0x06003C79 RID: 15481 RVA: 0x0013F89C File Offset: 0x0013DA9C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (base.TryExecuteWorker(parms))
			{
				SoundDefOf.PsychicSootheGlobal.PlayOneShotOnCamera((Map)parms.target);
				return true;
			}
			return false;
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x0013F8C0 File Offset: 0x0013DAC0
		protected override void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points)
		{
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicSoothe, duration);
			gameCondition_PsychicEmanation.gender = gender;
			map.gameConditionManager.RegisterCondition(gameCondition_PsychicEmanation);
			base.SendStandardLetter(gameCondition_PsychicEmanation.LabelCap, gameCondition_PsychicEmanation.LetterText, gameCondition_PsychicEmanation.def.letterDef, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
		}
	}
}
