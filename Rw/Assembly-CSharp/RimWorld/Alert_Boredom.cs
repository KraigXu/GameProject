using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DFB RID: 3579
	public class Alert_Boredom : Alert
	{
		// Token: 0x0600569D RID: 22173 RVA: 0x001CB78F File Offset: 0x001C998F
		public Alert_Boredom()
		{
			this.defaultLabel = "Boredom".Translate();
			this.defaultPriority = AlertPriority.Medium;
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x0600569E RID: 22174 RVA: 0x001CB7C0 File Offset: 0x001C99C0
		private List<Pawn> BoredPawns
		{
			get
			{
				this.boredPawnsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if ((pawn.needs.joy.CurLevelPercentage < 0.24000001f || pawn.GetTimeAssignment() == TimeAssignmentDefOf.Joy) && pawn.needs.joy.tolerances.BoredOfAllAvailableJoyKinds(pawn))
					{
						this.boredPawnsResult.Add(pawn);
					}
				}
				return this.boredPawnsResult;
			}
		}

		// Token: 0x0600569F RID: 22175 RVA: 0x001CB864 File Offset: 0x001C9A64
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BoredPawns);
		}

		// Token: 0x060056A0 RID: 22176 RVA: 0x001CB874 File Offset: 0x001C9A74
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pawn pawn = null;
			foreach (Pawn pawn2 in this.BoredPawns)
			{
				stringBuilder.AppendLine("   " + pawn2.Label);
				if (pawn == null)
				{
					pawn = pawn2;
				}
			}
			string value = (pawn != null) ? JoyUtility.JoyKindsOnMapString(pawn.Map) : "";
			return "BoredomDesc".Translate(stringBuilder.ToString().TrimEndNewlines(), pawn.LabelShort, value, pawn.Named("PAWN"));
		}

		// Token: 0x04002F39 RID: 12089
		private const float JoyNeedThreshold = 0.24000001f;

		// Token: 0x04002F3A RID: 12090
		private List<Pawn> boredPawnsResult = new List<Pawn>();
	}
}
