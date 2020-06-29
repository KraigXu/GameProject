using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_MajorOrExtremeBreakRisk : Alert_Critical
	{
		
		
		private List<Pawn> Culprits
		{
			get
			{
				this.culpritsResult.Clear();
				this.culpritsResult.AddRange(BreakRiskAlertUtility.PawnsAtRiskExtreme);
				this.culpritsResult.AddRange(BreakRiskAlertUtility.PawnsAtRiskMajor);
				return this.culpritsResult;
			}
		}

		
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		
		public override TaggedString GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Culprits);
		}

		
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
