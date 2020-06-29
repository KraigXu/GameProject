﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	
	public class DamageDef : Def
	{
		
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

		
		public Type workerClass = typeof(DamageWorker);

		
		private bool externalViolence;

		
		private bool externalViolenceForMechanoids;

		
		public bool hasForcefulImpact = true;

		
		public bool harmsHealth = true;

		
		public bool makesBlood = true;

		
		public bool canInterruptJobs = true;

		
		public bool isRanged;

		
		public bool makesAnimalsFlee;

		
		public bool execution;

		
		public RulePackDef combatLogRules;

		
		public float buildingDamageFactor = 1f;

		
		public float plantDamageFactor = 1f;

		
		public bool canUseDeflectMetalEffect = true;

		
		public ImpactSoundTypeDef impactSoundType;

		
		[MustTranslate]
		public string deathMessage = "{0} has been killed.";

		
		public int defaultDamage = -1;

		
		public float defaultArmorPenetration = -1f;

		
		public float defaultStoppingPower;

		
		public List<DamageDefAdditionalHediff> additionalHediffs;

		
		public DamageArmorCategoryDef armorCategory;

		
		public int minDamageToFragment = 99999;

		
		public FloatRange overkillPctToDestroyPart = new FloatRange(0f, 0.7f);

		
		public bool harmAllLayersUntilOutside;

		
		public HediffDef hediff;

		
		public HediffDef hediffSkin;

		
		public HediffDef hediffSolid;

		
		public bool isExplosive;

		
		public float explosionSnowMeltAmount = 1f;

		
		public bool explosionAffectOutsidePartsOnly = true;

		
		public ThingDef explosionCellMote;

		
		public Color explosionColorCenter = Color.white;

		
		public Color explosionColorEdge = Color.white;

		
		public ThingDef explosionInteriorMote;

		
		public float explosionHeatEnergyPerCell;

		
		public SoundDef soundExplosion;

		
		public float stabChanceOfForcedInternal;

		
		public float stabPierceBonus;

		
		public SimpleCurve cutExtraTargetsCurve;

		
		public float cutCleaveBonus;

		
		public float bluntInnerHitChance;

		
		public FloatRange bluntInnerHitDamageFractionToConvert;

		
		public FloatRange bluntInnerHitDamageFractionToAdd;

		
		public float bluntStunDuration = 1f;

		
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToHeadCurve;

		
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToBodyCurve;

		
		public float scratchSplitPercentage = 0.5f;

		
		public float biteDamageMultiplier = 1f;

		
		[Unsaved(false)]
		private DamageWorker workerInt;
	}
}
