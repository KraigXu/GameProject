using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020011A3 RID: 4515
	public class QuestNode_ResolveTextNow : QuestNode
	{
		// Token: 0x0600687D RID: 26749 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600687E RID: 26750 RVA: 0x00247B64 File Offset: 0x00245D64
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			string var = QuestGenUtility.ResolveLocalTextWithDescriptionRules(this.rules.GetValue(slate), this.root.GetValue(slate));
			slate.Set<string>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x040040C9 RID: 16585
		[NoTranslate]
		public SlateRef<string> root;

		// Token: 0x040040CA RID: 16586
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040040CB RID: 16587
		public SlateRef<RulePack> rules;
	}
}
