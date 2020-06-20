using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B56 RID: 2902
	public class PawnRelationWorker_Fiance : PawnRelationWorker
	{
		// Token: 0x06004427 RID: 17447 RVA: 0x00170BC0 File Offset: 0x0016EDC0
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			num *= this.GetOldAgeFactor(generated);
			num *= this.GetOldAgeFactor(other);
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request) * num;
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x00170BFC File Offset: 0x0016EDFC
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Fiance, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.7f);
			PawnRelationWorker_Fiance.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x00170C29 File Offset: 0x0016EE29
		private float GetOldAgeFactor(Pawn pawn)
		{
			return Mathf.Clamp(GenMath.LerpDouble(50f, 80f, 1f, 0.01f, (float)pawn.ageTracker.AgeBiologicalYears), 0.01f, 1f);
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x00170C60 File Offset: 0x0016EE60
		public override void OnRelationCreated(Pawn firstPawn, Pawn secondPawn)
		{
			firstPawn.relations.nextMarriageNameChange = (secondPawn.relations.nextMarriageNameChange = SpouseRelationUtility.Roll_NameChangeOnMarriage());
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x00170C8C File Offset: 0x0016EE8C
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
