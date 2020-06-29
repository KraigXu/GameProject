using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordToilData_Party : LordToilData
	{
		
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.presentForTicks.RemoveAll((KeyValuePair<Pawn, int> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Pawn, int>(ref this.presentForTicks, "presentForTicks", LookMode.Reference, LookMode.Undefined);
		}

		
		public Dictionary<Pawn, int> presentForTicks = new Dictionary<Pawn, int>();
	}
}
