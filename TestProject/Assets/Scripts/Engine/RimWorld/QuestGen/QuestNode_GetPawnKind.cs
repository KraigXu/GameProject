using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001135 RID: 4405
	public class QuestNode_GetPawnKind : QuestNode
	{
		// Token: 0x060066F4 RID: 26356 RVA: 0x0024103A File Offset: 0x0023F23A
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060066F5 RID: 26357 RVA: 0x00241044 File Offset: 0x0023F244
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060066F6 RID: 26358 RVA: 0x00241054 File Offset: 0x0023F254
		private void SetVars(Slate slate)
		{
			QuestNode_GetPawnKind.Option option = this.options.GetValue(slate).RandomElementByWeight((QuestNode_GetPawnKind.Option x) => x.weight);
			PawnKindDef var;
			if (option.kindDef != null)
			{
				var = option.kindDef;
			}
			else if (option.anyAnimal)
			{
				var = (from x in DefDatabase<PawnKindDef>.AllDefs
				where x.RaceProps.Animal && (option.onlyAllowedFleshType == null || x.RaceProps.FleshType == option.onlyAllowedFleshType)
				select x).RandomElement<PawnKindDef>();
			}
			else
			{
				var = null;
			}
			slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x04003F1E RID: 16158
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F1F RID: 16159
		public SlateRef<List<QuestNode_GetPawnKind.Option>> options;

		// Token: 0x02001F31 RID: 7985
		public class Option
		{
			// Token: 0x0400750C RID: 29964
			public PawnKindDef kindDef;

			// Token: 0x0400750D RID: 29965
			public float weight;

			// Token: 0x0400750E RID: 29966
			public bool anyAnimal;

			// Token: 0x0400750F RID: 29967
			public FleshTypeDef onlyAllowedFleshType;
		}
	}
}
