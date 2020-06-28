using System;

namespace Verse
{
	// Token: 0x020000A8 RID: 168
	public class ExtraDamage
	{
		// Token: 0x06000536 RID: 1334 RVA: 0x0001A50D File Offset: 0x0001870D
		public float AdjustedDamageAmount(Verb verb, Pawn caster)
		{
			return this.amount * verb.verbProps.GetDamageFactorFor(verb, caster);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001A523 File Offset: 0x00018723
		public float AdjustedArmorPenetration(Verb verb, Pawn caster)
		{
			if (this.armorPenetration < 0f)
			{
				return this.AdjustedDamageAmount(verb, caster) * 0.015f;
			}
			return this.armorPenetration;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001A547 File Offset: 0x00018747
		public float AdjustedArmorPenetration()
		{
			if (this.armorPenetration < 0f)
			{
				return this.amount * 0.015f;
			}
			return this.armorPenetration;
		}

		// Token: 0x0400033A RID: 826
		public DamageDef def;

		// Token: 0x0400033B RID: 827
		public float amount;

		// Token: 0x0400033C RID: 828
		public float armorPenetration = -1f;

		// Token: 0x0400033D RID: 829
		public float chance = 1f;
	}
}
