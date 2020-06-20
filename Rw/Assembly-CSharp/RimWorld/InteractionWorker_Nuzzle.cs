using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B48 RID: 2888
	public class InteractionWorker_Nuzzle : InteractionWorker
	{
		// Token: 0x060043D6 RID: 17366 RVA: 0x0016DE4E File Offset: 0x0016C04E
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			this.AddNuzzledThought(initiator, recipient);
			this.TryGiveName(initiator, recipient);
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x0016DE70 File Offset: 0x0016C070
		private void AddNuzzledThought(Pawn initiator, Pawn recipient)
		{
			Thought_Memory newThought = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.Nuzzled);
			if (recipient.needs.mood != null)
			{
				recipient.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
			}
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x0016DEB6 File Offset: 0x0016C0B6
		private void TryGiveName(Pawn initiator, Pawn recipient)
		{
			if ((initiator.Name == null || initiator.Name.Numerical) && Rand.Value < initiator.RaceProps.nameOnNuzzleChance)
			{
				PawnUtility.GiveNameBecauseOfNuzzle(recipient, initiator);
			}
		}
	}
}
