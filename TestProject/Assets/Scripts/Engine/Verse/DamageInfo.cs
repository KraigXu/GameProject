using System;
using UnityEngine;

namespace Verse
{
	
	public struct DamageInfo
	{
		
		// (get) Token: 0x06001E87 RID: 7815 RVA: 0x000BE4BB File Offset: 0x000BC6BB
		// (set) Token: 0x06001E88 RID: 7816 RVA: 0x000BE4C3 File Offset: 0x000BC6C3
		public DamageDef Def
		{
			get
			{
				return this.defInt;
			}
			set
			{
				this.defInt = value;
			}
		}

		
		// (get) Token: 0x06001E89 RID: 7817 RVA: 0x000BE4CC File Offset: 0x000BC6CC
		public float Amount
		{
			get
			{
				if (!DebugSettings.enableDamage)
				{
					return 0f;
				}
				return this.amountInt;
			}
		}

		
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x000BE4E1 File Offset: 0x000BC6E1
		public float ArmorPenetrationInt
		{
			get
			{
				return this.armorPenetrationInt;
			}
		}

		
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x000BE4E9 File Offset: 0x000BC6E9
		public Thing Instigator
		{
			get
			{
				return this.instigatorInt;
			}
		}

		
		// (get) Token: 0x06001E8C RID: 7820 RVA: 0x000BE4F1 File Offset: 0x000BC6F1
		public DamageInfo.SourceCategory Category
		{
			get
			{
				return this.categoryInt;
			}
		}

		
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x000BE4F9 File Offset: 0x000BC6F9
		public Thing IntendedTarget
		{
			get
			{
				return this.intendedTargetInt;
			}
		}

		
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x000BE501 File Offset: 0x000BC701
		public float Angle
		{
			get
			{
				return this.angleInt;
			}
		}

		
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x000BE509 File Offset: 0x000BC709
		public BodyPartRecord HitPart
		{
			get
			{
				return this.hitPartInt;
			}
		}

		
		// (get) Token: 0x06001E90 RID: 7824 RVA: 0x000BE511 File Offset: 0x000BC711
		public BodyPartHeight Height
		{
			get
			{
				return this.heightInt;
			}
		}

		
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x000BE519 File Offset: 0x000BC719
		public BodyPartDepth Depth
		{
			get
			{
				return this.depthInt;
			}
		}

		
		// (get) Token: 0x06001E92 RID: 7826 RVA: 0x000BE521 File Offset: 0x000BC721
		public ThingDef Weapon
		{
			get
			{
				return this.weaponInt;
			}
		}

		
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x000BE529 File Offset: 0x000BC729
		public BodyPartGroupDef WeaponBodyPartGroup
		{
			get
			{
				return this.weaponBodyPartGroupInt;
			}
		}

		
		// (get) Token: 0x06001E94 RID: 7828 RVA: 0x000BE531 File Offset: 0x000BC731
		public HediffDef WeaponLinkedHediff
		{
			get
			{
				return this.weaponHediffInt;
			}
		}

		
		// (get) Token: 0x06001E95 RID: 7829 RVA: 0x000BE539 File Offset: 0x000BC739
		public bool InstantPermanentInjury
		{
			get
			{
				return this.instantPermanentInjuryInt;
			}
		}

		
		// (get) Token: 0x06001E96 RID: 7830 RVA: 0x000BE541 File Offset: 0x000BC741
		public bool AllowDamagePropagation
		{
			get
			{
				return !this.InstantPermanentInjury && this.allowDamagePropagationInt;
			}
		}

		
		// (get) Token: 0x06001E97 RID: 7831 RVA: 0x000BE553 File Offset: 0x000BC753
		public bool IgnoreArmor
		{
			get
			{
				return this.ignoreArmorInt;
			}
		}

		
		public DamageInfo(DamageDef def, float amount, float armorPenetration = 0f, float angle = -1f, Thing instigator = null, BodyPartRecord hitPart = null, ThingDef weapon = null, DamageInfo.SourceCategory category = DamageInfo.SourceCategory.ThingOrUnknown, Thing intendedTarget = null)
		{
			this.defInt = def;
			this.amountInt = amount;
			this.armorPenetrationInt = armorPenetration;
			if (angle < 0f)
			{
				this.angleInt = (float)Rand.RangeInclusive(0, 359);
			}
			else
			{
				this.angleInt = angle;
			}
			this.instigatorInt = instigator;
			this.categoryInt = category;
			this.hitPartInt = hitPart;
			this.heightInt = BodyPartHeight.Undefined;
			this.depthInt = BodyPartDepth.Undefined;
			this.weaponInt = weapon;
			this.weaponBodyPartGroupInt = null;
			this.weaponHediffInt = null;
			this.instantPermanentInjuryInt = false;
			this.allowDamagePropagationInt = true;
			this.ignoreArmorInt = false;
			this.intendedTargetInt = intendedTarget;
		}

		
		public DamageInfo(DamageInfo cloneSource)
		{
			this.defInt = cloneSource.defInt;
			this.amountInt = cloneSource.amountInt;
			this.armorPenetrationInt = cloneSource.armorPenetrationInt;
			this.angleInt = cloneSource.angleInt;
			this.instigatorInt = cloneSource.instigatorInt;
			this.categoryInt = cloneSource.categoryInt;
			this.hitPartInt = cloneSource.hitPartInt;
			this.heightInt = cloneSource.heightInt;
			this.depthInt = cloneSource.depthInt;
			this.weaponInt = cloneSource.weaponInt;
			this.weaponBodyPartGroupInt = cloneSource.weaponBodyPartGroupInt;
			this.weaponHediffInt = cloneSource.weaponHediffInt;
			this.instantPermanentInjuryInt = cloneSource.instantPermanentInjuryInt;
			this.allowDamagePropagationInt = cloneSource.allowDamagePropagationInt;
			this.intendedTargetInt = cloneSource.intendedTargetInt;
			this.ignoreArmorInt = cloneSource.ignoreArmorInt;
		}

		
		public void SetAmount(float newAmount)
		{
			this.amountInt = newAmount;
		}

		
		public void SetIgnoreArmor(bool ignoreArmor)
		{
			this.ignoreArmorInt = ignoreArmor;
		}

		
		public void SetBodyRegion(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			this.heightInt = height;
			this.depthInt = depth;
		}

		
		public void SetHitPart(BodyPartRecord forceHitPart)
		{
			this.hitPartInt = forceHitPart;
		}

		
		public void SetInstantPermanentInjury(bool val)
		{
			this.instantPermanentInjuryInt = val;
		}

		
		public void SetWeaponBodyPartGroup(BodyPartGroupDef gr)
		{
			this.weaponBodyPartGroupInt = gr;
		}

		
		public void SetWeaponHediff(HediffDef hd)
		{
			this.weaponHediffInt = hd;
		}

		
		public void SetAllowDamagePropagation(bool val)
		{
			this.allowDamagePropagationInt = val;
		}

		
		public void SetAngle(Vector3 vec)
		{
			if (vec.x != 0f || vec.z != 0f)
			{
				this.angleInt = Quaternion.LookRotation(vec).eulerAngles.y;
				return;
			}
			this.angleInt = (float)Rand.RangeInclusive(0, 359);
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(def=",
				this.defInt,
				", amount= ",
				this.amountInt,
				", instigator=",
				(this.instigatorInt != null) ? this.instigatorInt.ToString() : this.categoryInt.ToString(),
				", angle=",
				this.angleInt.ToString("F1"),
				")"
			});
		}

		
		private DamageDef defInt;

		
		private float amountInt;

		
		private float armorPenetrationInt;

		
		private float angleInt;

		
		private Thing instigatorInt;

		
		private DamageInfo.SourceCategory categoryInt;

		
		public Thing intendedTargetInt;

		
		private bool ignoreArmorInt;

		
		private BodyPartRecord hitPartInt;

		
		private BodyPartHeight heightInt;

		
		private BodyPartDepth depthInt;

		
		private ThingDef weaponInt;

		
		private BodyPartGroupDef weaponBodyPartGroupInt;

		
		private HediffDef weaponHediffInt;

		
		private bool instantPermanentInjuryInt;

		
		private bool allowDamagePropagationInt;

		
		public enum SourceCategory
		{
			
			ThingOrUnknown,
			
			Collapse
		}
	}
}
