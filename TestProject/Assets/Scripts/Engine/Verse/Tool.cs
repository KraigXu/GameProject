using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	
	public class Tool
	{
		
		
		public string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null)
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		
		
		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where this.capacities.Contains(x.requiredCapacity)
				select x;
			}
		}

		
		
		public IEnumerable<VerbProperties> VerbsProperties
		{
			get
			{
				return from x in this.Maneuvers
				select x.verb;
			}
		}

		
		public float AdjustedBaseMeleeDamageAmount(Thing ownerEquipment, DamageDef damageDef)
		{
			float num = this.power;
			if (ownerEquipment != null)
			{
				num *= ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, true);
				if (ownerEquipment.Stuff != null && damageDef != null)
				{
					num *= ownerEquipment.Stuff.GetStatValueAbstract(damageDef.armorCategory.multStat, null);
				}
			}
			return num;
		}

		
		public float AdjustedBaseMeleeDamageAmount_NewTmp(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff, DamageDef damageDef)
		{
			float num = this.power;
			if (ownerEquipmentStuff != null)
			{
				num *= ownerEquipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_DamageMultiplier, ownerEquipmentStuff);
				if (ownerEquipmentStuff != null && damageDef != null)
				{
					num *= ownerEquipmentStuff.GetStatValueAbstract(damageDef.armorCategory.multStat, null);
				}
			}
			return num;
		}

		
		public float AdjustedCooldown(Thing ownerEquipment)
		{
			return this.cooldownTime * ((ownerEquipment == null) ? 1f : ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true));
		}

		
		public float AdjustedCooldown_NewTmp(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff)
		{
			return this.cooldownTime * ((ownerEquipment == null) ? 1f : ownerEquipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_CooldownMultiplier, ownerEquipmentStuff));
		}

		
		public override string ToString()
		{
			return this.label;
		}

		
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		
		public IEnumerable<string> ConfigErrors()
		{
			if (this.id.NullOrEmpty())
			{
				yield return "tool has null id (power=" + this.power.ToString("0.##") + ")";
			}
			yield break;
		}

		
		[Unsaved(false)]
		public string id;

		
		[MustTranslate]
		public string label;

		
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		
		public bool labelUsedInLogging = true;

		
		public List<ToolCapacityDef> capacities = new List<ToolCapacityDef>();

		
		public float power;

		
		public float armorPenetration = -1f;

		
		public float cooldownTime;

		
		public SurpriseAttackProps surpriseAttack;

		
		public HediffDef hediff;

		
		public float chanceFactor = 1f;

		
		public bool alwaysTreatAsWeapon;

		
		public List<ExtraDamage> extraMeleeDamages;

		
		public SoundDef soundMeleeHit;

		
		public SoundDef soundMeleeMiss;

		
		public BodyPartGroupDef linkedBodyPartsGroup;

		
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		
		[Unsaved(false)]
		private string cachedLabelCap;
	}
}
