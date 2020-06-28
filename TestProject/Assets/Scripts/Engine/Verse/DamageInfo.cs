using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200040A RID: 1034
	public struct DamageInfo
	{
		// Token: 0x170005B1 RID: 1457
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

		// Token: 0x170005B2 RID: 1458
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

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x000BE4E1 File Offset: 0x000BC6E1
		public float ArmorPenetrationInt
		{
			get
			{
				return this.armorPenetrationInt;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x000BE4E9 File Offset: 0x000BC6E9
		public Thing Instigator
		{
			get
			{
				return this.instigatorInt;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001E8C RID: 7820 RVA: 0x000BE4F1 File Offset: 0x000BC6F1
		public DamageInfo.SourceCategory Category
		{
			get
			{
				return this.categoryInt;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x000BE4F9 File Offset: 0x000BC6F9
		public Thing IntendedTarget
		{
			get
			{
				return this.intendedTargetInt;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x000BE501 File Offset: 0x000BC701
		public float Angle
		{
			get
			{
				return this.angleInt;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x000BE509 File Offset: 0x000BC709
		public BodyPartRecord HitPart
		{
			get
			{
				return this.hitPartInt;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001E90 RID: 7824 RVA: 0x000BE511 File Offset: 0x000BC711
		public BodyPartHeight Height
		{
			get
			{
				return this.heightInt;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x000BE519 File Offset: 0x000BC719
		public BodyPartDepth Depth
		{
			get
			{
				return this.depthInt;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001E92 RID: 7826 RVA: 0x000BE521 File Offset: 0x000BC721
		public ThingDef Weapon
		{
			get
			{
				return this.weaponInt;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x000BE529 File Offset: 0x000BC729
		public BodyPartGroupDef WeaponBodyPartGroup
		{
			get
			{
				return this.weaponBodyPartGroupInt;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001E94 RID: 7828 RVA: 0x000BE531 File Offset: 0x000BC731
		public HediffDef WeaponLinkedHediff
		{
			get
			{
				return this.weaponHediffInt;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001E95 RID: 7829 RVA: 0x000BE539 File Offset: 0x000BC739
		public bool InstantPermanentInjury
		{
			get
			{
				return this.instantPermanentInjuryInt;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001E96 RID: 7830 RVA: 0x000BE541 File Offset: 0x000BC741
		public bool AllowDamagePropagation
		{
			get
			{
				return !this.InstantPermanentInjury && this.allowDamagePropagationInt;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001E97 RID: 7831 RVA: 0x000BE553 File Offset: 0x000BC753
		public bool IgnoreArmor
		{
			get
			{
				return this.ignoreArmorInt;
			}
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x000BE55C File Offset: 0x000BC75C
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

		// Token: 0x06001E99 RID: 7833 RVA: 0x000BE5FC File Offset: 0x000BC7FC
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

		// Token: 0x06001E9A RID: 7834 RVA: 0x000BE6C9 File Offset: 0x000BC8C9
		public void SetAmount(float newAmount)
		{
			this.amountInt = newAmount;
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x000BE6D2 File Offset: 0x000BC8D2
		public void SetIgnoreArmor(bool ignoreArmor)
		{
			this.ignoreArmorInt = ignoreArmor;
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x000BE6DB File Offset: 0x000BC8DB
		public void SetBodyRegion(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			this.heightInt = height;
			this.depthInt = depth;
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x000BE6EB File Offset: 0x000BC8EB
		public void SetHitPart(BodyPartRecord forceHitPart)
		{
			this.hitPartInt = forceHitPart;
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x000BE6F4 File Offset: 0x000BC8F4
		public void SetInstantPermanentInjury(bool val)
		{
			this.instantPermanentInjuryInt = val;
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x000BE6FD File Offset: 0x000BC8FD
		public void SetWeaponBodyPartGroup(BodyPartGroupDef gr)
		{
			this.weaponBodyPartGroupInt = gr;
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x000BE706 File Offset: 0x000BC906
		public void SetWeaponHediff(HediffDef hd)
		{
			this.weaponHediffInt = hd;
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000BE70F File Offset: 0x000BC90F
		public void SetAllowDamagePropagation(bool val)
		{
			this.allowDamagePropagationInt = val;
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000BE718 File Offset: 0x000BC918
		public void SetAngle(Vector3 vec)
		{
			if (vec.x != 0f || vec.z != 0f)
			{
				this.angleInt = Quaternion.LookRotation(vec).eulerAngles.y;
				return;
			}
			this.angleInt = (float)Rand.RangeInclusive(0, 359);
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x000BE76C File Offset: 0x000BC96C
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

		// Token: 0x040012D6 RID: 4822
		private DamageDef defInt;

		// Token: 0x040012D7 RID: 4823
		private float amountInt;

		// Token: 0x040012D8 RID: 4824
		private float armorPenetrationInt;

		// Token: 0x040012D9 RID: 4825
		private float angleInt;

		// Token: 0x040012DA RID: 4826
		private Thing instigatorInt;

		// Token: 0x040012DB RID: 4827
		private DamageInfo.SourceCategory categoryInt;

		// Token: 0x040012DC RID: 4828
		public Thing intendedTargetInt;

		// Token: 0x040012DD RID: 4829
		private bool ignoreArmorInt;

		// Token: 0x040012DE RID: 4830
		private BodyPartRecord hitPartInt;

		// Token: 0x040012DF RID: 4831
		private BodyPartHeight heightInt;

		// Token: 0x040012E0 RID: 4832
		private BodyPartDepth depthInt;

		// Token: 0x040012E1 RID: 4833
		private ThingDef weaponInt;

		// Token: 0x040012E2 RID: 4834
		private BodyPartGroupDef weaponBodyPartGroupInt;

		// Token: 0x040012E3 RID: 4835
		private HediffDef weaponHediffInt;

		// Token: 0x040012E4 RID: 4836
		private bool instantPermanentInjuryInt;

		// Token: 0x040012E5 RID: 4837
		private bool allowDamagePropagationInt;

		// Token: 0x02001663 RID: 5731
		public enum SourceCategory
		{
			// Token: 0x040055D8 RID: 21976
			ThingOrUnknown,
			// Token: 0x040055D9 RID: 21977
			Collapse
		}
	}
}
