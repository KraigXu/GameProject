using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200110C RID: 4364
	public class QuestNode_Set : QuestNode
	{
		// Token: 0x06006654 RID: 26196 RVA: 0x0023D7A9 File Offset: 0x0023B9A9
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			return true;
		}

		// Token: 0x06006655 RID: 26197 RVA: 0x0023D7CC File Offset: 0x0023B9CC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
		}

		// Token: 0x04003E6A RID: 15978
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x04003E6B RID: 15979
		public SlateRef<object> value;
	}
}
