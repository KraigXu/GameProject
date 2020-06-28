using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DEC RID: 3564
	public class Alert_StarvationColonists : Alert
	{
		// Token: 0x06005661 RID: 22113 RVA: 0x001CA3A5 File Offset: 0x001C85A5
		public Alert_StarvationColonists()
		{
			this.defaultLabel = "Starvation".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x06005662 RID: 22114 RVA: 0x001CA3D4 File Offset: 0x001C85D4
		private List<Pawn> StarvingColonists
		{
			get
			{
				this.starvingColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (pawn.needs.food != null && pawn.needs.food.Starving)
					{
						this.starvingColonistsResult.Add(pawn);
					}
				}
				return this.starvingColonistsResult;
			}
		}

		// Token: 0x06005663 RID: 22115 RVA: 0x001CA45C File Offset: 0x001C865C
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.StarvingColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "StarvationDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06005664 RID: 22116 RVA: 0x001CA4E4 File Offset: 0x001C86E4
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingColonists);
		}

		// Token: 0x04002F2B RID: 12075
		private List<Pawn> starvingColonistsResult = new List<Pawn>();
	}
}
