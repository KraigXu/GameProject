using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001138 RID: 4408
	public class QuestNode_GetPopIntentForQuest : QuestNode
	{
		// Token: 0x06006700 RID: 26368 RVA: 0x002413E1 File Offset: 0x0023F5E1
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06006701 RID: 26369 RVA: 0x002413EB File Offset: 0x0023F5EB
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06006702 RID: 26370 RVA: 0x002413F8 File Offset: 0x0023F5F8
		private void SetVars(Slate slate)
		{
			float populationIntentForQuest = StorytellerUtilityPopulation.PopulationIntentForQuest;
			slate.Set<float>(this.storeAs.GetValue(slate), populationIntentForQuest, false);
		}

		// Token: 0x04003F2C RID: 16172
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
