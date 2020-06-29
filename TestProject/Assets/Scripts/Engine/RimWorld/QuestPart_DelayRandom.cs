using System;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_DelayRandom : QuestPart_Delay
	{
		
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.delayTicks = this.delayTicksRange.RandomInRange;
			base.Enable(receivedArgs);
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.delayTicksRange, "delayTicksRange", default(IntRange), false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.delayTicksRange = new IntRange(833, 2500);
		}

		
		public IntRange delayTicksRange;
	}
}
