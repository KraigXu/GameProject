using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020010FE RID: 4350
	public class QuestNode_AddRangeToList : QuestNode
	{
		// Token: 0x06006628 RID: 26152 RVA: 0x0023C884 File Offset: 0x0023AA84
		protected override bool TestRunInt(Slate slate)
		{
			List<object> list = this.value.GetValue(slate);
			if (list != null)
			{
				QuestGenUtility.AddRangeToOrMakeList(slate, this.name.GetValue(slate), list);
			}
			return true;
		}

		// Token: 0x06006629 RID: 26153 RVA: 0x0023C8B8 File Offset: 0x0023AAB8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			List<object> list = this.value.GetValue(slate);
			if (list != null)
			{
				QuestGenUtility.AddRangeToOrMakeList(slate, this.name.GetValue(slate), list);
			}
		}

		// Token: 0x04003E35 RID: 15925
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x04003E36 RID: 15926
		public SlateRef<List<object>> value;
	}
}
