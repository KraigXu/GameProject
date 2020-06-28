using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200111F RID: 4383
	public class QuestNode_GetBodySize : QuestNode
	{
		// Token: 0x06006692 RID: 26258 RVA: 0x0023EB2E File Offset: 0x0023CD2E
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.pawnKind.GetValue(slate).RaceProps.baseBodySize, false);
			return true;
		}

		// Token: 0x06006693 RID: 26259 RVA: 0x0023EB5C File Offset: 0x0023CD5C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<float>(this.storeAs.GetValue(slate), this.pawnKind.GetValue(slate).RaceProps.baseBodySize, false);
		}

		// Token: 0x04003EB9 RID: 16057
		public SlateRef<PawnKindDef> pawnKind;

		// Token: 0x04003EBA RID: 16058
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
