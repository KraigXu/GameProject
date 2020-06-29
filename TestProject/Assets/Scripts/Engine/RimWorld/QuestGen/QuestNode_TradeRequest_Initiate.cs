using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_TradeRequest_Initiate : QuestNode
	{
		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_InitiateTradeRequest questPart_InitiateTradeRequest = new QuestPart_InitiateTradeRequest();
			questPart_InitiateTradeRequest.settlement = this.settlement.GetValue(slate);
			questPart_InitiateTradeRequest.requestedThingDef = this.requestedThingDef.GetValue(slate);
			questPart_InitiateTradeRequest.requestedCount = this.requestedThingCount.GetValue(slate);
			questPart_InitiateTradeRequest.requestDuration = this.duration.GetValue(slate);
			questPart_InitiateTradeRequest.keepAfterQuestEnds = false;
			questPart_InitiateTradeRequest.inSignal = slate.Get<string>("inSignal", null, false);
			QuestGen.quest.AddPart(questPart_InitiateTradeRequest);
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			return this.settlement.GetValue(slate) != null && this.requestedThingCount.GetValue(slate) > 0 && this.requestedThingDef.GetValue(slate) != null && this.duration.GetValue(slate) > 0;
		}

		
		public SlateRef<Settlement> settlement;

		
		public SlateRef<ThingDef> requestedThingDef;

		
		public SlateRef<int> requestedThingCount;

		
		public SlateRef<int> duration;
	}
}
