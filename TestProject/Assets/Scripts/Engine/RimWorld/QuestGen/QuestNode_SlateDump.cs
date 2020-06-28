using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001114 RID: 4372
	public class QuestNode_SlateDump : QuestNode
	{
		// Token: 0x0600666D RID: 26221 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600666E RID: 26222 RVA: 0x0023DE6E File Offset: 0x0023C06E
		protected override void RunInt()
		{
			Log.Message(QuestGen.slate.ToString(), false);
		}
	}
}
