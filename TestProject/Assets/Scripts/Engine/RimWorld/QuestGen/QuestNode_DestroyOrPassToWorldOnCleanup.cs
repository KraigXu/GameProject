using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001167 RID: 4455
	public class QuestNode_DestroyOrPassToWorldOnCleanup : QuestNode
	{
		// Token: 0x060067AD RID: 26541 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067AE RID: 26542 RVA: 0x00243CAC File Offset: 0x00241EAC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.things.GetValue(slate).EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_DestroyThingsOrPassToWorldOnCleanup questPart_DestroyThingsOrPassToWorldOnCleanup = new QuestPart_DestroyThingsOrPassToWorldOnCleanup();
			questPart_DestroyThingsOrPassToWorldOnCleanup.things.AddRange(this.things.GetValue(slate));
			QuestGen.quest.AddPart(questPart_DestroyThingsOrPassToWorldOnCleanup);
		}

		// Token: 0x04003FCA RID: 16330
		public SlateRef<IEnumerable<Thing>> things;
	}
}
