using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200110D RID: 4365
	public class QuestNode_SetAndRestore : QuestNode
	{
		// Token: 0x06006657 RID: 26199 RVA: 0x0023D804 File Offset: 0x0023BA04
		protected override bool TestRunInt(Slate slate)
		{
			Slate.VarRestoreInfo restoreInfo = slate.GetRestoreInfo(this.name.GetValue(slate));
			slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			bool result;
			try
			{
				result = this.node.TestRun(slate);
			}
			finally
			{
				slate.Restore(restoreInfo);
			}
			return result;
		}

		// Token: 0x06006658 RID: 26200 RVA: 0x0023D86C File Offset: 0x0023BA6C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Slate.VarRestoreInfo restoreInfo = QuestGen.slate.GetRestoreInfo(this.name.GetValue(slate));
			QuestGen.slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			try
			{
				this.node.Run();
			}
			finally
			{
				QuestGen.slate.Restore(restoreInfo);
			}
		}

		// Token: 0x04003E6C RID: 15980
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x04003E6D RID: 15981
		public SlateRef<object> value;

		// Token: 0x04003E6E RID: 15982
		public QuestNode node;
	}
}
