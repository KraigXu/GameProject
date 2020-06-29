using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_RemoveMemoryThought : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_RemoveMemoryThought questPart_RemoveMemoryThought = new QuestPart_RemoveMemoryThought();
			questPart_RemoveMemoryThought.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_RemoveMemoryThought.def = this.def.GetValue(slate);
			questPart_RemoveMemoryThought.pawn = this.pawn.GetValue(slate);
			questPart_RemoveMemoryThought.count = this.count.GetValue(slate);
			questPart_RemoveMemoryThought.otherPawn = this.otherPawn.GetValue(slate);
			questPart_RemoveMemoryThought.addToLookTargets = (this.addToLookTargets.GetValue(slate) ?? true);
			QuestGen.quest.AddPart(questPart_RemoveMemoryThought);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<ThoughtDef> def;

		
		public SlateRef<Pawn> pawn;

		
		public SlateRef<Pawn> otherPawn;

		
		public SlateRef<int?> count;

		
		public SlateRef<bool?> addToLookTargets;
	}
}
