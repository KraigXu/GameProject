using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000326 RID: 806
	public class Tool
	{
		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001793 RID: 6035 RVA: 0x00085C3B File Offset: 0x00083E3B
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

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001794 RID: 6036 RVA: 0x00085C5C File Offset: 0x00083E5C
		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where this.capacities.Contains(x.requiredCapacity)
				select x;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001795 RID: 6037 RVA: 0x00085C74 File Offset: 0x00083E74
		public IEnumerable<VerbProperties> VerbsProperties
		{
			get
			{
				return from x in this.Maneuvers
				select x.verb;
			}
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x00085CA0 File Offset: 0x00083EA0
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

		// Token: 0x06001797 RID: 6039 RVA: 0x00085CEC File Offset: 0x00083EEC
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

		// Token: 0x06001798 RID: 6040 RVA: 0x00085D2E File Offset: 0x00083F2E
		public float AdjustedCooldown(Thing ownerEquipment)
		{
			return this.cooldownTime * ((ownerEquipment == null) ? 1f : ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true));
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x00085D4D File Offset: 0x00083F4D
		public float AdjustedCooldown_NewTmp(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff)
		{
			return this.cooldownTime * ((ownerEquipment == null) ? 1f : ownerEquipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_CooldownMultiplier, ownerEquipmentStuff));
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x00085D6C File Offset: 0x00083F6C
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x00085D74 File Offset: 0x00083F74
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x00085D82 File Offset: 0x00083F82
		public IEnumerable<string> ConfigErrors()
		{
			if (this.id.NullOrEmpty())
			{
				yield return "tool has null id (power=" + this.power.ToString("0.##") + ")";
			}
			yield break;
		}

		// Token: 0x04000EAC RID: 3756
		[Unsaved(false)]
		public string id;

		// Token: 0x04000EAD RID: 3757
		[MustTranslate]
		public string label;

		// Token: 0x04000EAE RID: 3758
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x04000EAF RID: 3759
		public bool labelUsedInLogging = true;

		// Token: 0x04000EB0 RID: 3760
		public List<ToolCapacityDef> capacities = new List<ToolCapacityDef>();

		// Token: 0x04000EB1 RID: 3761
		public float power;

		// Token: 0x04000EB2 RID: 3762
		public float armorPenetration = -1f;

		// Token: 0x04000EB3 RID: 3763
		public float cooldownTime;

		// Token: 0x04000EB4 RID: 3764
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x04000EB5 RID: 3765
		public HediffDef hediff;

		// Token: 0x04000EB6 RID: 3766
		public float chanceFactor = 1f;

		// Token: 0x04000EB7 RID: 3767
		public bool alwaysTreatAsWeapon;

		// Token: 0x04000EB8 RID: 3768
		public List<ExtraDamage> extraMeleeDamages;

		// Token: 0x04000EB9 RID: 3769
		public SoundDef soundMeleeHit;

		// Token: 0x04000EBA RID: 3770
		public SoundDef soundMeleeMiss;

		// Token: 0x04000EBB RID: 3771
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x04000EBC RID: 3772
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x04000EBD RID: 3773
		[Unsaved(false)]
		private string cachedLabelCap;
	}
}
