using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public struct RoyalTitleFoodRequirement
	{
		
		
		public bool Defined
		{
			get
			{
				return this.minQuality > FoodPreferability.Undefined;
			}
		}

		
		public bool Acceptable(ThingDef food)
		{
			return food.ingestible != null && (this.allowedDefs.Contains(food) || (this.allowedTypes != FoodTypeFlags.None && (this.allowedTypes & food.ingestible.foodType) != FoodTypeFlags.None) || food.ingestible.preferability >= this.minQuality);
		}

		
		public FoodPreferability minQuality;

		
		public FoodTypeFlags allowedTypes;

		
		public List<ThingDef> allowedDefs;
	}
}
