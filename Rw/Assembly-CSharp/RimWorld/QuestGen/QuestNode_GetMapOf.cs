using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200112B RID: 4395
	public class QuestNode_GetMapOf : QuestNode
	{
		// Token: 0x060066C5 RID: 26309 RVA: 0x0023F70D File Offset: 0x0023D90D
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x060066C6 RID: 26310 RVA: 0x0023F717 File Offset: 0x0023D917
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060066C7 RID: 26311 RVA: 0x0023F724 File Offset: 0x0023D924
		private void DoWork(Slate slate)
		{
			if (this.mapOf.GetValue(slate) != null)
			{
				slate.Set<Map>(this.storeAs.GetValue(slate), this.mapOf.GetValue(slate).MapHeld, false);
			}
		}

		// Token: 0x04003EE7 RID: 16103
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x04003EE8 RID: 16104
		public SlateRef<Thing> mapOf;
	}
}
