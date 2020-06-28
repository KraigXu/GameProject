using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B5F RID: 2911
	public class PawnRelationWorker_Lover : PawnRelationWorker
	{
		// Token: 0x0600443D RID: 17469 RVA: 0x00170EF8 File Offset: 0x0016F0F8
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x00170F0D File Offset: 0x0016F10D
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Lover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
			PawnRelationWorker_Lover.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x00170F3C File Offset: 0x0016F13C
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
