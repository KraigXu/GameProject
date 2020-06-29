using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_Log : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Log.Message("QuestNode_Log: " + this.message.ToString(QuestGen.slate), false);
		}

		
		[NoTranslate]
		public SlateRef<object> message;
	}
}
