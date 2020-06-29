using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public struct RewardsGeneratorParams
	{
		
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

		
		public override string ToString()
		{
			return GenText.FieldsToString<RewardsGeneratorParams>(this);
		}

		
		public float rewardValue;

		
		public Faction giverFaction;

		
		public string chosenPawnSignal;

		
		public bool giveToCaravan;

		
		public float minGeneratedRewardValue;

		
		public bool thingRewardDisallowed;

		
		public bool thingRewardRequired;

		
		public bool thingRewardItemsOnly;

		
		public List<ThingDef> disallowedThingDefs;

		
		public bool allowRoyalFavor;

		
		public bool allowGoodwill;

		
		public float populationIntent;
	}
}
