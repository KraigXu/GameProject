using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetAnimalKindByPoints : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.SetVars(slate);
		}

		
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		
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
