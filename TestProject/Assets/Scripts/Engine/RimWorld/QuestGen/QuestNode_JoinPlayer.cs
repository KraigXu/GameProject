using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001175 RID: 4469
	public class QuestNode_JoinPlayer : QuestNode
	{
		// Token: 0x060067E0 RID: 26592 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067E1 RID: 26593 RVA: 0x002452E8 File Offset: 0x002434E8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_JoinPlayer questPart_JoinPlayer = new QuestPart_JoinPlayer();
			questPart_JoinPlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_JoinPlayer.joinPlayer = this.joinPlayer.GetValue(slate);
			questPart_JoinPlayer.makePrisoners = this.makePrisoners.GetValue(slate);
			questPart_JoinPlayer.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			questPart_JoinPlayer.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_JoinPlayer);
		}

		// Token: 0x04004010 RID: 16400
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04004011 RID: 16401
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04004012 RID: 16402
		public SlateRef<bool> joinPlayer;

		// Token: 0x04004013 RID: 16403
		public SlateRef<bool> makePrisoners;
	}
}
