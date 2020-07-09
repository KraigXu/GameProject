using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetMapWealth : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.map.GetValue(slate).wealthWatcher.WealthTotal, false);
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<float>(this.storeAs.GetValue(slate), this.map.GetValue(slate).wealthWatcher.WealthTotal, false);
		}

		
		public SlateRef<Map> map;

		
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
