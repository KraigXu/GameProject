using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086C RID: 2156
	public class CompProperties_Explosive : CompProperties
	{
		// Token: 0x0600351E RID: 13598 RVA: 0x00122CA4 File Offset: 0x00120EA4
		public CompProperties_Explosive()
		{
			this.compClass = typeof(CompExplosive);
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x00122D1D File Offset: 0x00120F1D
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			if (this.explosiveDamageType == null)
			{
				this.explosiveDamageType = DamageDefOf.Bomb;
			}
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x00122D39 File Offset: 0x00120F39
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
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

		// Token: 0x04001C4F RID: 7247
		public float explosiveRadius = 1.9f;

		// Token: 0x04001C50 RID: 7248
		public DamageDef explosiveDamageType;

		// Token: 0x04001C51 RID: 7249
		public int damageAmountBase = -1;

		// Token: 0x04001C52 RID: 7250
		public float armorPenetrationBase = -1f;

		// Token: 0x04001C53 RID: 7251
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04001C54 RID: 7252
		public float postExplosionSpawnChance;

		// Token: 0x04001C55 RID: 7253
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04001C56 RID: 7254
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x04001C57 RID: 7255
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x04001C58 RID: 7256
		public float preExplosionSpawnChance;

		// Token: 0x04001C59 RID: 7257
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04001C5A RID: 7258
		public float chanceToStartFire;

		// Token: 0x04001C5B RID: 7259
		public bool damageFalloff;

		// Token: 0x04001C5C RID: 7260
		public bool explodeOnKilled;

		// Token: 0x04001C5D RID: 7261
		public float explosiveExpandPerStackcount;

		// Token: 0x04001C5E RID: 7262
		public float explosiveExpandPerFuel;

		// Token: 0x04001C5F RID: 7263
		public EffecterDef explosionEffect;

		// Token: 0x04001C60 RID: 7264
		public SoundDef explosionSound;

		// Token: 0x04001C61 RID: 7265
		public List<DamageDef> startWickOnDamageTaken;

		// Token: 0x04001C62 RID: 7266
		public float startWickHitPointsPercent = 0.2f;

		// Token: 0x04001C63 RID: 7267
		public IntRange wickTicks = new IntRange(140, 150);

		// Token: 0x04001C64 RID: 7268
		public float wickScale = 1f;

		// Token: 0x04001C65 RID: 7269
		public float chanceNeverExplodeFromDamage;

		// Token: 0x04001C66 RID: 7270
		public float destroyThingOnExplosionSize;

		// Token: 0x04001C67 RID: 7271
		public DamageDef requiredDamageTypeToExplode;

		// Token: 0x04001C68 RID: 7272
		public IntRange? countdownTicks;

		// Token: 0x04001C69 RID: 7273
		public string extraInspectStringKey;
	}
}
