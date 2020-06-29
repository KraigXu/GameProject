using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_SplitRandomly : QuestNode
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
			float num = this.valueToSplit.GetValue(slate) ?? 1f;
			QuestNode_SplitRandomly.tmpMinFractions.Clear();
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction1.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction2.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction3.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction4.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction5.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Clear();
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow1.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow2.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow3.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow4.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow5.GetValue(slate));
			Rand.SplitRandomly(num, this.countToSplit.GetValue(slate), QuestNode_SplitRandomly.tmpValues, QuestNode_SplitRandomly.tmpZeroIfFractionBelow, QuestNode_SplitRandomly.tmpMinFractions);
			for (int i = 0; i < QuestNode_SplitRandomly.tmpValues.Count; i++)
			{
				if (this.storeAsFormat.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAsFormat.GetValue(slate).Formatted(i.Named("INDEX")), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				if (i == 0 && this.storeAs1.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs1.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 1 && this.storeAs2.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs2.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 2 && this.storeAs3.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs3.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 3 && this.storeAs4.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs4.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 4 && this.storeAs5.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs5.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
			}
		}

		
		[NoTranslate]
		public SlateRef<string> storeAsFormat;

		
		[NoTranslate]
		public SlateRef<string> storeAs1;

		
		[NoTranslate]
		public SlateRef<string> storeAs2;

		
		[NoTranslate]
		public SlateRef<string> storeAs3;

		
		[NoTranslate]
		public SlateRef<string> storeAs4;

		
		[NoTranslate]
		public SlateRef<string> storeAs5;

		
		public SlateRef<float?> valueToSplit;

		
		public SlateRef<int> countToSplit;

		
		public SlateRef<float> zeroIfFractionBelow1;

		
		public SlateRef<float> zeroIfFractionBelow2;

		
		public SlateRef<float> zeroIfFractionBelow3;

		
		public SlateRef<float> zeroIfFractionBelow4;

		
		public SlateRef<float> zeroIfFractionBelow5;

		
		public SlateRef<float> minFraction1;

		
		public SlateRef<float> minFraction2;

		
		public SlateRef<float> minFraction3;

		
		public SlateRef<float> minFraction4;

		
		public SlateRef<float> minFraction5;

		
		private static List<float> tmpValues = new List<float>();

		
		private static List<float> tmpZeroIfFractionBelow = new List<float>();

		
		private static List<float> tmpMinFractions = new List<float>();
	}
}
