using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001021 RID: 4129
	public class StatWorker_ShootingAccuracy : StatWorker
	{
		// Token: 0x060062E1 RID: 25313 RVA: 0x0022587C File Offset: 0x00223A7C
		public override string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 5; i <= 45; i += 5)
			{
				float f = ShotReport.HitFactorFromShooter(finalVal, (float)i);
				stringBuilder.AppendLine("distance".Translate().CapitalizeFirst() + " " + i.ToString() + ": " + f.ToStringPercent("F1"));
			}
			stringBuilder.AppendLine(base.GetExplanationFinalizePart(req, numberSense, finalVal));
			return stringBuilder.ToString();
		}
	}
}
