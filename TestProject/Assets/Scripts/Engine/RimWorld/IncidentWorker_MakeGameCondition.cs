using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D2 RID: 2514
	public class IncidentWorker_MakeGameCondition : IncidentWorker
	{
		// Token: 0x06003C0F RID: 15375 RVA: 0x0013D00C File Offset: 0x0013B20C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			GameConditionManager gameConditionManager = parms.target.GameConditionManager;
			if (gameConditionManager == null)
			{
				Log.ErrorOnce(string.Format("Couldn't find condition manager for incident target {0}", parms.target), 70849667, false);
				return false;
			}
			if (gameConditionManager.ConditionIsActive(this.def.gameCondition))
			{
				return false;
			}
			List<GameCondition> activeConditions = gameConditionManager.ActiveConditions;
			for (int i = 0; i < activeConditions.Count; i++)
			{
				if (!this.def.gameCondition.CanCoexistWith(activeConditions[i].def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x0013D094 File Offset: 0x0013B294
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			GameConditionManager gameConditionManager = parms.target.GameConditionManager;
			int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
			GameCondition gameCondition = GameConditionMaker.MakeCondition(this.def.gameCondition, duration);
			gameConditionManager.RegisterCondition(gameCondition);
			parms.letterHyperlinkThingDefs = gameCondition.def.letterHyperlinks;
			base.SendStandardLetter(this.def.letterLabel, gameCondition.LetterText, this.def.letterDef, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
