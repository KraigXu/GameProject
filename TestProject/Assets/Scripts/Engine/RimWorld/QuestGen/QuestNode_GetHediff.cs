using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetHediff : QuestNode
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
			QuestNode_GetHediff.Option option = this.options.GetValue(slate).RandomElementByWeight((QuestNode_GetHediff.Option x) => x.weight);
			slate.Set<HediffDef>(this.storeHediffAs.GetValue(slate), option.def, false);
			if (this.storePartsToAffectAs.GetValue(slate) != null)
			{
				slate.Set<List<BodyPartDef>>(this.storePartsToAffectAs.GetValue(slate), option.partsToAffect, false);
			}
		}

		
		[NoTranslate]
		public SlateRef<string> storeHediffAs;

		
		[NoTranslate]
		public SlateRef<string> storePartsToAffectAs;

		
		public SlateRef<List<QuestNode_GetHediff.Option>> options;

		
		public class Option
		{
			
			public HediffDef def;

			
			public List<BodyPartDef> partsToAffect;

			
			public float weight = 1f;
		}
	}
}
