using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_Clamp : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		
		private void DoWork(Slate slate)
		{
			double num = this.value.GetValue(slate);
			if (this.min.GetValue(slate) != null)
			{
				num = Math.Max(num, this.min.GetValue(slate).Value);
			}
			if (this.max.GetValue(slate) != null)
			{
				num = Math.Min(num, this.max.GetValue(slate).Value);
			}
			slate.Set<double>(this.storeAs.GetValue(slate), num, false);
		}

		
		public SlateRef<double> value;

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<double?> min;

		
		public SlateRef<double?> max;
	}
}
