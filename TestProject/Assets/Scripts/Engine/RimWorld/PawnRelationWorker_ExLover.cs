using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B54 RID: 2900
	public class PawnRelationWorker_ExLover : PawnRelationWorker
	{
		// Token: 0x0600441F RID: 17439 RVA: 0x00170AD2 File Offset: 0x0016ECD2
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x00170AE7 File Offset: 0x0016ECE7
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
			PawnRelationWorker_ExLover.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x06004421 RID: 17441 RVA: 0x00170B14 File Offset: 0x0016ED14
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin != null)
			{
				return;
			}
			request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
		}
	}
}
