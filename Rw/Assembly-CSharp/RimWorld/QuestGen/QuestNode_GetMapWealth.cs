using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200112C RID: 4396
	public class QuestNode_GetMapWealth : QuestNode
	{
		// Token: 0x060066C9 RID: 26313 RVA: 0x0023F770 File Offset: 0x0023D970
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.map.GetValue(slate).wealthWatcher.WealthTotal, false);
			return true;
		}

		// Token: 0x060066CA RID: 26314 RVA: 0x0023F79C File Offset: 0x0023D99C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<float>(this.storeAs.GetValue(slate), this.map.GetValue(slate).wealthWatcher.WealthTotal, false);
		}

		// Token: 0x04003EE9 RID: 16105
		public SlateRef<Map> map;

		// Token: 0x04003EEA RID: 16106
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
