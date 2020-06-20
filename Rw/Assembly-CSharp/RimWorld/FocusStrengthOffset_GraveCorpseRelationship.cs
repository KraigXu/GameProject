using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D28 RID: 3368
	public class FocusStrengthOffset_GraveCorpseRelationship : FocusStrengthOffset
	{
		// Token: 0x060051E5 RID: 20965 RVA: 0x001B626A File Offset: 0x001B446A
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_GraveCorpseRelatedAbstract".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x001B6125 File Offset: 0x001B4325
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			if (!this.CanApply(parent, user))
			{
				return 0f;
			}
			return this.offset;
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x001B629C File Offset: 0x001B449C
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			Building_Grave building_Grave = parent as Building_Grave;
			return parent.Spawned && building_Grave != null && building_Grave.HasCorpse && building_Grave.Corpse.InnerPawn.RaceProps.Humanlike && building_Grave.Corpse.InnerPawn.relations.PotentiallyRelatedPawns.Contains(user);
		}
	}
}
