using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD5 RID: 3541
	public class Alert_MajorOrExtremeBreakRisk : Alert_Critical
	{
		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x060055FD RID: 22013 RVA: 0x001C8402 File Offset: 0x001C6602
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

		// Token: 0x060055FE RID: 22014 RVA: 0x001C8435 File Offset: 0x001C6635
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x060055FF RID: 22015 RVA: 0x001C843C File Offset: 0x001C663C
		public override TaggedString GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06005600 RID: 22016 RVA: 0x001C8448 File Offset: 0x001C6648
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Culprits);
		}

		// Token: 0x04002F0F RID: 12047
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
