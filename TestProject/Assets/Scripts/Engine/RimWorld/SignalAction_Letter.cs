using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C98 RID: 3224
	public class SignalAction_Letter : SignalAction
	{
		// Token: 0x06004DBF RID: 19903 RVA: 0x001A20C2 File Offset: 0x001A02C2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Letter>(ref this.letter, "letter", Array.Empty<object>());
		}

		// Token: 0x06004DC0 RID: 19904 RVA: 0x001A20E0 File Offset: 0x001A02E0
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

		// Token: 0x04002B8A RID: 11146
		public Letter letter;
	}
}
