using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetRandomPawnKindForFaction : QuestNode
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
			Thing value = this.factionOf.GetValue(slate);
			if (value == null)
			{
				return false;
			}
			Faction faction = value.Faction;
			if (faction == null)
			{
				return false;
			}
			List<QuestNode_GetRandomPawnKindForFaction.Choice> value2 = this.choices.GetValue(slate);
			for (int i = 0; i < value2.Count; i++)
			{
				PawnKindDef var;
				if (((value2[i].factionDef != null && faction.def == value2[i].factionDef) || (!value2[i].categoryTag.NullOrEmpty() && value2[i].categoryTag == faction.def.categoryTag)) && value2[i].pawnKinds.TryRandomElement(out var))
				{
					slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), var, false);
					return true;
				}
			}
			if (this.fallback.GetValue(slate) != null)
			{
				slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), this.fallback.GetValue(slate), false);
				return true;
			}
			return false;
		}

		
		public SlateRef<Thing> factionOf;

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<List<QuestNode_GetRandomPawnKindForFaction.Choice>> choices;

		
		public SlateRef<PawnKindDef> fallback;

		
		public class Choice
		{
			
			public FactionDef factionDef;

			
			public string categoryTag;

			
			public List<PawnKindDef> pawnKinds;
		}
	}
}
