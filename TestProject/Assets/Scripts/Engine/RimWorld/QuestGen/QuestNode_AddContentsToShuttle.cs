using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200115A RID: 4442
	public class QuestNode_AddContentsToShuttle : QuestNode
	{
		// Token: 0x06006786 RID: 26502 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006787 RID: 26503 RVA: 0x00243578 File Offset: 0x00241778
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.contents.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_AddContentsToShuttle questPart_AddContentsToShuttle = new QuestPart_AddContentsToShuttle();
			questPart_AddContentsToShuttle.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_AddContentsToShuttle.shuttle = this.shuttle.GetValue(slate);
			questPart_AddContentsToShuttle.Things = this.contents.GetValue(slate);
			QuestGen.quest.AddPart(questPart_AddContentsToShuttle);
		}

		// Token: 0x04003FA1 RID: 16289
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FA2 RID: 16290
		public SlateRef<Thing> shuttle;

		// Token: 0x04003FA3 RID: 16291
		public SlateRef<IEnumerable<Thing>> contents;
	}
}
