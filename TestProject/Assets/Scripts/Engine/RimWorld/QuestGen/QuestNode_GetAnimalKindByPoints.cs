using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200111D RID: 4381
	public class QuestNode_GetAnimalKindByPoints : QuestNode
	{
		// Token: 0x06006689 RID: 26249 RVA: 0x0023E8F9 File Offset: 0x0023CAF9
		protected override bool TestRunInt(Slate slate)
		{
			return this.SetVars(slate);
		}

		// Token: 0x0600668A RID: 26250 RVA: 0x0023E902 File Offset: 0x0023CB02
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600668B RID: 26251 RVA: 0x0023E910 File Offset: 0x0023CB10
		private bool SetVars(Slate slate)
		{
			float points = slate.Get<float>("points", 0f, false);
			PawnKindDef var;
			if ((from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal && x.combatPower < points
			select x).TryRandomElement(out var))
			{
				slate.Set<PawnKindDef>("animalKindDef", var, false);
				return true;
			}
			return false;
		}
	}
}
