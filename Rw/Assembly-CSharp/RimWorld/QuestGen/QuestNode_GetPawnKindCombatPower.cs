using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200116F RID: 4463
	public class QuestNode_GetPawnKindCombatPower : QuestNode
	{
		// Token: 0x060067C5 RID: 26565 RVA: 0x002442E9 File Offset: 0x002424E9
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.kindDef.GetValue(slate).combatPower, false);
			return true;
		}

		// Token: 0x060067C6 RID: 26566 RVA: 0x00244310 File Offset: 0x00242510
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<float>(this.storeAs.GetValue(slate), this.kindDef.GetValue(slate).combatPower, false);
		}

		// Token: 0x04003FEF RID: 16367
		public SlateRef<PawnKindDef> kindDef;

		// Token: 0x04003FF0 RID: 16368
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
