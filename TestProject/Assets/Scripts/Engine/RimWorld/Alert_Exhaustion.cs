using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DED RID: 3565
	public class Alert_Exhaustion : Alert
	{
		// Token: 0x06005665 RID: 22117 RVA: 0x001CA4F1 File Offset: 0x001C86F1
		public Alert_Exhaustion()
		{
			this.defaultLabel = "Exhaustion".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x06005666 RID: 22118 RVA: 0x001CA520 File Offset: 0x001C8720
		private List<Pawn> ExhaustedColonists
		{
			get
			{
				this.exhaustedColonistsResult.Clear();
				List<Pawn> allMaps_FreeColonistsSpawned = PawnsFinder.AllMaps_FreeColonistsSpawned;
				for (int i = 0; i < allMaps_FreeColonistsSpawned.Count; i++)
				{
					if (allMaps_FreeColonistsSpawned[i].needs.rest != null && allMaps_FreeColonistsSpawned[i].needs.rest.CurCategory == RestCategory.Exhausted)
					{
						this.exhaustedColonistsResult.Add(allMaps_FreeColonistsSpawned[i]);
					}
				}
				return this.exhaustedColonistsResult;
			}
		}

		// Token: 0x06005667 RID: 22119 RVA: 0x001CA594 File Offset: 0x001C8794
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ExhaustedColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ExhaustionDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06005668 RID: 22120 RVA: 0x001CA61C File Offset: 0x001C881C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ExhaustedColonists);
		}

		// Token: 0x04002F2C RID: 12076
		private List<Pawn> exhaustedColonistsResult = new List<Pawn>();
	}
}
