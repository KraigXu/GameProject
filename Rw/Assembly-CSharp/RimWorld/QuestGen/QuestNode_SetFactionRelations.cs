using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001149 RID: 4425
	public class QuestNode_SetFactionRelations : QuestNode
	{
		// Token: 0x06006747 RID: 26439 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006748 RID: 26440 RVA: 0x00242970 File Offset: 0x00240B70
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

		// Token: 0x04003F67 RID: 16231
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003F68 RID: 16232
		public SlateRef<Faction> faction;

		// Token: 0x04003F69 RID: 16233
		public SlateRef<FactionRelationKind> relationKind;

		// Token: 0x04003F6A RID: 16234
		public SlateRef<bool?> sendLetter;

		// Token: 0x04003F6B RID: 16235
		private const string RootSymbol = "root";
	}
}
