using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_ChangeNeed : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeNeed questPart_ChangeNeed = new QuestPart_ChangeNeed();
			questPart_ChangeNeed.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ChangeNeed.pawn = this.pawn.GetValue(slate);
			questPart_ChangeNeed.need = this.need.GetValue(slate);
			questPart_ChangeNeed.offset = this.offset.GetValue(slate);
			QuestGen.quest.AddPart(questPart_ChangeNeed);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<Pawn> pawn;

		
		public SlateRef<NeedDef> need;

		
		public SlateRef<float> offset;
	}
}
