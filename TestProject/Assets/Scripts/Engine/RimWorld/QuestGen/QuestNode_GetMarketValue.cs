using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetMarketValue : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		
		private bool DoWork(Slate slate)
		{
			float num = 0f;
			if (this.things.GetValue(slate) != null)
			{
				foreach (Thing thing in this.things.GetValue(slate))
				{
					num += thing.MarketValue * (float)thing.stackCount;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
			return true;
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<IEnumerable<Thing>> things;
	}
}
