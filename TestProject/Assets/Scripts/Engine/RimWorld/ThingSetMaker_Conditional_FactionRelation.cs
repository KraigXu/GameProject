using System;
using Verse;

namespace RimWorld
{
	
	public class ThingSetMaker_Conditional_FactionRelation : ThingSetMaker_Conditional
	{
		
		protected override bool Condition(ThingSetMakerParams parms)
		{
			Faction faction = Find.FactionManager.FirstFactionOfDef(this.factionDef);
			if (faction == null)
			{
				return false;
			}
			switch (faction.RelationKindWith(Faction.OfPlayer))
			{
			case FactionRelationKind.Hostile:
				return this.allowHostile;
			case FactionRelationKind.Neutral:
				return this.allowNeutral;
			case FactionRelationKind.Ally:
				return this.allowAlly;
			default:
				throw new NotImplementedException();
			}
		}

		
		public FactionDef factionDef;

		
		public bool allowHostile;

		
		public bool allowNeutral;

		
		public bool allowAlly;
	}
}
