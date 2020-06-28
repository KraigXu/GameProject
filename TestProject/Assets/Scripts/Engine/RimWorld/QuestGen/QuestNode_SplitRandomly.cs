using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200114A RID: 4426
	public class QuestNode_SplitRandomly : QuestNode
	{
		// Token: 0x0600674A RID: 26442 RVA: 0x00242A0A File Offset: 0x00240C0A
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600674B RID: 26443 RVA: 0x00242A14 File Offset: 0x00240C14
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600674C RID: 26444 RVA: 0x00242A24 File Offset: 0x00240C24
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

		// Token: 0x04003F6C RID: 16236
		[NoTranslate]
		public SlateRef<string> storeAsFormat;

		// Token: 0x04003F6D RID: 16237
		[NoTranslate]
		public SlateRef<string> storeAs1;

		// Token: 0x04003F6E RID: 16238
		[NoTranslate]
		public SlateRef<string> storeAs2;

		// Token: 0x04003F6F RID: 16239
		[NoTranslate]
		public SlateRef<string> storeAs3;

		// Token: 0x04003F70 RID: 16240
		[NoTranslate]
		public SlateRef<string> storeAs4;

		// Token: 0x04003F71 RID: 16241
		[NoTranslate]
		public SlateRef<string> storeAs5;

		// Token: 0x04003F72 RID: 16242
		public SlateRef<float?> valueToSplit;

		// Token: 0x04003F73 RID: 16243
		public SlateRef<int> countToSplit;

		// Token: 0x04003F74 RID: 16244
		public SlateRef<float> zeroIfFractionBelow1;

		// Token: 0x04003F75 RID: 16245
		public SlateRef<float> zeroIfFractionBelow2;

		// Token: 0x04003F76 RID: 16246
		public SlateRef<float> zeroIfFractionBelow3;

		// Token: 0x04003F77 RID: 16247
		public SlateRef<float> zeroIfFractionBelow4;

		// Token: 0x04003F78 RID: 16248
		public SlateRef<float> zeroIfFractionBelow5;

		// Token: 0x04003F79 RID: 16249
		public SlateRef<float> minFraction1;

		// Token: 0x04003F7A RID: 16250
		public SlateRef<float> minFraction2;

		// Token: 0x04003F7B RID: 16251
		public SlateRef<float> minFraction3;

		// Token: 0x04003F7C RID: 16252
		public SlateRef<float> minFraction4;

		// Token: 0x04003F7D RID: 16253
		public SlateRef<float> minFraction5;

		// Token: 0x04003F7E RID: 16254
		private static List<float> tmpValues = new List<float>();

		// Token: 0x04003F7F RID: 16255
		private static List<float> tmpZeroIfFractionBelow = new List<float>();

		// Token: 0x04003F80 RID: 16256
		private static List<float> tmpMinFractions = new List<float>();
	}
}
