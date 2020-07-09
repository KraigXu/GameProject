using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_AddMemoryThought : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				foreach (Pawn pawn in this.pawns.GetValue(slate))
				{
					QuestPart_AddMemoryThought questPart_AddMemoryThought = new QuestPart_AddMemoryThought();
					questPart_AddMemoryThought.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
					questPart_AddMemoryThought.def = this.def.GetValue(slate);
					questPart_AddMemoryThought.pawn = pawn;
					questPart_AddMemoryThought.otherPawn = this.otherPawn.GetValue(slate);
					questPart_AddMemoryThought.addToLookTargets = (this.addToLookTargets.GetValue(slate) ?? true);
					QuestGen.quest.AddPart(questPart_AddMemoryThought);
				}
			}
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<ThoughtDef> def;

		
		public SlateRef<IEnumerable<Pawn>> pawns;

		
		public SlateRef<Pawn> otherPawn;

		
		public SlateRef<bool?> addToLookTargets;
	}
}
