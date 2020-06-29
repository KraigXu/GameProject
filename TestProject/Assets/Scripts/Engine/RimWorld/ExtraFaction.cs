using System;
using Verse;

namespace RimWorld
{
	
	public class ExtraFaction : IExposable
	{
		
		public ExtraFaction()
		{
		}

		
		public ExtraFaction(Faction faction, ExtraFactionType factionType)
		{
			this.faction = faction;
			this.factionType = factionType;
		}

		
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<ExtraFactionType>(ref this.factionType, "factionType", ExtraFactionType.HomeFaction, false);
		}

		
		public Faction faction;

		
		public ExtraFactionType factionType;
	}
}
