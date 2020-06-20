using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC6 RID: 3270
	public class ThingSetMaker_Conditional_FactionRelation : ThingSetMaker_Conditional
	{
		// Token: 0x06004F4F RID: 20303 RVA: 0x001AB620 File Offset: 0x001A9820
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

		// Token: 0x04002C7F RID: 11391
		public FactionDef factionDef;

		// Token: 0x04002C80 RID: 11392
		public bool allowHostile;

		// Token: 0x04002C81 RID: 11393
		public bool allowNeutral;

		// Token: 0x04002C82 RID: 11394
		public bool allowAlly;
	}
}
