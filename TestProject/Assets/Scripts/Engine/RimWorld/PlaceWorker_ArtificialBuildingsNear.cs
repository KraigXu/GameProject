using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001070 RID: 4208
	public class PlaceWorker_ArtificialBuildingsNear : PlaceWorker
	{
		// Token: 0x06006408 RID: 25608 RVA: 0x0022A97C File Offset: 0x00228B7C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			FocusStrengthOffset_ArtificialBuildings focusStrengthOffset_ArtificialBuildings = ((CompProperties_MeditationFocus)def.CompDefFor<CompMeditationFocus>()).offsets.OfType<FocusStrengthOffset_ArtificialBuildings>().FirstOrDefault<FocusStrengthOffset_ArtificialBuildings>();
			if (focusStrengthOffset_ArtificialBuildings != null)
			{
				MeditationUtility.DrawArtificialBuildingOverlay(center, def, Find.CurrentMap, focusStrengthOffset_ArtificialBuildings.radius);
			}
		}
	}
}
