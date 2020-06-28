using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE9 RID: 2793
	public class DeathActionWorker_SmallExplosion : DeathActionWorker
	{
		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06004203 RID: 16899 RVA: 0x00160BAA File Offset: 0x0015EDAA
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06004204 RID: 16900 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x00160C44 File Offset: 0x0015EE44
		public override void PawnDied(Corpse corpse)
		{
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, 1.9f, DamageDefOf.Flame, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}
	}
}
