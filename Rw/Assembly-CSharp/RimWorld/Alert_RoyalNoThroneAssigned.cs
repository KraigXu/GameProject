using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF3 RID: 3571
	public class Alert_RoyalNoThroneAssigned : Alert
	{
		// Token: 0x0600567F RID: 22143 RVA: 0x001CADAC File Offset: 0x001C8FAC
		public Alert_RoyalNoThroneAssigned()
		{
			this.defaultLabel = "NeedThroneAssigned".Translate();
			this.defaultExplanation = "NeedThroneAssignedDesc".Translate();
		}

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x06005680 RID: 22144 RVA: 0x001CADEC File Offset: 0x001C8FEC
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

		// Token: 0x06005681 RID: 22145 RVA: 0x001CAEEC File Offset: 0x001C90EC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04002F33 RID: 12083
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
