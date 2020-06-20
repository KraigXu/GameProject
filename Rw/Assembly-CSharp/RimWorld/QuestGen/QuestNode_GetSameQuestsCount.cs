using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001142 RID: 4418
	public class QuestNode_GetSameQuestsCount : QuestNode
	{
		// Token: 0x06006729 RID: 26409 RVA: 0x00241C78 File Offset: 0x0023FE78
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600672A RID: 26410 RVA: 0x00241C82 File Offset: 0x0023FE82
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600672B RID: 26411 RVA: 0x00241C90 File Offset: 0x0023FE90
		private void SetVars(Slate slate)
		{
			int var = Find.QuestManager.QuestsListForReading.Count((Quest x) => x.root == QuestGen.Root);
			slate.Set<int>("sameQuestsCount", var, false);
		}

		// Token: 0x04003F4C RID: 16204
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
