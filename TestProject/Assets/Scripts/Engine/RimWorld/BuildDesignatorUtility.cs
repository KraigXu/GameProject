using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3C RID: 3644
	public static class BuildDesignatorUtility
	{
		// Token: 0x06005826 RID: 22566 RVA: 0x001D4278 File Offset: 0x001D2478
		public static void TryDrawPowerGridAndAnticipatedConnection(BuildableDef def, Rot4 rotation)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null && (thingDef.EverTransmitsPower || thingDef.ConnectToPower))
			{
				OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
				if (thingDef.ConnectToPower)
				{
					IntVec3 intVec = UI.MouseCell();
					CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(intVec, Find.CurrentMap, null);
					if (compPower != null && !compPower.parent.Position.Fogged(compPower.parent.Map))
					{
						PowerNetGraphics.RenderAnticipatedWirePieceConnecting(intVec, rotation, def.Size, compPower.parent);
					}
				}
			}
		}
	}
}
