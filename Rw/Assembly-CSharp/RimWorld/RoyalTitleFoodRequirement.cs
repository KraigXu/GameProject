using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103B RID: 4155
	public struct RoyalTitleFoodRequirement
	{
		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x06006342 RID: 25410 RVA: 0x00227EFA File Offset: 0x002260FA
		public bool Defined
		{
			get
			{
				return this.minQuality > FoodPreferability.Undefined;
			}
		}

		// Token: 0x06006343 RID: 25411 RVA: 0x00227F08 File Offset: 0x00226108
		public bool Acceptable(ThingDef food)
		{
			return food.ingestible != null && (this.allowedDefs.Contains(food) || (this.allowedTypes != FoodTypeFlags.None && (this.allowedTypes & food.ingestible.foodType) != FoodTypeFlags.None) || food.ingestible.preferability >= this.minQuality);
		}

		// Token: 0x04003C65 RID: 15461
		public FoodPreferability minQuality;

		// Token: 0x04003C66 RID: 15462
		public FoodTypeFlags allowedTypes;

		// Token: 0x04003C67 RID: 15463
		public List<ThingDef> allowedDefs;
	}
}
