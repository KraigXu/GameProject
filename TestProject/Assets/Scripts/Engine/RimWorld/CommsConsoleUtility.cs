using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C77 RID: 3191
	public static class CommsConsoleUtility
	{
		// Token: 0x06004C8C RID: 19596 RVA: 0x0019B040 File Offset: 0x00199240
		public static bool PlayerHasPoweredCommsConsole(Map map)
		{
			foreach (Building_CommsConsole building_CommsConsole in map.listerBuildings.AllBuildingsColonistOfClass<Building_CommsConsole>())
			{
				if (building_CommsConsole.Faction == Faction.OfPlayer)
				{
					CompPowerTrader compPowerTrader = building_CommsConsole.TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004C8D RID: 19597 RVA: 0x0019B0B4 File Offset: 0x001992B4
		public static bool PlayerHasPoweredCommsConsole()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (CommsConsoleUtility.PlayerHasPoweredCommsConsole(maps[i]))
				{
					return true;
				}
			}
			return false;
		}
	}
}
