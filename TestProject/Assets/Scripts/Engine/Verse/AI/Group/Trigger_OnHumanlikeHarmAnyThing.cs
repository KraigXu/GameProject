using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	
	public class Trigger_OnHumanlikeHarmAnyThing : Trigger
	{
		
		public Trigger_OnHumanlikeHarmAnyThing(List<Thing> things)
		{
			this.things = things;
		}

		
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			Pawn pawn;
			return signal.dinfo.Def != null && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.dinfo.Instigator != null && (pawn = (signal.dinfo.Instigator as Pawn)) != null && pawn.RaceProps.Humanlike && this.things.Contains(signal.thing);
		}

		
		private List<Thing> things;
	}
}
