using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001168 RID: 4456
	public class QuestNode_DestroyWorldObject : QuestNode
	{
		// Token: 0x060067B0 RID: 26544 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067B1 RID: 26545 RVA: 0x00243CFC File Offset: 0x00241EFC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_DestroyWorldObject questPart_DestroyWorldObject = new QuestPart_DestroyWorldObject();
			questPart_DestroyWorldObject.worldObject = this.worldObject.GetValue(slate);
			questPart_DestroyWorldObject.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_DestroyWorldObject);
		}

		// Token: 0x04003FCB RID: 16331
		public SlateRef<WorldObject> worldObject;

		// Token: 0x04003FCC RID: 16332
		[NoTranslate]
		public SlateRef<string> inSignal;
	}
}
