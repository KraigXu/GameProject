﻿using System;

namespace RimWorld
{
	
	public class PawnGroupMakerParms
	{
		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"groupKind=",
				this.groupKind,
				", tile=",
				this.tile,
				", inhabitants=",
				this.inhabitants.ToString(),
				", points=",
				this.points,
				", faction=",
				this.faction,
				", traderKind=",
				this.traderKind,
				", generateFightersOnly=",
				this.generateFightersOnly.ToString(),
				", dontUseSingleUseRocketLaunchers=",
				this.dontUseSingleUseRocketLaunchers.ToString(),
				", raidStrategy=",
				this.raidStrategy,
				", forceOneIncap=",
				this.forceOneIncap.ToString(),
				", seed=",
				this.seed
			});
		}

		
		public PawnGroupKindDef groupKind;

		
		public int tile = -1;

		
		public bool inhabitants;

		
		public float points;

		
		public Faction faction;

		
		public TraderKindDef traderKind;

		
		public bool generateFightersOnly;

		
		public bool dontUseSingleUseRocketLaunchers;

		
		public RaidStrategyDef raidStrategy;

		
		public bool forceOneIncap;

		
		public int? seed;
	}
}
