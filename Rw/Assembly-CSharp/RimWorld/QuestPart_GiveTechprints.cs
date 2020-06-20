using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099E RID: 2462
	public class QuestPart_GiveTechprints : QuestPart
	{
		// Token: 0x06003A77 RID: 14967 RVA: 0x00135918 File Offset: 0x00133B18
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.amount; i++)
				{
					Find.ResearchManager.ApplyTechprint(this.project, null);
				}
			}
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x00135964 File Offset: 0x00133B64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalWasGiven, "outSignalWasGiven", null, false);
		}

		// Token: 0x0400227B RID: 8827
		public const string WasGivenSignal = "AddedTechprints";

		// Token: 0x0400227C RID: 8828
		public string inSignal;

		// Token: 0x0400227D RID: 8829
		public string outSignalWasGiven;

		// Token: 0x0400227E RID: 8830
		public ResearchProjectDef project;

		// Token: 0x0400227F RID: 8831
		public int amount;
	}
}
