using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetRandomElementByWeight : QuestNode
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
			QuestNode_GetRandomElementByWeight.Option option;
			if (this.options.TryRandomElementByWeight((QuestNode_GetRandomElementByWeight.Option x) => x.weight, out option))
			{
				slate.Set<object>(this.storeAs.GetValue(slate), option.element.GetValue(slate), false);
			}
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public List<QuestNode_GetRandomElementByWeight.Option> options = new List<QuestNode_GetRandomElementByWeight.Option>();

		
		public class Option
		{
			
			public SlateRef<object> element;

			
			public float weight;
		}
	}
}
