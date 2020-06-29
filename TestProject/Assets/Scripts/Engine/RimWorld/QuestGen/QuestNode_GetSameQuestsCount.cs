using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetSameQuestsCount : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		
		private void SetVars(Slate slate)
		{
			int var = Find.QuestManager.QuestsListForReading.Count((Quest x) => x.root == QuestGen.Root);
			slate.Set<int>("sameQuestsCount", var, false);
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
