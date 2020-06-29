using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Explosive : CompProperties
	{
		
		public CompProperties_Explosive()
		{
			this.compClass = typeof(CompExplosive);
		}

		
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			if (this.explosiveDamageType == null)
			{
				this.explosiveDamageType = DamageDefOf.Bomb;
			}
		}

		
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in this.n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return "CompExplosive requires Normal ticker type";
			}
			yield break;
			yield break;
		}

		
		public float explosiveRadius = 1.9f;

		
		public DamageDef explosiveDamageType;

		
		public int damageAmountBase = -1;

		
		public float armorPenetrationBase = -1f;

		
		public ThingDef postExplosionSpawnThingDef;

		
		public float postExplosionSpawnChance;

		
		public int postExplosionSpawnThingCount = 1;

		
		public bool applyDamageToExplosionCellsNeighbors;

		
		public ThingDef preExplosionSpawnThingDef;

		
		public float preExplosionSpawnChance;

		
		public int preExplosionSpawnThingCount = 1;

		
		public float chanceToStartFire;

		
		public bool damageFalloff;

		
		public bool explodeOnKilled;

		
		public float explosiveExpandPerStackcount;

		
		public float explosiveExpandPerFuel;

		
		public EffecterDef explosionEffect;

		
		public SoundDef explosionSound;

		
		public List<DamageDef> startWickOnDamageTaken;

		
		public float startWickHitPointsPercent = 0.2f;

		
		public IntRange wickTicks = new IntRange(140, 150);

		
		public float wickScale = 1f;

		
		public float chanceNeverExplodeFromDamage;

		
		public float destroyThingOnExplosionSize;

		
		public DamageDef requiredDamageTypeToExplode;

		
		public IntRange? countdownTicks;

		
		public string extraInspectStringKey;
	}
}
