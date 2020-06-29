using System;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_RequirementsToAcceptColonistWithTitle : QuestNode
	{
		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.AddPart(new QuestPart_RequirementsToAcceptColonistWithTitle
			{
				minimumTitle = this.minimumTitle.GetValue(slate),
				faction = this.faction.GetValue(slate)
			});
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		public SlateRef<RoyalTitleDef> minimumTitle;

		
		public SlateRef<Faction> faction;
	}
}
