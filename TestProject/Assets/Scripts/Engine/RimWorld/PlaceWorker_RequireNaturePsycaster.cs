using System;
using Verse;

namespace RimWorld
{
	
	public class PlaceWorker_RequireNaturePsycaster : PlaceWorker
	{
		
		public override bool IsBuildDesignatorVisible(BuildableDef def)
		{
			foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
			{
				if (MeditationFocusDefOf.Natural.CanPawnUse(p))
				{
					return true;
				}
			}
			return false;
		}
	}
}
