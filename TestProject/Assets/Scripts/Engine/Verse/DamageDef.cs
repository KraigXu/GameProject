using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200007E RID: 126
	public class DamageDef : Def
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0001787B File Offset: 0x00015A7B
		public DamageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (DamageWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000178B0 File Offset: 0x00015AB0
		public bool ExternalViolenceFor(Thing thing)
		{
			if (this.externalViolence)
			{
				return true;
			}
			if (this.externalViolenceForMechanoids)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.RaceProps.IsMechanoid)
				{
					return true;
				}
				if (thing is Building_Turret)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040001DD RID: 477
		public Type workerClass = typeof(DamageWorker);

		// Token: 0x040001DE RID: 478
		private bool externalViolence;

		// Token: 0x040001DF RID: 479
		private bool externalViolenceForMechanoids;

		// Token: 0x040001E0 RID: 480
		public bool hasForcefulImpact = true;

		// Token: 0x040001E1 RID: 481
		public bool harmsHealth = true;

		// Token: 0x040001E2 RID: 482
		public bool makesBlood = true;

		// Token: 0x040001E3 RID: 483
		public bool canInterruptJobs = true;

		// Token: 0x040001E4 RID: 484
		public bool isRanged;

		// Token: 0x040001E5 RID: 485
		public bool makesAnimalsFlee;

		// Token: 0x040001E6 RID: 486
		public bool execution;

		// Token: 0x040001E7 RID: 487
		public RulePackDef combatLogRules;

		// Token: 0x040001E8 RID: 488
		public float buildingDamageFactor = 1f;

		// Token: 0x040001E9 RID: 489
		public float plantDamageFactor = 1f;

		// Token: 0x040001EA RID: 490
		public bool canUseDeflectMetalEffect = true;

		// Token: 0x040001EB RID: 491
		public ImpactSoundTypeDef impactSoundType;

		// Token: 0x040001EC RID: 492
		[MustTranslate]
		public string deathMessage = "{0} has been killed.";

		// Token: 0x040001ED RID: 493
		public int defaultDamage = -1;

		// Token: 0x040001EE RID: 494
		public float defaultArmorPenetration = -1f;

		// Token: 0x040001EF RID: 495
		public float defaultStoppingPower;

		// Token: 0x040001F0 RID: 496
		public List<DamageDefAdditionalHediff> additionalHediffs;

		// Token: 0x040001F1 RID: 497
		public DamageArmorCategoryDef armorCategory;

		// Token: 0x040001F2 RID: 498
		public int minDamageToFragment = 99999;

		// Token: 0x040001F3 RID: 499
		public FloatRange overkillPctToDestroyPart = new FloatRange(0f, 0.7f);

		// Token: 0x040001F4 RID: 500
		public bool harmAllLayersUntilOutside;

		// Token: 0x040001F5 RID: 501
		public HediffDef hediff;

		// Token: 0x040001F6 RID: 502
		public HediffDef hediffSkin;

		// Token: 0x040001F7 RID: 503
		public HediffDef hediffSolid;

		// Token: 0x040001F8 RID: 504
		public bool isExplosive;

		// Token: 0x040001F9 RID: 505
		public float explosionSnowMeltAmount = 1f;

		// Token: 0x040001FA RID: 506
		public bool explosionAffectOutsidePartsOnly = true;

		// Token: 0x040001FB RID: 507
		public ThingDef explosionCellMote;

		// Token: 0x040001FC RID: 508
		public Color explosionColorCenter = Color.white;

		// Token: 0x040001FD RID: 509
		public Color explosionColorEdge = Color.white;

		// Token: 0x040001FE RID: 510
		public ThingDef explosionInteriorMote;

		// Token: 0x040001FF RID: 511
		public float explosionHeatEnergyPerCell;

		// Token: 0x04000200 RID: 512
		public SoundDef soundExplosion;

		// Token: 0x04000201 RID: 513
		public float stabChanceOfForcedInternal;

		// Token: 0x04000202 RID: 514
		public float stabPierceBonus;

		// Token: 0x04000203 RID: 515
		public SimpleCurve cutExtraTargetsCurve;

		// Token: 0x04000204 RID: 516
		public float cutCleaveBonus;

		// Token: 0x04000205 RID: 517
		public float bluntInnerHitChance;

		// Token: 0x04000206 RID: 518
		public FloatRange bluntInnerHitDamageFractionToConvert;

		// Token: 0x04000207 RID: 519
		public FloatRange bluntInnerHitDamageFractionToAdd;

		// Token: 0x04000208 RID: 520
		public float bluntStunDuration = 1f;

		// Token: 0x04000209 RID: 521
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToHeadCurve;

		// Token: 0x0400020A RID: 522
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToBodyCurve;

		// Token: 0x0400020B RID: 523
		public float scratchSplitPercentage = 0.5f;

		// Token: 0x0400020C RID: 524
		public float biteDamageMultiplier = 1f;

		// Token: 0x0400020D RID: 525
		[Unsaved(false)]
		private DamageWorker workerInt;
	}
}
