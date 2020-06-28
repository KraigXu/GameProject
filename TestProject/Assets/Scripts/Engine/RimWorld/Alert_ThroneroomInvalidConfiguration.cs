using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF8 RID: 3576
	public class Alert_ThroneroomInvalidConfiguration : Alert
	{
		// Token: 0x06005691 RID: 22161 RVA: 0x001CB44F File Offset: 0x001C964F
		public Alert_ThroneroomInvalidConfiguration()
		{
			this.defaultLabel = "ThroneroomInvalidConfiguration".Translate();
			this.defaultExplanation = "ThroneroomInvalidConfigurationDesc".Translate();
		}

		// Token: 0x06005692 RID: 22162 RVA: 0x001CB481 File Offset: 0x001C9681
		public override TaggedString GetExplanation()
		{
			return base.GetExplanation() + "\n\n" + Alert_ThroneroomInvalidConfiguration.validationInfo;
		}

		// Token: 0x06005693 RID: 22163 RVA: 0x001CB4A0 File Offset: 0x001C96A0
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

		// Token: 0x04002F38 RID: 12088
		private static string validationInfo;
	}
}
