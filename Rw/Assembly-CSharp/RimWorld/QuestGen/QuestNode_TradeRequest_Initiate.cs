using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200119B RID: 4507
	public class QuestNode_TradeRequest_Initiate : QuestNode
	{
		// Token: 0x0600685E RID: 26718 RVA: 0x00247340 File Offset: 0x00245540
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

		// Token: 0x0600685F RID: 26719 RVA: 0x002473C8 File Offset: 0x002455C8
		protected override bool TestRunInt(Slate slate)
		{
			return this.settlement.GetValue(slate) != null && this.requestedThingCount.GetValue(slate) > 0 && this.requestedThingDef.GetValue(slate) != null && this.duration.GetValue(slate) > 0;
		}

		// Token: 0x040040AA RID: 16554
		public SlateRef<Settlement> settlement;

		// Token: 0x040040AB RID: 16555
		public SlateRef<ThingDef> requestedThingDef;

		// Token: 0x040040AC RID: 16556
		public SlateRef<int> requestedThingCount;

		// Token: 0x040040AD RID: 16557
		public SlateRef<int> duration;
	}
}
