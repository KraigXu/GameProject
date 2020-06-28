using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001071 RID: 4209
	public class PlaceWorker_RequireNaturePsycaster : PlaceWorker
	{
		// Token: 0x0600640A RID: 25610 RVA: 0x0022A9BC File Offset: 0x00228BBC
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
