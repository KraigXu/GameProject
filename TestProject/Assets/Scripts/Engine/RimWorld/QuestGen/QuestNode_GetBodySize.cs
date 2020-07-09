using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetBodySize : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.pawnKind.GetValue(slate).RaceProps.baseBodySize, false);
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<float>(this.storeAs.GetValue(slate), this.pawnKind.GetValue(slate).RaceProps.baseBodySize, false);
		}

		
		public SlateRef<PawnKindDef> pawnKind;

		
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
