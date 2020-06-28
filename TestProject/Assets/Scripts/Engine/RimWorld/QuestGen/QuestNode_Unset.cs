using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001111 RID: 4369
	public class QuestNode_Unset : QuestNode
	{
		// Token: 0x06006664 RID: 26212 RVA: 0x0023DDA2 File Offset: 0x0023BFA2
		protected override bool TestRunInt(Slate slate)
		{
			slate.Remove(this.name.GetValue(slate), false);
			return true;
		}

		// Token: 0x06006665 RID: 26213 RVA: 0x0023DDBC File Offset: 0x0023BFBC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Remove(this.name.GetValue(slate), false);
		}

		// Token: 0x04003E7F RID: 15999
		[NoTranslate]
		public SlateRef<string> name;
	}
}
