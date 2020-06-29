using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_Boredom : Alert
	{
		
		public Alert_Boredom()
		{
			this.defaultLabel = "Boredom".Translate();
			this.defaultPriority = AlertPriority.Medium;
		}

		
		
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

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BoredPawns);
		}

		
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

		
		private const float JoyNeedThreshold = 0.24000001f;

		
		private List<Pawn> boredPawnsResult = new List<Pawn>();
	}
}
