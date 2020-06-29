using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_ThroneroomInvalidConfiguration : Alert
	{
		
		public Alert_ThroneroomInvalidConfiguration()
		{
			this.defaultLabel = "ThroneroomInvalidConfiguration".Translate();
			this.defaultExplanation = "ThroneroomInvalidConfigurationDesc".Translate();
		}

		
		public override TaggedString GetExplanation()
		{
			return base.GetExplanation() + "\n\n" + Alert_ThroneroomInvalidConfiguration.validationInfo;
		}

		
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Thing thing in maps[i].listerThings.ThingsInGroup(ThingRequestGroup.Throne))
				{
					Building_Throne building_Throne = (Building_Throne)thing;
					Alert_ThroneroomInvalidConfiguration.validationInfo = RoomRoleWorker_ThroneRoom.Validate(building_Throne.GetRoom(RegionType.Set_Passable));
					if (Alert_ThroneroomInvalidConfiguration.validationInfo != null)
					{
						return AlertReport.CulpritIs(building_Throne);
					}
				}
			}
			return false;
		}

		
		private static string validationInfo;
	}
}
