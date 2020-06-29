using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_RoyalNoThroneAssigned : Alert
	{
		
		public Alert_RoyalNoThroneAssigned()
		{
			this.defaultLabel = "NeedThroneAssigned".Translate();
			this.defaultExplanation = "NeedThroneAssignedDesc".Translate();
		}

		
		
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonists)
					{
						if (pawn.royalty != null && pawn.royalty.CanRequireThroneroom())
						{
							bool flag = false;
							List<RoyalTitle> allTitlesForReading = pawn.royalty.AllTitlesForReading;
							for (int j = 0; j < allTitlesForReading.Count; j++)
							{
								if (!allTitlesForReading[j].def.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
								{
									flag = true;
									break;
								}
							}
							if (flag && pawn.ownership.AssignedThrone == null)
							{
								this.targetsResult.Add(pawn);
							}
						}
					}
				}
				return this.targetsResult;
			}
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
