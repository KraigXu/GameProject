using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_SetFactionRelations : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_SetFactionRelations questPart_SetFactionRelations = new QuestPart_SetFactionRelations();
			questPart_SetFactionRelations.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetFactionRelations.faction = this.faction.GetValue(slate);
			questPart_SetFactionRelations.relationKind = this.relationKind.GetValue(slate);
			questPart_SetFactionRelations.canSendLetter = (this.sendLetter.GetValue(slate) ?? true);
			QuestGen.quest.AddPart(questPart_SetFactionRelations);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<Faction> faction;

		
		public SlateRef<FactionRelationKind> relationKind;

		
		public SlateRef<bool?> sendLetter;

		
		private const string RootSymbol = "root";
	}
}
