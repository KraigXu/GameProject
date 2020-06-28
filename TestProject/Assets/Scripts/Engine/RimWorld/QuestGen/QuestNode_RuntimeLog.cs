using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001113 RID: 4371
	public class QuestNode_RuntimeLog : QuestNode
	{
		// Token: 0x0600666A RID: 26218 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600666B RID: 26219 RVA: 0x0023DE0C File Offset: 0x0023C00C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Log questPart_Log = new QuestPart_Log();
			questPart_Log.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Log.message = this.message.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Log);
		}

		// Token: 0x04003E81 RID: 16001
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003E82 RID: 16002
		[NoTranslate]
		public SlateRef<string> message;
	}
}
