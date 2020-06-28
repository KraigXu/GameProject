﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B4C RID: 2892
	public class InteractionWorker_SparkJailbreak : InteractionWorker
	{
		// Token: 0x060043E8 RID: 17384 RVA: 0x0016F2BC File Offset: 0x0016D4BC
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			if (!recipient.IsPrisoner || !recipient.guest.PrisonerIsSecure || !PrisonBreakUtility.CanParticipateInPrisonBreak(recipient))
			{
				letterText = null;
				letterLabel = null;
				letterDef = null;
				lookTargets = null;
				return;
			}
			PrisonBreakUtility.StartPrisonBreak(recipient, out letterText, out letterLabel, out letterDef);
			lookTargets = new LookTargets(new TargetInfo[]
			{
				initiator,
				recipient
			});
			MentalState_Jailbreaker mentalState_Jailbreaker = initiator.MentalState as MentalState_Jailbreaker;
			if (mentalState_Jailbreaker != null)
			{
				mentalState_Jailbreaker.Notify_InducedPrisonerToEscape();
			}
		}
	}
}
