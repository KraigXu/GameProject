using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001160 RID: 4448
	public class QuestNode_AllowDecreesForLodger : QuestNode
	{
		// Token: 0x06006798 RID: 26520 RVA: 0x00243947 File Offset: 0x00241B47
		protected override void RunInt()
		{
			QuestGen.quest.AddPart(new QuestPart_AllowDecreesForLodger
			{
				lodger = this.lodger.GetValue(QuestGen.slate)
			});
		}

		// Token: 0x06006799 RID: 26521 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04003FB5 RID: 16309
		public SlateRef<Pawn> lodger;
	}
}
