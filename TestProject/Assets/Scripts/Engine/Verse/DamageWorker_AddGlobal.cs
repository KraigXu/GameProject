using System;

namespace Verse
{
	// Token: 0x0200022D RID: 557
	public class DamageWorker_AddGlobal : DamageWorker
	{
		// Token: 0x06000F78 RID: 3960 RVA: 0x00059930 File Offset: 0x00057B30
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				Hediff hediff = HediffMaker.MakeHediff(dinfo.Def.hediff, pawn, null);
				hediff.Severity = dinfo.Amount;
				pawn.health.AddHediff(hediff, null, new DamageInfo?(dinfo), null);
			}
			return new DamageWorker.DamageResult();
		}
	}
}
