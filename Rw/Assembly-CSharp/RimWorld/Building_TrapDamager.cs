using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C5B RID: 3163
	public class Building_TrapDamager : Building_Trap
	{
		// Token: 0x06004B90 RID: 19344 RVA: 0x0019777B File Offset: 0x0019597B
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				SoundDefOf.TrapArm.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x001977A4 File Offset: 0x001959A4
		protected override void SpringSub(Pawn p)
		{
			SoundDefOf.TrapSpring.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			if (p == null)
			{
				return;
			}
			float num = this.GetStatValue(StatDefOf.TrapMeleeDamage, true) * Building_TrapDamager.DamageRandomFactorRange.RandomInRange / Building_TrapDamager.DamageCount;
			float armorPenetration = num * 0.015f;
			int num2 = 0;
			while ((float)num2 < Building_TrapDamager.DamageCount)
			{
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Stab, num, armorPenetration, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
				DamageWorker.DamageResult damageResult = p.TakeDamage(dinfo);
				if (num2 == 0)
				{
					BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(p, RulePackDefOf.DamageEvent_TrapSpike, null);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
					damageResult.AssociateWithLog(battleLogEntry_DamageTaken);
				}
				num2++;
			}
		}

		// Token: 0x04002AB6 RID: 10934
		private static readonly FloatRange DamageRandomFactorRange = new FloatRange(0.8f, 1.2f);

		// Token: 0x04002AB7 RID: 10935
		private static readonly float DamageCount = 5f;
	}
}
