using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001189 RID: 4489
	public class QuestNode_SendShuttleAwayOnCleanup : QuestNode
	{
		// Token: 0x0600681E RID: 26654 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600681F RID: 26655 RVA: 0x002464DC File Offset: 0x002446DC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.shuttle.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SendShuttleAwayOnCleanup questPart_SendShuttleAwayOnCleanup = new QuestPart_SendShuttleAwayOnCleanup();
			questPart_SendShuttleAwayOnCleanup.shuttle = this.shuttle.GetValue(slate);
			questPart_SendShuttleAwayOnCleanup.dropEverything = this.dropEverything.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SendShuttleAwayOnCleanup);
		}

		// Token: 0x0400406D RID: 16493
		public SlateRef<Thing> shuttle;

		// Token: 0x0400406E RID: 16494
		public SlateRef<bool> dropEverything;
	}
}
