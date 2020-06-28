using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE8 RID: 2792
	public class DeathActionWorker_BigExplosion : DeathActionWorker
	{
		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x060041FF RID: 16895 RVA: 0x00160BAA File Offset: 0x0015EDAA
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06004200 RID: 16896 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004201 RID: 16897 RVA: 0x00160BB4 File Offset: 0x0015EDB4
		public override void PawnDied(Corpse corpse)
		{
			float radius;
			if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0)
			{
				radius = 1.9f;
			}
			else if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 1)
			{
				radius = 2.9f;
			}
			else
			{
				radius = 4.9f;
			}
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, DamageDefOf.Flame, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}
	}
}
