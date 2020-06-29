using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public static class BreakRiskAlertUtility
	{
		
		
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

		
		public static void Clear()
		{
			BreakRiskAlertUtility.pawnsAtRiskExtremeResult.Clear();
			BreakRiskAlertUtility.pawnsAtRiskMajorResult.Clear();
			BreakRiskAlertUtility.pawnsAtRiskMinorResult.Clear();
		}

		
		private static List<Pawn> pawnsAtRiskExtremeResult = new List<Pawn>();

		
		private static List<Pawn> pawnsAtRiskMajorResult = new List<Pawn>();

		
		private static List<Pawn> pawnsAtRiskMinorResult = new List<Pawn>();
	}
}
