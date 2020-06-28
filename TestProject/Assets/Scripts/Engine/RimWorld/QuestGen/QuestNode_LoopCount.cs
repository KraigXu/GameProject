using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200117A RID: 4474
	public class QuestNode_LoopCount : QuestNode
	{
		// Token: 0x060067EF RID: 26607 RVA: 0x002457A0 File Offset: 0x002439A0
		protected override bool TestRunInt(Slate slate)
		{
			for (int i = 0; i < this.loopCount.GetValue(slate); i++)
			{
				if (this.storeLoopCounterAs.GetValue(slate) != null)
				{
					slate.Set<int>(this.storeLoopCounterAs.GetValue(slate), i, false);
				}
				try
				{
					if (!this.node.TestRun(slate))
					{
						return false;
					}
				}
				finally
				{
					slate.PopPrefix();
				}
			}
			return true;
		}

		// Token: 0x060067F0 RID: 26608 RVA: 0x00245818 File Offset: 0x00243A18
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			for (int i = 0; i < this.loopCount.GetValue(slate); i++)
			{
				if (this.storeLoopCounterAs.GetValue(slate) != null)
				{
					QuestGen.slate.Set<int>(this.storeLoopCounterAs.GetValue(slate), i, false);
				}
				try
				{
					this.node.Run();
				}
				finally
				{
					QuestGen.slate.PopPrefix();
				}
			}
		}

		// Token: 0x0400402D RID: 16429
		public QuestNode node;

		// Token: 0x0400402E RID: 16430
		public SlateRef<int> loopCount;

		// Token: 0x0400402F RID: 16431
		[NoTranslate]
		public SlateRef<string> storeLoopCounterAs;
	}
}
