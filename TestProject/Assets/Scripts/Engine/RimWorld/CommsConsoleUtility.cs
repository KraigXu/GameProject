using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public static class CommsConsoleUtility
	{
		
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
