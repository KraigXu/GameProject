using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020011B6 RID: 4534
	public class QuestPart_Filter_DecreeNotPossible : QuestPart_Filter
	{
		// Token: 0x060068CD RID: 26829 RVA: 0x00249A14 File Offset: 0x00247C14
		protected override bool Pass(SignalArgs args)
		{
			Pawn pawn;
			return args.TryGetArg<Pawn>("SUBJECT", out pawn) && (pawn.royalty == null || !pawn.royalty.PossibleDecreeQuests.Contains(this.quest.root));
		}
	}
}
