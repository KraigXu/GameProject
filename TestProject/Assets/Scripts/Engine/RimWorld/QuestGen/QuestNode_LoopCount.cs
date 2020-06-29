using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_LoopCount : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			for (int i = 0; i < this.loopCount.GetValue(slate); i++)
			{
				if (this.storeLoopCounterAs.GetValue(slate) != null)
				{
					slate.Set<int>(this.storeLoopCounterAs.GetValue(slate), i, false);
				}
				try
				{
					if (!this.node.TestRun(slate))
					{
						return false;
					}
				}
				finally
				{
					slate.PopPrefix();
				}
			}
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			for (int i = 0; i < this.loopCount.GetValue(slate); i++)
			{
				if (this.storeLoopCounterAs.GetValue(slate) != null)
				{
					QuestGen.slate.Set<int>(this.storeLoopCounterAs.GetValue(slate), i, false);
				}
				try
				{
					this.node.Run();
				}
				finally
				{
					QuestGen.slate.PopPrefix();
				}
			}
		}

		
		public QuestNode node;

		
		public SlateRef<int> loopCount;

		
		[NoTranslate]
		public SlateRef<string> storeLoopCounterAs;
	}
}
