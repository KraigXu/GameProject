using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001161 RID: 4449
	public class QuestNode_BiocodeWeapons : QuestNode
	{
		// Token: 0x0600679B RID: 26523 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600679C RID: 26524 RVA: 0x00243970 File Offset: 0x00241B70
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_BiocodeWeapons questPart_BiocodeWeapons = new QuestPart_BiocodeWeapons();
			questPart_BiocodeWeapons.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_BiocodeWeapons.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_BiocodeWeapons);
		}

		// Token: 0x04003FB6 RID: 16310
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FB7 RID: 16311
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
