using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_AddToList : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			QuestGenUtility.AddToOrMakeList(slate, this.name.GetValue(slate), this.value.GetValue(slate));
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.name.GetValue(slate), this.value.GetValue(slate));
		}

		
		[NoTranslate]
		public SlateRef<string> name;

		
		public SlateRef<object> value;
	}
}
