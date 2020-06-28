using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000262 RID: 610
	public class HediffComp_Invisibility : HediffComp
	{
		// Token: 0x06001095 RID: 4245 RVA: 0x0005EAD1 File Offset: 0x0005CCD1
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.UpdateTarget();
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0005EAE0 File Offset: 0x0005CCE0
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			this.UpdateTarget();
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0005EAF0 File Offset: 0x0005CCF0
		private void UpdateTarget()
		{
			Pawn pawn = this.parent.pawn;
			if (pawn.Spawned)
			{
				pawn.Map.attackTargetsCache.UpdateTarget(pawn);
			}
			PortraitsCache.SetDirty(pawn);
		}
	}
}
