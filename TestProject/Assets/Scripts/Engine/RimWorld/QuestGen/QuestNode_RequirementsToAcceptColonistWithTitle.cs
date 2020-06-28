using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001187 RID: 4487
	public class QuestNode_RequirementsToAcceptColonistWithTitle : QuestNode
	{
		// Token: 0x06006818 RID: 26648 RVA: 0x00246410 File Offset: 0x00244610
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.AddPart(new QuestPart_RequirementsToAcceptColonistWithTitle
			{
				minimumTitle = this.minimumTitle.GetValue(slate),
				faction = this.faction.GetValue(slate)
			});
		}

		// Token: 0x06006819 RID: 26649 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04004068 RID: 16488
		public SlateRef<RoyalTitleDef> minimumTitle;

		// Token: 0x04004069 RID: 16489
		public SlateRef<Faction> faction;
	}
}
