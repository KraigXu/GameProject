using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_ResolveTextNow : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			string var = QuestGenUtility.ResolveLocalTextWithDescriptionRules(this.rules.GetValue(slate), this.root.GetValue(slate));
			slate.Set<string>(this.storeAs.GetValue(slate), var, false);
		}

		
		[NoTranslate]
		public SlateRef<string> root;

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<RulePack> rules;
	}
}
