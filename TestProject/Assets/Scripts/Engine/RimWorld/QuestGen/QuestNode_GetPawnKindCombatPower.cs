using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetPawnKindCombatPower : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.kindDef.GetValue(slate).combatPower, false);
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<float>(this.storeAs.GetValue(slate), this.kindDef.GetValue(slate).combatPower, false);
		}

		
		public SlateRef<PawnKindDef> kindDef;

		
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
