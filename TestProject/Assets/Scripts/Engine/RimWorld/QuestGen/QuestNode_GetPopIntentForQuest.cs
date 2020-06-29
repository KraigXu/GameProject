using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetPopIntentForQuest : QuestNode
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
			float populationIntentForQuest = StorytellerUtilityPopulation.PopulationIntentForQuest;
			slate.Set<float>(this.storeAs.GetValue(slate), populationIntentForQuest, false);
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
