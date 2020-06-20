using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200117B RID: 4475
	public class QuestNode_MakeMinified : QuestNode
	{
		// Token: 0x060067F2 RID: 26610 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067F3 RID: 26611 RVA: 0x00245894 File Offset: 0x00243A94
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MinifiedThing var = this.thing.GetValue(slate).MakeMinified();
			QuestGen.slate.Set<MinifiedThing>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x04004030 RID: 16432
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04004031 RID: 16433
		public SlateRef<Thing> thing;
	}
}
