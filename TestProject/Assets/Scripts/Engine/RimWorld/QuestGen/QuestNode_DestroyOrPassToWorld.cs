using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001166 RID: 4454
	public class QuestNode_DestroyOrPassToWorld : QuestNode
	{
		// Token: 0x060067AA RID: 26538 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067AB RID: 26539 RVA: 0x00243C30 File Offset: 0x00241E30
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.things.GetValue(slate).EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_DestroyThingsOrPassToWorld questPart_DestroyThingsOrPassToWorld = new QuestPart_DestroyThingsOrPassToWorld();
			questPart_DestroyThingsOrPassToWorld.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_DestroyThingsOrPassToWorld.things.AddRange(this.things.GetValue(slate));
			QuestGen.quest.AddPart(questPart_DestroyThingsOrPassToWorld);
		}

		// Token: 0x04003FC8 RID: 16328
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FC9 RID: 16329
		public SlateRef<IEnumerable<Thing>> things;
	}
}
