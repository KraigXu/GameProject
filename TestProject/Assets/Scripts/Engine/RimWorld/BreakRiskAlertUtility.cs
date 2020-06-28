using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0C RID: 3596
	public static class BreakRiskAlertUtility
	{
		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x060056D8 RID: 22232 RVA: 0x001CCB64 File Offset: 0x001CAD64
		public static List<Pawn> PawnsAtRiskExtreme
		{
			get
			{
				BreakRiskAlertUtility.pawnsAtRiskExtremeResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (!pawn.Downed && pawn.mindState.mentalBreaker.BreakExtremeIsImminent)
					{
						BreakRiskAlertUtility.pawnsAtRiskExtremeResult.Add(pawn);
					}
				}
				return BreakRiskAlertUtility.pawnsAtRiskExtremeResult;
			}
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x060056D9 RID: 22233 RVA: 0x001CCBE4 File Offset: 0x001CADE4
		public static List<Pawn> PawnsAtRiskMajor
		{
			get
			{
				BreakRiskAlertUtility.pawnsAtRiskMajorResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (!pawn.Downed && pawn.mindState.mentalBreaker.BreakMajorIsImminent)
					{
						BreakRiskAlertUtility.pawnsAtRiskMajorResult.Add(pawn);
					}
				}
				return BreakRiskAlertUtility.pawnsAtRiskMajorResult;
			}
		}

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x060056DA RID: 22234 RVA: 0x001CCC64 File Offset: 0x001CAE64
		public static List<Pawn> PawnsAtRiskMinor
		{
			get
			{
				BreakRiskAlertUtility.pawnsAtRiskMinorResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (!pawn.Downed && pawn.mindState.mentalBreaker.BreakMinorIsImminent)
					{
						BreakRiskAlertUtility.pawnsAtRiskMinorResult.Add(pawn);
					}
				}
				return BreakRiskAlertUtility.pawnsAtRiskMinorResult;
			}
		}

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x060056DB RID: 22235 RVA: 0x001CCCE4 File Offset: 0x001CAEE4
		public static string AlertLabel
		{
			get
			{
				int num = BreakRiskAlertUtility.PawnsAtRiskExtreme.Count<Pawn>();
				string text;
				if (num > 0)
				{
					text = "BreakRiskExtreme".Translate();
				}
				else
				{
					num = BreakRiskAlertUtility.PawnsAtRiskMajor.Count<Pawn>();
					if (num > 0)
					{
						text = "BreakRiskMajor".Translate();
					}
					else
					{
						num = BreakRiskAlertUtility.PawnsAtRiskMinor.Count<Pawn>();
						text = "BreakRiskMinor".Translate();
					}
				}
				if (num > 1)
				{
					text = text + " x" + num.ToStringCached();
				}
				return text;
			}
		}

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x060056DC RID: 22236 RVA: 0x001CCD68 File Offset: 0x001CAF68
		public static string AlertExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (BreakRiskAlertUtility.PawnsAtRiskExtreme.Any<Pawn>())
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (Pawn pawn in BreakRiskAlertUtility.PawnsAtRiskExtreme)
					{
						stringBuilder2.AppendLine("  - " + pawn.NameShortColored.Resolve());
					}
					stringBuilder.Append("BreakRiskExtremeDesc".Translate(stringBuilder2).Resolve());
				}
				if (BreakRiskAlertUtility.PawnsAtRiskMajor.Any<Pawn>())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					StringBuilder stringBuilder3 = new StringBuilder();
					foreach (Pawn pawn2 in BreakRiskAlertUtility.PawnsAtRiskMajor)
					{
						stringBuilder3.AppendLine("  - " + pawn2.NameShortColored.Resolve());
					}
					stringBuilder.Append("BreakRiskMajorDesc".Translate(stringBuilder3).Resolve());
				}
				if (BreakRiskAlertUtility.PawnsAtRiskMinor.Any<Pawn>())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					StringBuilder stringBuilder4 = new StringBuilder();
					foreach (Pawn pawn3 in BreakRiskAlertUtility.PawnsAtRiskMinor)
					{
						stringBuilder4.AppendLine("  - " + pawn3.NameShortColored.Resolve());
					}
					stringBuilder.Append("BreakRiskMinorDesc".Translate(stringBuilder4).Resolve());
				}
				stringBuilder.AppendLine();
				stringBuilder.Append("BreakRiskDescEnding".Translate());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060056DD RID: 22237 RVA: 0x001CCF74 File Offset: 0x001CB174
		public static void Clear()
		{
			BreakRiskAlertUtility.pawnsAtRiskExtremeResult.Clear();
			BreakRiskAlertUtility.pawnsAtRiskMajorResult.Clear();
			BreakRiskAlertUtility.pawnsAtRiskMinorResult.Clear();
		}

		// Token: 0x04002F4C RID: 12108
		private static List<Pawn> pawnsAtRiskExtremeResult = new List<Pawn>();

		// Token: 0x04002F4D RID: 12109
		private static List<Pawn> pawnsAtRiskMajorResult = new List<Pawn>();

		// Token: 0x04002F4E RID: 12110
		private static List<Pawn> pawnsAtRiskMinorResult = new List<Pawn>();
	}
}
