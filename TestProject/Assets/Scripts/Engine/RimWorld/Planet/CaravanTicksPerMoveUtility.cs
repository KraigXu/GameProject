using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200123C RID: 4668
	public static class CaravanTicksPerMoveUtility
	{
		// Token: 0x06006CBD RID: 27837 RVA: 0x0025F392 File Offset: 0x0025D592
		public static int GetTicksPerMove(Caravan caravan, StringBuilder explanation = null)
		{
			if (caravan == null)
			{
				if (explanation != null)
				{
					CaravanTicksPerMoveUtility.AppendUsingDefaultTicksPerMoveInfo(explanation);
				}
				return 3300;
			}
			return CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(caravan), explanation);
		}

		// Token: 0x06006CBE RID: 27838 RVA: 0x0025F3B2 File Offset: 0x0025D5B2
		public static int GetTicksPerMove(CaravanTicksPerMoveUtility.CaravanInfo caravanInfo, StringBuilder explanation = null)
		{
			return CaravanTicksPerMoveUtility.GetTicksPerMove(caravanInfo.pawns, caravanInfo.massUsage, caravanInfo.massCapacity, explanation);
		}

		// Token: 0x06006CBF RID: 27839 RVA: 0x0025F3CC File Offset: 0x0025D5CC
		public static int GetTicksPerMove(List<Pawn> pawns, float massUsage, float massCapacity, StringBuilder explanation = null)
		{
			if (pawns.Any<Pawn>())
			{
				if (explanation != null)
				{
					explanation.Append("CaravanMovementSpeedFull".Translate() + ":");
				}
				float num = 0f;
				for (int i = 0; i < pawns.Count; i++)
				{
					float num2 = (float)((pawns[i].Downed || pawns[i].CarriedByCaravan()) ? 450 : pawns[i].TicksPerMoveCardinal);
					num2 = Mathf.Min(num2, 150f) * 340f;
					float num3 = 60000f / num2;
					if (explanation != null)
					{
						explanation.AppendLine();
						explanation.Append(string.Concat(new string[]
						{
							"  - ",
							pawns[i].LabelShortCap,
							": ",
							num3.ToString("0.#"),
							" "
						}) + "TilesPerDay".Translate());
						if (pawns[i].Downed)
						{
							explanation.Append(" (" + "DownedLower".Translate() + ")");
						}
						else if (pawns[i].CarriedByCaravan())
						{
							explanation.Append(" (" + "Carried".Translate() + ")");
						}
					}
					num += num2 / (float)pawns.Count;
				}
				float moveSpeedFactorFromMass = CaravanTicksPerMoveUtility.GetMoveSpeedFactorFromMass(massUsage, massCapacity);
				if (explanation != null)
				{
					float num4 = 60000f / num;
					explanation.AppendLine();
					explanation.Append("  " + "Average".Translate() + ": " + num4.ToString("0.#") + " " + "TilesPerDay".Translate());
					explanation.AppendLine();
					explanation.Append("  " + "MultiplierForCarriedMass".Translate(moveSpeedFactorFromMass.ToStringPercent()));
				}
				int num5 = Mathf.Max(Mathf.RoundToInt(num / moveSpeedFactorFromMass), 1);
				if (explanation != null)
				{
					float num6 = 60000f / (float)num5;
					explanation.AppendLine();
					explanation.Append("  " + "FinalCaravanPawnsMovementSpeed".Translate() + ": " + num6.ToString("0.#") + " " + "TilesPerDay".Translate());
				}
				return num5;
			}
			if (explanation != null)
			{
				CaravanTicksPerMoveUtility.AppendUsingDefaultTicksPerMoveInfo(explanation);
			}
			return 3300;
		}

		// Token: 0x06006CC0 RID: 27840 RVA: 0x0025F68C File Offset: 0x0025D88C
		private static float GetMoveSpeedFactorFromMass(float massUsage, float massCapacity)
		{
			if (massCapacity <= 0f)
			{
				return 1f;
			}
			float t = massUsage / massCapacity;
			return Mathf.Lerp(2f, 1f, t);
		}

		// Token: 0x06006CC1 RID: 27841 RVA: 0x0025F6BC File Offset: 0x0025D8BC
		private static void AppendUsingDefaultTicksPerMoveInfo(StringBuilder sb)
		{
			sb.Append("CaravanMovementSpeedFull".Translate() + ":");
			float num = 18.181818f;
			sb.AppendLine();
			sb.Append("  " + "Default".Translate() + ": " + num.ToString("0.#") + " " + "TilesPerDay".Translate());
		}

		// Token: 0x0400439A RID: 17306
		private const int MaxPawnTicksPerMove = 150;

		// Token: 0x0400439B RID: 17307
		private const int DownedPawnMoveTicks = 450;

		// Token: 0x0400439C RID: 17308
		public const float CellToTilesConversionRatio = 340f;

		// Token: 0x0400439D RID: 17309
		public const int DefaultTicksPerMove = 3300;

		// Token: 0x0400439E RID: 17310
		private const float MoveSpeedFactorAtZeroMass = 2f;

		// Token: 0x02001FDA RID: 8154
		public struct CaravanInfo
		{
			// Token: 0x0600AEF0 RID: 44784 RVA: 0x003270D6 File Offset: 0x003252D6
			public CaravanInfo(Caravan caravan)
			{
				this.pawns = caravan.PawnsListForReading;
				this.massUsage = caravan.MassUsage;
				this.massCapacity = caravan.MassCapacity;
			}

			// Token: 0x0600AEF1 RID: 44785 RVA: 0x003270FC File Offset: 0x003252FC
			public CaravanInfo(Dialog_FormCaravan formCaravanDialog)
			{
				this.pawns = TransferableUtility.GetPawnsFromTransferables(formCaravanDialog.transferables);
				this.massUsage = formCaravanDialog.MassUsage;
				this.massCapacity = formCaravanDialog.MassCapacity;
			}

			// Token: 0x04007727 RID: 30503
			public List<Pawn> pawns;

			// Token: 0x04007728 RID: 30504
			public float massUsage;

			// Token: 0x04007729 RID: 30505
			public float massCapacity;
		}
	}
}
