using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestPart_Filter_DecreeNotPossible : QuestPart_Filter
	{
		
		protected override bool Pass(SignalArgs args)
		{
			Pawn pawn;
			return args.TryGetArg<Pawn>("SUBJECT", out pawn) && (pawn.royalty == null || !pawn.royalty.PossibleDecreeQuests.Contains(this.quest.root));
		}
	}
}
