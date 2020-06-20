using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001188 RID: 4488
	public class QuestNode_SendShuttleAway : QuestNode
	{
		// Token: 0x0600681B RID: 26651 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600681C RID: 26652 RVA: 0x00246458 File Offset: 0x00244658
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.shuttle.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SendShuttleAway questPart_SendShuttleAway = new QuestPart_SendShuttleAway();
			questPart_SendShuttleAway.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SendShuttleAway.shuttle = this.shuttle.GetValue(slate);
			questPart_SendShuttleAway.dropEverything = this.dropEverything.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SendShuttleAway);
		}

		// Token: 0x0400406A RID: 16490
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400406B RID: 16491
		public SlateRef<Thing> shuttle;

		// Token: 0x0400406C RID: 16492
		public SlateRef<bool> dropEverything;
	}
}
