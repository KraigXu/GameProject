﻿using System;

namespace Verse.AI.Group
{
	
	public class Trigger_PawnsLost : Trigger
	{
		
		public Trigger_PawnsLost(int count)
		{
			this.count = count;
		}

		
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && lord.numPawnsLostViolently >= this.count;
		}

		
		private int count = 1;
	}
}
