using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class Bombardment : OrbitalStrike
	{
		
		public override void StartStrike()
		{
			base.StartStrike();
			MoteMaker.MakeBombardmentMote(base.Position, base.Map);
		}

		
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

		
		private void StartRandomFire()
		{
			FireUtility.TryStartFireIn((from x in GenRadial.RadialCellsAround(base.Position, 25f, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => Bombardment.DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position))), base.Map, Rand.Range(0.1f, 0.925f));
		}

		
		private const int ImpactAreaRadius = 15;

		
		private const int ExplosionRadiusMin = 6;

		
		private const int ExplosionRadiusMax = 8;

		
		public const int EffectiveRadius = 23;

		
		public const int RandomFireRadius = 25;

		
		private const int BombIntervalTicks = 18;

		
		private const int StartRandomFireEveryTicks = 20;

		
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
