using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008EB RID: 2283
	public class PawnRelationWorker
	{
		// Token: 0x060036A8 RID: 13992 RVA: 0x00127D74 File Offset: 0x00125F74
		public virtual bool InRelation(Pawn me, Pawn other)
		{
			if (this.def.implied)
			{
				throw new NotImplementedException(this.def + " lacks InRelation implementation.");
			}
			return me.relations.DirectRelationExists(this.def, other);
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return 0f;
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x00127DAB File Offset: 0x00125FAB
		public virtual void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (!this.def.implied)
			{
				generated.relations.AddDirectRelation(this.def, other);
				return;
			}
			throw new NotImplementedException(this.def + " lacks CreateRelation implementation.");
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x00127DE4 File Offset: 0x00125FE4
		public float BaseGenerationChanceFactor(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			if (generated.Faction != other.Faction)
			{
				num *= 0.65f;
			}
			if (generated.HostileTo(other))
			{
				num *= 0.7f;
			}
			if (other.Faction != null && other.Faction.IsPlayer && (generated.Faction == null || !generated.Faction.IsPlayer))
			{
				num *= 0.5f;
			}
			if (other.Faction != null && other.Faction.IsPlayer)
			{
				num *= request.ColonistRelationChanceFactor;
			}
			if (other == request.ExtraPawnForExtraRelationChance)
			{
				num *= request.RelationWithExtraPawnChanceFactor;
			}
			TechLevel techLevel = (generated.Faction != null) ? generated.Faction.def.techLevel : TechLevel.Undefined;
			TechLevel techLevel2 = (other.Faction != null) ? other.Faction.def.techLevel : TechLevel.Undefined;
			if (techLevel != TechLevel.Undefined && techLevel2 != TechLevel.Undefined && techLevel != techLevel2)
			{
				num *= 0.85f;
			}
			if ((techLevel.IsNeolithicOrWorse() && !techLevel2.IsNeolithicOrWorse()) || (!techLevel.IsNeolithicOrWorse() && techLevel2.IsNeolithicOrWorse()))
			{
				num *= 0.03f;
			}
			return num;
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void OnRelationCreated(Pawn firstPawn, Pawn secondPawn)
		{
		}

		// Token: 0x04001F3A RID: 7994
		public PawnRelationDef def;
	}
}
