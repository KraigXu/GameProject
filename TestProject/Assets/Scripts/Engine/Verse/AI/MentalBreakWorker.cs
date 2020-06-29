﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	
	public class MentalBreakWorker
	{
		
		public virtual float CommonalityFor(Pawn pawn, bool moodCaused = false)
		{
			float num = this.def.baseCommonality;
			if (pawn.Faction == Faction.OfPlayer && this.def.commonalityFactorPerPopulationCurve != null)
			{
				num *= this.def.commonalityFactorPerPopulationCurve.Evaluate((float)PawnsFinder.AllMaps_FreeColonists.Count<Pawn>());
			}
			return num;
		}

		
		public virtual bool BreakCanOccur(Pawn pawn)
		{
			if (this.def.requiredTrait != null && (pawn.story == null || !pawn.story.traits.HasTrait(this.def.requiredTrait)))
			{
				return false;
			}
			if (this.def.mentalState != null && pawn.story != null && pawn.story.traits.allTraits.Any((Trait tr) => tr.CurrentData.disallowedMentalStates != null && tr.CurrentData.disallowedMentalStates.Contains(this.def.mentalState)))
			{
				return false;
			}
			if (this.def.mentalState != null && !this.def.mentalState.Worker.StateCanOccur(pawn))
			{
				return false;
			}
			if (pawn.story != null)
			{
				IEnumerable<MentalBreakDef> theOnlyAllowedMentalBreaks = pawn.story.traits.TheOnlyAllowedMentalBreaks;
				if (!theOnlyAllowedMentalBreaks.Contains(this.def) && theOnlyAllowedMentalBreaks.Any((MentalBreakDef x) => x.intensity == this.def.intensity && x.Worker.BreakCanOccur(pawn)))
				{
					return false;
				}
			}
			return !TutorSystem.TutorialMode || pawn.Faction != Faction.OfPlayer;
		}

		
		public virtual bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			return pawn.mindState.mentalStateHandler.TryStartMentalState(this.def.mentalState, reason, false, causedByMood, null, false);
		}

		
		protected bool TrySendLetter(Pawn pawn, string textKey, string reason)
		{
			if (!PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				return false;
			}
			TaggedString label = this.def.LabelCap + ": " + pawn.LabelShortCap;
			TaggedString taggedString = textKey.Translate(pawn.Label, pawn.Named("PAWN")).CapitalizeFirst();
			if (reason != null)
			{
				taggedString += "\n\n" + reason;
			}
			taggedString = taggedString.AdjustedFor(pawn, "PAWN", true);
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NegativeEvent, pawn, null, null, null, null);
			return true;
		}

		
		public MentalBreakDef def;
	}
}
