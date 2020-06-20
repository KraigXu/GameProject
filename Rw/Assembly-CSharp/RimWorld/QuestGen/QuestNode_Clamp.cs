using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001104 RID: 4356
	public class QuestNode_Clamp : QuestNode
	{
		// Token: 0x0600663A RID: 26170 RVA: 0x0023CFB3 File Offset: 0x0023B1B3
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600663B RID: 26171 RVA: 0x0023CFBD File Offset: 0x0023B1BD
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600663C RID: 26172 RVA: 0x0023CFCC File Offset: 0x0023B1CC
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

		// Token: 0x04003E48 RID: 15944
		public SlateRef<double> value;

		// Token: 0x04003E49 RID: 15945
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003E4A RID: 15946
		public SlateRef<double?> min;

		// Token: 0x04003E4B RID: 15947
		public SlateRef<double?> max;
	}
}
