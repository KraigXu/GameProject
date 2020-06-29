using System;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_GiveTechprints : QuestPart
	{
		
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

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalWasGiven, "outSignalWasGiven", null, false);
		}

		
		public const string WasGivenSignal = "AddedTechprints";

		
		public string inSignal;

		
		public string outSignalWasGiven;

		
		public ResearchProjectDef project;

		
		public int amount;
	}
}
