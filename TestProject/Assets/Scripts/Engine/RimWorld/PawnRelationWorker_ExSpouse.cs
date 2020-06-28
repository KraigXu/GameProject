using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B55 RID: 2901
	public class PawnRelationWorker_ExSpouse : PawnRelationWorker
	{
		// Token: 0x06004423 RID: 17443 RVA: 0x00170AD2 File Offset: 0x0016ECD2
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x00170B52 File Offset: 0x0016ED52
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			PawnRelationWorker_ExSpouse.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x00170B80 File Offset: 0x0016ED80
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
