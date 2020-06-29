using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public static class RecruitUtility
	{
		
		public static float RecruitChanceFactorForMood(Pawn recruitee)
		{
			if (recruitee.needs.mood == null)
			{
				return 1f;
			}
			float curLevel = recruitee.needs.mood.CurLevel;
			return RecruitUtility.RecruitChanceFactorCurve_Mood.Evaluate(curLevel);
		}

		
		public static float RecruitChanceFactorForOpinion(Pawn recruiter, Pawn recruitee)
		{
			if (recruitee.relations == null)
			{
				return 1f;
			}
			float x = (float)recruitee.relations.OpinionOf(recruiter);
			return RecruitUtility.RecruitChanceFactorCurve_Opinion.Evaluate(x);
		}

		
		public static float RecruitChanceFactorForRecruiterNegotiationAbility(Pawn recruiter)
		{
			return recruiter.GetStatValue(StatDefOf.NegotiationAbility, true) * 0.5f;
		}

		
		public static float RecruitChanceFactorForRecruiter(Pawn recruiter, Pawn recruitee)
		{
			return RecruitUtility.RecruitChanceFactorForRecruiterNegotiationAbility(recruiter) * RecruitUtility.RecruitChanceFactorForOpinion(recruiter, recruitee);
		}

		
		public static float RecruitChanceFactorForRecruitDifficulty(Pawn recruitee, Faction recruiterFaction)
		{
			float x = recruitee.RecruitDifficulty(recruiterFaction);
			return RecruitUtility.RecruitChanceFactorCurve_RecruitDifficulty.Evaluate(x);
		}

		
		public static float RecruitChanceFinalByFaction(this Pawn recruitee, Faction recruiterFaction)
		{
			return Mathf.Clamp01(RecruitUtility.RecruitChanceFactorForRecruitDifficulty(recruitee, recruiterFaction) * RecruitUtility.RecruitChanceFactorForMood(recruitee));
		}

		
		public static float RecruitChanceFinalByPawn(this Pawn recruitee, Pawn recruiter)
		{
			return Mathf.Clamp01(recruitee.RecruitChanceFinalByFaction(recruiter.Faction) * RecruitUtility.RecruitChanceFactorForRecruiter(recruiter, recruitee));
		}

		
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

		
		private const float RecruitChancePerNegotiatingAbility = 0.5f;
	}
}
