using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001112 RID: 4370
	public class QuestNode_Log : QuestNode
	{
		// Token: 0x06006667 RID: 26215 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006668 RID: 26216 RVA: 0x0023DDE7 File Offset: 0x0023BFE7
		protected override void RunInt()
		{
			Log.Message("QuestNode_Log: " + this.message.ToString(QuestGen.slate), false);
		}

		// Token: 0x04003E80 RID: 16000
		[NoTranslate]
		public SlateRef<object> message;
	}
}
