using System;

namespace Verse.AI.Group
{
	
	public class Trigger_OnClamor : Trigger
	{
		
		public Trigger_OnClamor(ClamorDef clamorType)
		{
			this.clamorType = clamorType;
		}

		
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Clamor && signal.clamorType == this.clamorType;
		}

		
		private ClamorDef clamorType;
	}
}
