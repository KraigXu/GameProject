using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001174 RID: 4468
	public class QuestNode_InspectString : QuestNode
	{
		// Token: 0x060067DD RID: 26589 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067DE RID: 26590 RVA: 0x00245258 File Offset: 0x00243458
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.targets.GetValue(slate).EnumerableNullOrEmpty<ISelectable>())
			{
				return;
			}
			QuestPart_InspectString questPart_InspectString = new QuestPart_InspectString();
			questPart_InspectString.targets.AddRange(this.targets.GetValue(slate));
			questPart_InspectString.inspectString = this.inspectString.GetValue(slate);
			questPart_InspectString.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_InspectString);
		}

		// Token: 0x0400400D RID: 16397
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400400E RID: 16398
		public SlateRef<IEnumerable<ISelectable>> targets;

		// Token: 0x0400400F RID: 16399
		public SlateRef<string> inspectString;
	}
}
