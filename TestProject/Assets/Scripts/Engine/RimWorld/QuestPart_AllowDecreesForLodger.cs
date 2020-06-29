using System;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_AllowDecreesForLodger : QuestPart
	{
		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.lodger, "lodger", false);
		}

		
		public Pawn lodger;
	}
}
