using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	
	public static class DebugToolsMisc
	{
		
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AttachFire()
		{
			foreach (Thing t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				t.TryAttachFire(1f);
			}
		}
	}
}
