using System;
using Verse;

namespace RimWorld
{
	
	public class SignalAction_Letter : SignalAction
	{
		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Letter>(ref this.letter, "letter", Array.Empty<object>());
		}

		
		protected override void DoAction(SignalArgs args)
		{
			Pawn pawn;
			if (args.TryGetArg<Pawn>("SUBJECT", out pawn))
			{
				ChoiceLetter choiceLetter = this.letter as ChoiceLetter;
				if (choiceLetter != null)
				{
					choiceLetter.text = choiceLetter.text.Resolve().Formatted(pawn.LabelShort, pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
				}
				if (!this.letter.lookTargets.IsValid())
				{
					this.letter.lookTargets = pawn;
				}
			}
			Find.LetterStack.ReceiveLetter(this.letter, null);
		}

		
		public Letter letter;
	}
}
