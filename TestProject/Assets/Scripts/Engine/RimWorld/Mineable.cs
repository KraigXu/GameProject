using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C54 RID: 3156
	public class Mineable : Building
	{
		// Token: 0x06004B58 RID: 19288 RVA: 0x001968AC File Offset: 0x00194AAC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.yieldPct, "yieldPct", 0f, false);
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x001968CC File Offset: 0x00194ACC
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (this.def.building.mineableThing != null && this.def.building.mineableYieldWasteable && dinfo.Def == DamageDefOf.Mining && dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.Notify_TookMiningDamage(GenMath.RoundRandom(dinfo.Amount), (Pawn)dinfo.Instigator);
			}
			absorbed = false;
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x0019694C File Offset: 0x00194B4C
		public void DestroyMined(Pawn pawn)
		{
			Map map = base.Map;
			base.Destroy(DestroyMode.KillFinalize);
			this.TrySpawnYield(map, this.yieldPct, true, pawn);
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x00196978 File Offset: 0x00194B78
		public override void Destroy(DestroyMode mode)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				this.TrySpawnYield(map, 0.2f, false, null);
			}
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x001969A8 File Offset: 0x00194BA8
		private void TrySpawnYield(Map map, float yieldChance, bool moteOnWaste, Pawn pawn)
		{
			if (this.def.building.mineableThing == null)
			{
				return;
			}
			if (Rand.Value > this.def.building.mineableDropChance)
			{
				return;
			}
			int num = Mathf.Max(1, this.def.building.mineableYield);
			if (this.def.building.mineableYieldWasteable)
			{
				num = Mathf.Max(1, GenMath.RoundRandom((float)num * this.yieldPct));
			}
			Thing thing = ThingMaker.MakeThing(this.def.building.mineableThing, null);
			thing.stackCount = num;
			GenSpawn.Spawn(thing, base.Position, map, WipeMode.Vanish);
			if ((pawn == null || !pawn.IsColonist) && thing.def.EverHaulable && !thing.def.designateHaulable)
			{
				thing.SetForbidden(true, true);
			}
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x00196A7C File Offset: 0x00194C7C
		public void Notify_TookMiningDamage(int amount, Pawn miner)
		{
			float num = (float)Mathf.Min(amount, this.HitPoints) / (float)base.MaxHitPoints;
			this.yieldPct += num * miner.GetStatValue(StatDefOf.MiningYield, true);
		}

		// Token: 0x04002A8E RID: 10894
		private float yieldPct;

		// Token: 0x04002A8F RID: 10895
		private const float YieldChanceOnNonMiningKill = 0.2f;
	}
}
