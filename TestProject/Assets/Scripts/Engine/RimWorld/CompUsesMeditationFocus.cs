using System;
using Verse;

namespace RimWorld
{
	
	public class CompUsesMeditationFocus : ThingComp
	{
		
		public override void PostDrawExtraSelectionOverlays()
		{
			MeditationUtility.DrawMeditationSpotOverlay(this.parent.Position, this.parent.Map);
		}
	}
}
