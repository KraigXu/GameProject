using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDB RID: 4059
	public struct RewardsGeneratorParams
	{
		// Token: 0x06006187 RID: 24967 RVA: 0x0021DD28 File Offset: 0x0021BF28
		public string ConfigError()
		{
			if (this.rewardValue <= 0f)
			{
				return "rewardValue is " + this.rewardValue;
			}
			if (this.thingRewardDisallowed && this.thingRewardRequired)
			{
				return "thing reward is both disallowed and required";
			}
			if (this.thingRewardDisallowed && !this.allowRoyalFavor && !this.allowGoodwill)
			{
				return "no reward types are allowed";
			}
			return null;
		}

		// Token: 0x06006188 RID: 24968 RVA: 0x0021DD8D File Offset: 0x0021BF8D
		public override string ToString()
		{
			return GenText.FieldsToString<RewardsGeneratorParams>(this);
		}

		// Token: 0x04003B59 RID: 15193
		public float rewardValue;

		// Token: 0x04003B5A RID: 15194
		public Faction giverFaction;

		// Token: 0x04003B5B RID: 15195
		public string chosenPawnSignal;

		// Token: 0x04003B5C RID: 15196
		public bool giveToCaravan;

		// Token: 0x04003B5D RID: 15197
		public float minGeneratedRewardValue;

		// Token: 0x04003B5E RID: 15198
		public bool thingRewardDisallowed;

		// Token: 0x04003B5F RID: 15199
		public bool thingRewardRequired;

		// Token: 0x04003B60 RID: 15200
		public bool thingRewardItemsOnly;

		// Token: 0x04003B61 RID: 15201
		public List<ThingDef> disallowedThingDefs;

		// Token: 0x04003B62 RID: 15202
		public bool allowRoyalFavor;

		// Token: 0x04003B63 RID: 15203
		public bool allowGoodwill;

		// Token: 0x04003B64 RID: 15204
		public float populationIntent;
	}
}
