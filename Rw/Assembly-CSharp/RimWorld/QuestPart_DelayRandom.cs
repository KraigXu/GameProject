using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000941 RID: 2369
	public class QuestPart_DelayRandom : QuestPart_Delay
	{
		// Token: 0x06003837 RID: 14391 RVA: 0x0012D5DD File Offset: 0x0012B7DD
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.delayTicks = this.delayTicksRange.RandomInRange;
			base.Enable(receivedArgs);
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x0012D5F8 File Offset: 0x0012B7F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.delayTicksRange, "delayTicksRange", default(IntRange), false);
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x0012D625 File Offset: 0x0012B825
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.delayTicksRange = new IntRange(833, 2500);
		}

		// Token: 0x04002141 RID: 8513
		public IntRange delayTicksRange;
	}
}
