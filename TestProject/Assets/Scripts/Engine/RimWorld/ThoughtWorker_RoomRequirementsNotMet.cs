using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public abstract class ThoughtWorker_RoomRequirementsNotMet : ThoughtWorker
	{
		
		protected abstract IEnumerable<string> UnmetRequirements(Pawn p);

		
		protected bool Active(Pawn p)
		{
			return p.royalty != null && p.MapHeld != null && p.MapHeld.IsPlayerHome && this.UnmetRequirements(p).Any<string>();
		}

		
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!this.Active(p))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
