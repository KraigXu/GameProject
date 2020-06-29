using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_TitleRequiresBedroom : Alert
	{
		
		public Alert_TitleRequiresBedroom()
		{
			this.defaultLabel = "NeedBedroomAssigned".Translate();
			this.defaultExplanation = "NeedBedroomAssignedDesc".Translate();
		}

		
		// (get) Token: 0x0600568B RID: 22155 RVA: 0x001CB1D0 File Offset: 0x001C93D0
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
						if (pawn.royalty != null && pawn.royalty.CanRequireBedroom() && pawn.royalty.HighestTitleWithBedroomRequirements() != null && !pawn.royalty.HasPersonalBedroom())
						{
							this.targetsResult.Add(pawn);
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
