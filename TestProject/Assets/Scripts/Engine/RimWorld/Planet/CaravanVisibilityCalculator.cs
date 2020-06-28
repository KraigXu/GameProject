using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200123E RID: 4670
	public static class CaravanVisibilityCalculator
	{
		// Token: 0x06006CCA RID: 27850 RVA: 0x0025F8A0 File Offset: 0x0025DAA0
		public static float Visibility(float bodySizeSum, bool caravanMovingNow, StringBuilder explanation = null)
		{
			float num = CaravanVisibilityCalculator.BodySizeSumToVisibility.Evaluate(bodySizeSum);
			if (explanation != null)
			{
				if (explanation.Length > 0)
				{
					explanation.AppendLine();
				}
				explanation.Append("TotalBodySize".Translate() + ": " + bodySizeSum.ToString("0.##"));
			}
			if (!caravanMovingNow)
			{
				num *= 0.3f;
				if (explanation != null)
				{
					explanation.AppendLine();
					explanation.Append("CaravanNotMoving".Translate() + ": " + 0.3f.ToStringPercent());
				}
			}
			return num;
		}

		// Token: 0x06006CCB RID: 27851 RVA: 0x0025F942 File Offset: 0x0025DB42
		public static float Visibility(Caravan caravan, StringBuilder explanation = null)
		{
			return CaravanVisibilityCalculator.Visibility(caravan.PawnsListForReading, caravan.pather.MovingNow, explanation);
		}

		// Token: 0x06006CCC RID: 27852 RVA: 0x0025F95C File Offset: 0x0025DB5C
		public static float Visibility(List<Pawn> pawns, bool caravanMovingNow, StringBuilder explanation = null)
		{
			float num = 0f;
			for (int i = 0; i < pawns.Count; i++)
			{
				num += pawns[i].BodySize;
			}
			return CaravanVisibilityCalculator.Visibility(num, caravanMovingNow, explanation);
		}

		// Token: 0x06006CCD RID: 27853 RVA: 0x0025F997 File Offset: 0x0025DB97
		public static float Visibility(IEnumerable<Pawn> pawns, bool caravanMovingNow, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			CaravanVisibilityCalculator.tmpPawns.AddRange(pawns);
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, caravanMovingNow, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06006CCE RID: 27854 RVA: 0x0025F9C4 File Offset: 0x0025DBC4
		public static float Visibility(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
					{
						CaravanVisibilityCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, true, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06006CCF RID: 27855 RVA: 0x0025FA4C File Offset: 0x0025DC4C
		public static float VisibilityLeftAfterTransfer(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
					{
						CaravanVisibilityCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, true, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06006CD0 RID: 27856 RVA: 0x0025FADF File Offset: 0x0025DCDF
		public static float VisibilityLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, CaravanVisibilityCalculator.tmpThingCounts);
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpThingCounts, explanation);
			CaravanVisibilityCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06006CD1 RID: 27857 RVA: 0x0025FB0C File Offset: 0x0025DD0C
		public static float Visibility(List<ThingCount> thingCounts, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						CaravanVisibilityCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, true, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0400439F RID: 17311
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x040043A0 RID: 17312
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040043A1 RID: 17313
		private static readonly SimpleCurve BodySizeSumToVisibility = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0.2f),
				true
			},
			{
				new CurvePoint(6f, 1f),
				true
			},
			{
				new CurvePoint(12f, 1.12f),
				true
			}
		};

		// Token: 0x040043A2 RID: 17314
		public const float NotMovingFactor = 0.3f;
	}
}
