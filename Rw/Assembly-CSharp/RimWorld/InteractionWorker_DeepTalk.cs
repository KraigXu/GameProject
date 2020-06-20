using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B44 RID: 2884
	public class InteractionWorker_DeepTalk : InteractionWorker
	{
		// Token: 0x060043CC RID: 17356 RVA: 0x0016D724 File Offset: 0x0016B924
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.075f * this.CompatibilityFactorCurve.Evaluate(initiator.relations.CompatibilityWith(recipient));
		}

		// Token: 0x040026CD RID: 9933
		private const float BaseSelectionWeight = 0.075f;

		// Token: 0x040026CE RID: 9934
		private SimpleCurve CompatibilityFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1.5f, 0f),
				true
			},
			{
				new CurvePoint(-0.5f, 0.1f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.8f),
				true
			},
			{
				new CurvePoint(2f, 3f),
				true
			}
		};
	}
}
