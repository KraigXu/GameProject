using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_AddRangeToList : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			List<object> list = this.value.GetValue(slate);
			if (list != null)
			{
				QuestGenUtility.AddRangeToOrMakeList(slate, this.name.GetValue(slate), list);
			}
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			List<object> list = this.value.GetValue(slate);
			if (list != null)
			{
				QuestGenUtility.AddRangeToOrMakeList(slate, this.name.GetValue(slate), list);
			}
		}

		
		[NoTranslate]
		public SlateRef<string> name;

		
		public SlateRef<List<object>> value;
	}
}
