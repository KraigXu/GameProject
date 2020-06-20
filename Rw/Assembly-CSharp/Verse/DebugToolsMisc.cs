using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200033C RID: 828
	public static class DebugToolsMisc
	{
		// Token: 0x06001897 RID: 6295 RVA: 0x0008D964 File Offset: 0x0008BB64
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
