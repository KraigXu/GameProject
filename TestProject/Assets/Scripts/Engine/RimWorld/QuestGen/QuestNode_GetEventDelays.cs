using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001123 RID: 4387
	public class QuestNode_GetEventDelays : QuestNode
	{
		// Token: 0x060066A1 RID: 26273 RVA: 0x0023ED93 File Offset: 0x0023CF93
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060066A2 RID: 26274 RVA: 0x0023ED9D File Offset: 0x0023CF9D
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060066A3 RID: 26275 RVA: 0x0023EDAC File Offset: 0x0023CFAC
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

		// Token: 0x04003EC4 RID: 16068
		public SlateRef<int> durationTicks;

		// Token: 0x04003EC5 RID: 16069
		public SlateRef<IntRange> intervalTicksRange;

		// Token: 0x04003EC6 RID: 16070
		[NoTranslate]
		public SlateRef<string> storeCountAs;

		// Token: 0x04003EC7 RID: 16071
		[NoTranslate]
		public SlateRef<string> storeDelaysAs;
	}
}
