using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B46 RID: 2886
	public static class NegativeInteractionUtility
	{
		// Token: 0x060043D0 RID: 17360 RVA: 0x0016D7E0 File Offset: 0x0016B9E0
		public static float NegativeInteractionChanceFactor(Pawn initiator, Pawn recipient)
		{
			if (initiator.story.traits.HasTrait(TraitDefOf.Kind))
			{
				return 0f;
			}
			float num = 1f;
			num *= NegativeInteractionUtility.OpinionFactorCurve.Evaluate((float)initiator.relations.OpinionOf(recipient));
			num *= NegativeInteractionUtility.CompatibilityFactorCurve.Evaluate(initiator.relations.CompatibilityWith(recipient));
			if (initiator.story.traits.HasTrait(TraitDefOf.Abrasive))
			{
				num *= 2.3f;
			}
			return num;
		}

		// Token: 0x040026D0 RID: 9936
		public const float AbrasiveSelectionChanceFactor = 2.3f;

		// Token: 0x040026D1 RID: 9937
		private static readonly SimpleCurve CompatibilityFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-2.5f, 4f),
				true
			},
			{
				new CurvePoint(-1.5f, 3f),
				true
			},
			{
				new CurvePoint(-0.5f, 2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.75f),
				true
			},
			{
				new CurvePoint(2f, 0.5f),
				true
			},
			{
				new CurvePoint(3f, 0.4f),
				true
			}
		};

		// Token: 0x040026D2 RID: 9938
		private static readonly SimpleCurve OpinionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-100f, 6f),
				true
			},
			{
				new CurvePoint(-50f, 4f),
				true
			},
			{
				new CurvePoint(-25f, 2f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(50f, 0.1f),
				true
			},
			{
				new CurvePoint(100f, 0f),
				true
			}
		};
	}
}
