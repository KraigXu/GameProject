using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001127 RID: 4391
	public class QuestNode_GetHediff : QuestNode
	{
		// Token: 0x060066B3 RID: 26291 RVA: 0x0023F2C7 File Offset: 0x0023D4C7
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060066B4 RID: 26292 RVA: 0x0023F2D1 File Offset: 0x0023D4D1
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060066B5 RID: 26293 RVA: 0x0023F2E0 File Offset: 0x0023D4E0
		private void SetVars(Slate slate)
		{
			QuestNode_GetHediff.Option option = this.options.GetValue(slate).RandomElementByWeight((QuestNode_GetHediff.Option x) => x.weight);
			slate.Set<HediffDef>(this.storeHediffAs.GetValue(slate), option.def, false);
			if (this.storePartsToAffectAs.GetValue(slate) != null)
			{
				slate.Set<List<BodyPartDef>>(this.storePartsToAffectAs.GetValue(slate), option.partsToAffect, false);
			}
		}

		// Token: 0x04003EDC RID: 16092
		[NoTranslate]
		public SlateRef<string> storeHediffAs;

		// Token: 0x04003EDD RID: 16093
		[NoTranslate]
		public SlateRef<string> storePartsToAffectAs;

		// Token: 0x04003EDE RID: 16094
		public SlateRef<List<QuestNode_GetHediff.Option>> options;

		// Token: 0x02001F21 RID: 7969
		public class Option
		{
			// Token: 0x040074E7 RID: 29927
			public HediffDef def;

			// Token: 0x040074E8 RID: 29928
			public List<BodyPartDef> partsToAffect;

			// Token: 0x040074E9 RID: 29929
			public float weight = 1f;
		}
	}
}
