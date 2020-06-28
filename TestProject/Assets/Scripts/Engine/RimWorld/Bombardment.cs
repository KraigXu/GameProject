using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C90 RID: 3216
	public class Bombardment : OrbitalStrike
	{
		// Token: 0x06004D79 RID: 19833 RVA: 0x001A0209 File Offset: 0x0019E409
		public override void StartStrike()
		{
			base.StartStrike();
			MoteMaker.MakeBombardmentMote(base.Position, base.Map);
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x001A0222 File Offset: 0x0019E422
		public override void Tick()
		{
			base.Tick();
			if (base.Destroyed)
			{
				return;
			}
			if (Find.TickManager.TicksGame % 18 == 0)
			{
				this.CreateRandomExplosion();
			}
			if (Find.TickManager.TicksGame % 20 == 0)
			{
				this.StartRandomFire();
			}
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x001A0260 File Offset: 0x0019E460
		private void CreateRandomExplosion()
		{
			IntVec3 center = (from x in GenRadial.RadialCellsAround(base.Position, 15f, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => Bombardment.DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position)));
			float num = (float)Rand.Range(6, 8);
			Map map = base.Map;
			float radius = num;
			DamageDef bomb = DamageDefOf.Bomb;
			Thing instigator = this.instigator;
			int damAmount = -1;
			float armorPenetration = -1f;
			SoundDef explosionSound = null;
			ThingDef def = this.def;
			GenExplosion.DoExplosion(center, map, radius, bomb, instigator, damAmount, armorPenetration, explosionSound, this.weaponDef, def, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x001A02F8 File Offset: 0x0019E4F8
		private void StartRandomFire()
		{
			FireUtility.TryStartFireIn((from x in GenRadial.RadialCellsAround(base.Position, 25f, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => Bombardment.DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position))), base.Map, Rand.Range(0.1f, 0.925f));
		}

		// Token: 0x04002B5D RID: 11101
		private const int ImpactAreaRadius = 15;

		// Token: 0x04002B5E RID: 11102
		private const int ExplosionRadiusMin = 6;

		// Token: 0x04002B5F RID: 11103
		private const int ExplosionRadiusMax = 8;

		// Token: 0x04002B60 RID: 11104
		public const int EffectiveRadius = 23;

		// Token: 0x04002B61 RID: 11105
		public const int RandomFireRadius = 25;

		// Token: 0x04002B62 RID: 11106
		private const int BombIntervalTicks = 18;

		// Token: 0x04002B63 RID: 11107
		private const int StartRandomFireEveryTicks = 20;

		// Token: 0x04002B64 RID: 11108
		private static readonly SimpleCurve DistanceChanceFactor = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(15f, 0.1f),
				true
			}
		};
	}
}
