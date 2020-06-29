using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetRandomElement : QuestNode
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
			SlateRef<object> slateRef;
			if (this.options.TryRandomElement(out slateRef))
			{
				slate.Set<object>(this.storeAs.GetValue(slate), slateRef.GetValue(slate), false);
			}
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public List<SlateRef<object>> options;
	}
}
