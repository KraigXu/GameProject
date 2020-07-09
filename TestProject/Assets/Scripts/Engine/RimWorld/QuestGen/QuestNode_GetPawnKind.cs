using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetPawnKind : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<List<QuestNode_GetPawnKind.Option>> options;

		
		public class Option
		{
			
			public PawnKindDef kindDef;

			
			public float weight;

			
			public bool anyAnimal;

			
			public FleshTypeDef onlyAllowedFleshType;
		}
	}
}
