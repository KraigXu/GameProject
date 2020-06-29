using System;
using Verse;

namespace RimWorld
{
	
	public class DeathActionWorker_SmallExplosion : DeathActionWorker
	{
		
		// (get) Token: 0x06004203 RID: 16899 RVA: 0x00160BAA File Offset: 0x0015EDAA
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		
		// (get) Token: 0x06004204 RID: 16900 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		
		public override void PawnDied(Corpse corpse)
		{
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, 1.9f, DamageDefOf.Flame, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}
	}
}
