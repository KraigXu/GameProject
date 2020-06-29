using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetMonumentSize : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		
		private void DoWork(Slate slate)
		{
			if (this.monumentMarker.GetValue(slate) == null)
			{
				return;
			}
			slate.Set<IntVec2>(this.storeAs.GetValue(slate), this.monumentMarker.GetValue(slate).Size, false);
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
