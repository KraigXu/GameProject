using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetEventDelays : QuestNode
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
			if (this.intervalTicksRange.GetValue(slate).max <= 0)
			{
				Log.Error("intervalTicksRange with max <= 0", false);
				return;
			}
			int num = 0;
			int num2 = 0;
			for (;;)
			{
				num += this.intervalTicksRange.GetValue(slate).RandomInRange;
				if (num > this.durationTicks.GetValue(slate))
				{
					break;
				}
				slate.Set<int>(this.storeDelaysAs.GetValue(slate).Formatted(num2.Named("INDEX")), num, false);
				num2++;
			}
			slate.Set<int>(this.storeCountAs.GetValue(slate), num2, false);
		}

		
		public SlateRef<int> durationTicks;

		
		public SlateRef<IntRange> intervalTicksRange;

		
		[NoTranslate]
		public SlateRef<string> storeCountAs;

		
		[NoTranslate]
		public SlateRef<string> storeDelaysAs;
	}
}
