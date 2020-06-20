using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD2 RID: 4050
	public static class RecruitUtility
	{
		// Token: 0x06006137 RID: 24887 RVA: 0x0021C7C0 File Offset: 0x0021A9C0
		public static float RecruitChanceFactorForMood(Pawn recruitee)
		{
			if (recruitee.needs.mood == null)
			{
				return 1f;
			}
			float curLevel = recruitee.needs.mood.CurLevel;
			return RecruitUtility.RecruitChanceFactorCurve_Mood.Evaluate(curLevel);
		}

		// Token: 0x06006138 RID: 24888 RVA: 0x0021C7FC File Offset: 0x0021A9FC
		public static float RecruitChanceFactorForOpinion(Pawn recruiter, Pawn recruitee)
		{
			if (recruitee.relations == null)
			{
				return 1f;
			}
			float x = (float)recruitee.relations.OpinionOf(recruiter);
			return RecruitUtility.RecruitChanceFactorCurve_Opinion.Evaluate(x);
		}

		// Token: 0x06006139 RID: 24889 RVA: 0x0021C830 File Offset: 0x0021AA30
		public static float RecruitChanceFactorForRecruiterNegotiationAbility(Pawn recruiter)
		{
			return recruiter.GetStatValue(StatDefOf.NegotiationAbility, true) * 0.5f;
		}

		// Token: 0x0600613A RID: 24890 RVA: 0x0021C844 File Offset: 0x0021AA44
		public static float RecruitChanceFactorForRecruiter(Pawn recruiter, Pawn recruitee)
		{
			return RecruitUtility.RecruitChanceFactorForRecruiterNegotiationAbility(recruiter) * RecruitUtility.RecruitChanceFactorForOpinion(recruiter, recruitee);
		}

		// Token: 0x0600613B RID: 24891 RVA: 0x0021C854 File Offset: 0x0021AA54
		public static float RecruitChanceFactorForRecruitDifficulty(Pawn recruitee, Faction recruiterFaction)
		{
			float x = recruitee.RecruitDifficulty(recruiterFaction);
			return RecruitUtility.RecruitChanceFactorCurve_RecruitDifficulty.Evaluate(x);
		}

		// Token: 0x0600613C RID: 24892 RVA: 0x0021C874 File Offset: 0x0021AA74
		public static float RecruitChanceFinalByFaction(this Pawn recruitee, Faction recruiterFaction)
		{
			return Mathf.Clamp01(RecruitUtility.RecruitChanceFactorForRecruitDifficulty(recruitee, recruiterFaction) * RecruitUtility.RecruitChanceFactorForMood(recruitee));
		}

		// Token: 0x0600613D RID: 24893 RVA: 0x0021C889 File Offset: 0x0021AA89
		public static float RecruitChanceFinalByPawn(this Pawn recruitee, Pawn recruiter)
		{
			return Mathf.Clamp01(recruitee.RecruitChanceFinalByFaction(recruiter.Faction) * RecruitUtility.RecruitChanceFactorForRecruiter(recruiter, recruitee));
		}

		// Token: 0x04003B36 RID: 15158
		private static readonly SimpleCurve RecruitChanceFactorCurve_Mood = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 2f),
				true
			}
		};

		// Token: 0x04003B37 RID: 15159
		private static readonly SimpleCurve RecruitChanceFactorCurve_Opinion = new SimpleCurve
		{
			{
				new CurvePoint(-100f, 0.5f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 2f),
				true
			}
		};

		// Token: 0x04003B38 RID: 15160
		private static readonly SimpleCurve RecruitChanceFactorCurve_RecruitDifficulty = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.02f),
				true
			}
		};

		// Token: 0x04003B39 RID: 15161
		private const float RecruitChancePerNegotiatingAbility = 0.5f;
	}
}
