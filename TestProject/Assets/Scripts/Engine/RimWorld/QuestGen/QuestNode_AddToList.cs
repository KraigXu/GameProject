using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001100 RID: 4352
	public class QuestNode_AddToList : QuestNode
	{
		// Token: 0x0600662E RID: 26158 RVA: 0x0023C998 File Offset: 0x0023AB98
		protected override bool TestRunInt(Slate slate)
		{
			QuestGenUtility.AddToOrMakeList(slate, this.name.GetValue(slate), this.value.GetValue(slate));
			return true;
		}

		// Token: 0x0600662F RID: 26159 RVA: 0x0023C9BC File Offset: 0x0023ABBC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.name.GetValue(slate), this.value.GetValue(slate));
		}

		// Token: 0x04003E39 RID: 15929
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x04003E3A RID: 15930
		public SlateRef<object> value;
	}
}
