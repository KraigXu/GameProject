using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200112D RID: 4397
	public class QuestNode_GetMarketValue : QuestNode
	{
		// Token: 0x060066CC RID: 26316 RVA: 0x0023F7D8 File Offset: 0x0023D9D8
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x060066CD RID: 26317 RVA: 0x0023F7E1 File Offset: 0x0023D9E1
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060066CE RID: 26318 RVA: 0x0023F7F0 File Offset: 0x0023D9F0
		private bool DoWork(Slate slate)
		{
			float num = 0f;
			if (this.things.GetValue(slate) != null)
			{
				foreach (Thing thing in this.things.GetValue(slate))
				{
					num += thing.MarketValue * (float)thing.stackCount;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
			return true;
		}

		// Token: 0x04003EEB RID: 16107
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003EEC RID: 16108
		public SlateRef<IEnumerable<Thing>> things;
	}
}
