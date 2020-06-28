using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B4D RID: 2893
	public static class LovePartnerRelationUtility
	{
		// Token: 0x060043EA RID: 17386 RVA: 0x0016F340 File Offset: 0x0016D540
		public static bool HasAnyLovePartner(Pawn pawn)
		{
			return pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null) != null || pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, null) != null || pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null) != null;
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x0016F37E File Offset: 0x0016D57E
		public static bool IsLovePartnerRelation(PawnRelationDef relation)
		{
			return relation == PawnRelationDefOf.Lover || relation == PawnRelationDefOf.Fiance || relation == PawnRelationDefOf.Spouse;
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x0016F39A File Offset: 0x0016D59A
		public static bool IsExLovePartnerRelation(PawnRelationDef relation)
		{
			return relation == PawnRelationDefOf.ExLover || relation == PawnRelationDefOf.ExSpouse;
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x0016F3B0 File Offset: 0x0016D5B0
		public static bool HasAnyLovePartnerOfTheSameGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsLovePartnerRelation(x.def) && x.otherPawn.gender == pawn.gender) != null;
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x0016F3F0 File Offset: 0x0016D5F0
		public static bool HasAnyExLovePartnerOfTheSameGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsExLovePartnerRelation(x.def) && x.otherPawn.gender == pawn.gender) != null;
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x0016F430 File Offset: 0x0016D630
		public static bool HasAnyLovePartnerOfTheOppositeGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsLovePartnerRelation(x.def) && x.otherPawn.gender != pawn.gender) != null;
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x0016F470 File Offset: 0x0016D670
		public static bool HasAnyExLovePartnerOfTheOppositeGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsExLovePartnerRelation(x.def) && x.otherPawn.gender != pawn.gender) != null;
		}

		// Token: 0x060043F1 RID: 17393 RVA: 0x0016F4B0 File Offset: 0x0016D6B0
		public static Pawn ExistingLovePartner(Pawn pawn)
		{
			Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
			if (firstDirectRelationPawn != null)
			{
				return firstDirectRelationPawn;
			}
			firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, null);
			if (firstDirectRelationPawn != null)
			{
				return firstDirectRelationPawn;
			}
			firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null);
			if (firstDirectRelationPawn != null)
			{
				return firstDirectRelationPawn;
			}
			return null;
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x0016F503 File Offset: 0x0016D703
		public static bool LovePartnerRelationExists(Pawn first, Pawn second)
		{
			return first.relations.DirectRelationExists(PawnRelationDefOf.Lover, second) || first.relations.DirectRelationExists(PawnRelationDefOf.Fiance, second) || first.relations.DirectRelationExists(PawnRelationDefOf.Spouse, second);
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x0016F53E File Offset: 0x0016D73E
		public static bool ExLovePartnerRelationExists(Pawn first, Pawn second)
		{
			return first.relations.DirectRelationExists(PawnRelationDefOf.ExSpouse, second) || first.relations.DirectRelationExists(PawnRelationDefOf.ExLover, second);
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x0016F568 File Offset: 0x0016D768
		public static void GiveRandomExLoverOrExSpouseRelation(Pawn first, Pawn second)
		{
			PawnRelationDef def;
			if (Rand.Value < 0.5f)
			{
				def = PawnRelationDefOf.ExLover;
			}
			else
			{
				def = PawnRelationDefOf.ExSpouse;
			}
			first.relations.AddDirectRelation(def, second);
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x0016F59C File Offset: 0x0016D79C
		public static Pawn GetPartnerInMyBed(Pawn pawn)
		{
			Building_Bed building_Bed = pawn.CurrentBed();
			if (building_Bed == null)
			{
				return null;
			}
			if (building_Bed.SleepingSlotsCount <= 1)
			{
				return null;
			}
			if (!LovePartnerRelationUtility.HasAnyLovePartner(pawn))
			{
				return null;
			}
			foreach (Pawn pawn2 in building_Bed.CurOccupants)
			{
				if (pawn2 != pawn && LovePartnerRelationUtility.LovePartnerRelationExists(pawn, pawn2))
				{
					return pawn2;
				}
			}
			return null;
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x0016F618 File Offset: 0x0016D818
		public static Pawn ExistingMostLikedLovePartner(Pawn p, bool allowDead)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, allowDead);
			if (directPawnRelation != null)
			{
				return directPawnRelation.otherPawn;
			}
			return null;
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x0016F638 File Offset: 0x0016D838
		public static DirectPawnRelation ExistingMostLikedLovePartnerRel(Pawn p, bool allowDead)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return null;
			}
			DirectPawnRelation directPawnRelation = null;
			int num = int.MinValue;
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if ((allowDead || !directRelations[i].otherPawn.Dead) && LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def))
				{
					int num2 = p.relations.OpinionOf(directRelations[i].otherPawn);
					if (directPawnRelation == null || num2 > num)
					{
						directPawnRelation = directRelations[i];
						num = num2;
					}
				}
			}
			return directPawnRelation;
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x0016F6D0 File Offset: 0x0016D8D0
		public static float GetLovinMtbHours(Pawn pawn, Pawn partner)
		{
			if (pawn.Dead || partner.Dead)
			{
				return -1f;
			}
			if (DebugSettings.alwaysDoLovin)
			{
				return 0.1f;
			}
			if (pawn.needs.food.Starving || partner.needs.food.Starving)
			{
				return -1f;
			}
			if (pawn.health.hediffSet.BleedRateTotal > 0f || partner.health.hediffSet.BleedRateTotal > 0f)
			{
				return -1f;
			}
			float num = LovePartnerRelationUtility.LovinMtbSinglePawnFactor(pawn);
			if (num <= 0f)
			{
				return -1f;
			}
			float num2 = LovePartnerRelationUtility.LovinMtbSinglePawnFactor(partner);
			if (num2 <= 0f)
			{
				return -1f;
			}
			return 12f * num * num2 / Mathf.Max(pawn.relations.SecondaryLovinChanceFactor(partner), 0.1f) / Mathf.Max(partner.relations.SecondaryLovinChanceFactor(pawn), 0.1f) * GenMath.LerpDouble(-100f, 100f, 1.3f, 0.7f, (float)pawn.relations.OpinionOf(partner)) * GenMath.LerpDouble(-100f, 100f, 1.3f, 0.7f, (float)partner.relations.OpinionOf(pawn));
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x0016F810 File Offset: 0x0016DA10
		private static float LovinMtbSinglePawnFactor(Pawn pawn)
		{
			float num = 1f;
			num /= 1f - pawn.health.hediffSet.PainTotal;
			float level = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness);
			if (level < 0.5f)
			{
				num /= level * 2f;
			}
			return num / GenMath.FlatHill(0f, 14f, 16f, 25f, 80f, 0.2f, pawn.ageTracker.AgeBiologicalYearsFloat);
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x0016F896 File Offset: 0x0016DA96
		public static void TryToShareBed(Pawn first, Pawn second)
		{
			if (LovePartnerRelationUtility.TryToShareBed_Int(first, second))
			{
				return;
			}
			LovePartnerRelationUtility.TryToShareBed_Int(second, first);
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x0016F8AC File Offset: 0x0016DAAC
		private static bool TryToShareBed_Int(Pawn bedOwner, Pawn otherPawn)
		{
			Building_Bed ownedBed = bedOwner.ownership.OwnedBed;
			if (ownedBed != null && ownedBed.AnyUnownedSleepingSlot)
			{
				otherPawn.ownership.ClaimBedIfNonMedical(ownedBed);
				return true;
			}
			return false;
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0016F8E0 File Offset: 0x0016DAE0
		public static float LovePartnerRelationGenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request, bool ex)
		{
			if (generated.ageTracker.AgeBiologicalYearsFloat < 14f)
			{
				return 0f;
			}
			if (other.ageTracker.AgeBiologicalYearsFloat < 14f)
			{
				return 0f;
			}
			if (generated.gender == other.gender && (!other.story.traits.HasTrait(TraitDefOf.Gay) || !request.AllowGay))
			{
				return 0f;
			}
			if (generated.gender != other.gender && other.story.traits.HasTrait(TraitDefOf.Gay))
			{
				return 0f;
			}
			float num = 1f;
			if (ex)
			{
				int num2 = 0;
				List<DirectPawnRelation> directRelations = other.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					if (LovePartnerRelationUtility.IsExLovePartnerRelation(directRelations[i].def))
					{
						num2++;
					}
				}
				num = Mathf.Pow(0.2f, (float)num2);
			}
			else if (LovePartnerRelationUtility.HasAnyLovePartner(other))
			{
				return 0f;
			}
			float num3 = (generated.gender == other.gender) ? 0.01f : 1f;
			float generationChanceAgeFactor = LovePartnerRelationUtility.GetGenerationChanceAgeFactor(generated);
			float generationChanceAgeFactor2 = LovePartnerRelationUtility.GetGenerationChanceAgeFactor(other);
			float generationChanceAgeGapFactor = LovePartnerRelationUtility.GetGenerationChanceAgeGapFactor(generated, other, ex);
			float num4 = 1f;
			if (generated.GetRelations(other).Any((PawnRelationDef x) => x.familyByBloodRelation))
			{
				num4 = 0.01f;
			}
			float num5;
			if (request.FixedMelanin != null)
			{
				num5 = ChildRelationUtility.GetMelaninSimilarityFactor(request.FixedMelanin.Value, other.story.melanin);
			}
			else
			{
				num5 = PawnSkinColors.GetMelaninCommonalityFactor(other.story.melanin);
			}
			return num * generationChanceAgeFactor * generationChanceAgeFactor2 * generationChanceAgeGapFactor * num3 * num5 * num4;
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x0016FAB1 File Offset: 0x0016DCB1
		private static float GetGenerationChanceAgeFactor(Pawn p)
		{
			return Mathf.Clamp(GenMath.LerpDouble(14f, 27f, 0f, 1f, p.ageTracker.AgeBiologicalYearsFloat), 0f, 1f);
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x0016FAE8 File Offset: 0x0016DCE8
		private static float GetGenerationChanceAgeGapFactor(Pawn p1, Pawn p2, bool ex)
		{
			float num = Mathf.Abs(p1.ageTracker.AgeBiologicalYearsFloat - p2.ageTracker.AgeBiologicalYearsFloat);
			if (ex)
			{
				float num2 = LovePartnerRelationUtility.MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p1, p2);
				if (num2 >= 0f)
				{
					num = Mathf.Min(num, num2);
				}
				float num3 = LovePartnerRelationUtility.MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p2, p1);
				if (num3 >= 0f)
				{
					num = Mathf.Min(num, num3);
				}
			}
			if (num > 40f)
			{
				return 0f;
			}
			return Mathf.Clamp(GenMath.LerpDouble(0f, 20f, 1f, 0.001f, num), 0.001f, 1f);
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x0016FB7C File Offset: 0x0016DD7C
		private static float MinPossibleAgeGapAtMinAgeToGenerateAsLovers(Pawn p1, Pawn p2)
		{
			float num = p1.ageTracker.AgeChronologicalYearsFloat - 14f;
			if (num < 0f)
			{
				Log.Warning("at < 0", false);
				return 0f;
			}
			float num2 = PawnRelationUtility.MaxPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, p2.ageTracker.AgeChronologicalYearsFloat, num);
			float num3 = PawnRelationUtility.MinPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, num);
			if (num2 < 0f)
			{
				return -1f;
			}
			if (num2 < 14f)
			{
				return -1f;
			}
			if (num3 <= 14f)
			{
				return 0f;
			}
			return num3 - 14f;
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x0016FC14 File Offset: 0x0016DE14
		public static void TryToShareChildrenForGeneratedLovePartner(Pawn generated, Pawn other, PawnGenerationRequest request, float extraChanceFactor)
		{
			if (generated.gender == other.gender)
			{
				return;
			}
			List<Pawn> list = other.relations.Children.ToList<Pawn>();
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				float num = 1f;
				if (generated.gender == Gender.Male)
				{
					num = ChildRelationUtility.ChanceOfBecomingChildOf(pawn, generated, other, null, new PawnGenerationRequest?(request), null);
				}
				else if (generated.gender == Gender.Female)
				{
					num = ChildRelationUtility.ChanceOfBecomingChildOf(pawn, other, generated, null, null, new PawnGenerationRequest?(request));
				}
				num *= extraChanceFactor;
				if (Rand.Value < num)
				{
					if (generated.gender == Gender.Male)
					{
						pawn.SetFather(generated);
					}
					else if (generated.gender == Gender.Female)
					{
						pawn.SetMother(generated);
					}
				}
			}
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x0016FCF0 File Offset: 0x0016DEF0
		public static void ChangeSpouseRelationsToExSpouse(Pawn pawn)
		{
			for (;;)
			{
				Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
				if (firstDirectRelationPawn == null)
				{
					break;
				}
				pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Spouse, firstDirectRelationPawn);
				pawn.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, firstDirectRelationPawn);
			}
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x0016FD38 File Offset: 0x0016DF38
		public static Pawn GetMostDislikedNonPartnerBedOwner(Pawn p)
		{
			Building_Bed ownedBed = p.ownership.OwnedBed;
			if (ownedBed == null)
			{
				return null;
			}
			Pawn pawn = null;
			int num = 0;
			for (int i = 0; i < ownedBed.OwnersForReading.Count; i++)
			{
				if (ownedBed.OwnersForReading[i] != p && !LovePartnerRelationUtility.LovePartnerRelationExists(p, ownedBed.OwnersForReading[i]))
				{
					int num2 = p.relations.OpinionOf(ownedBed.OwnersForReading[i]);
					if (pawn == null || num2 < num)
					{
						pawn = ownedBed.OwnersForReading[i];
						num = num2;
					}
				}
			}
			return pawn;
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x0016FDC8 File Offset: 0x0016DFC8
		public static float IncestOpinionOffsetFor(Pawn other, Pawn pawn)
		{
			float num = 0f;
			List<DirectPawnRelation> directRelations = other.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && directRelations[i].otherPawn != pawn && !directRelations[i].otherPawn.Dead)
				{
					foreach (PawnRelationDef pawnRelationDef in other.GetRelations(directRelations[i].otherPawn))
					{
						float incestOpinionOffset = pawnRelationDef.incestOpinionOffset;
						if (incestOpinionOffset < num)
						{
							num = incestOpinionOffset;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x040026E6 RID: 9958
		private const float MinAgeToGenerateWithLovePartnerRelation = 14f;
	}
}
