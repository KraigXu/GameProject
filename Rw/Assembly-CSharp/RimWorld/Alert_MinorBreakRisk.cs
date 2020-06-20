using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDF RID: 3551
	public class Alert_MinorBreakRisk : Alert
	{
		// Token: 0x06005629 RID: 22057 RVA: 0x001C8E4B File Offset: 0x001C704B
		public Alert_MinorBreakRisk()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x001C8435 File Offset: 0x001C6635
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x001C843C File Offset: 0x001C663C
		public override TaggedString GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x001C8E5A File Offset: 0x001C705A
		public override AlertReport GetReport()
		{
			if (BreakRiskAlertUtility.PawnsAtRiskExtreme.Any<Pawn>() || BreakRiskAlertUtility.PawnsAtRiskMajor.Any<Pawn>())
			{
				return false;
			}
			return AlertReport.CulpritsAre(BreakRiskAlertUtility.PawnsAtRiskMinor);
		}
	}
}
