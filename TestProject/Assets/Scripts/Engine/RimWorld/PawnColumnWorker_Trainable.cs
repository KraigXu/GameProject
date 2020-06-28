using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EEE RID: 3822
	public class PawnColumnWorker_Trainable : PawnColumnWorker
	{
		// Token: 0x06005DB0 RID: 23984 RVA: 0x00203F72 File Offset: 0x00202172
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06005DB1 RID: 23985 RVA: 0x00205C2C File Offset: 0x00203E2C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.training == null)
			{
				return;
			}
			bool flag;
			AcceptanceReport canTrain = pawn.training.CanAssignToTrain(this.def.trainable, out flag);
			if (!flag || !canTrain.Accepted)
			{
				return;
			}
			int num = (int)((rect.width - 24f) / 2f);
			int num2 = Mathf.Max(3, 0);
			TrainingCardUtility.DoTrainableCheckbox(new Rect(rect.x + (float)num, rect.y + (float)num2, 24f, 24f), pawn, this.def.trainable, canTrain, false, true);
		}

		// Token: 0x06005DB2 RID: 23986 RVA: 0x0020526B File Offset: 0x0020346B
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06005DB3 RID: 23987 RVA: 0x0020405E File Offset: 0x0020225E
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06005DB4 RID: 23988 RVA: 0x00204073 File Offset: 0x00202273
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x06005DB5 RID: 23989 RVA: 0x00205CBC File Offset: 0x00203EBC
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06005DB6 RID: 23990 RVA: 0x00205CE0 File Offset: 0x00203EE0
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.training == null)
			{
				return int.MinValue;
			}
			if (pawn.training.HasLearned(this.def.trainable))
			{
				return 4;
			}
			bool flag;
			AcceptanceReport acceptanceReport = pawn.training.CanAssignToTrain(this.def.trainable, out flag);
			if (!flag)
			{
				return 0;
			}
			if (!acceptanceReport.Accepted)
			{
				return 1;
			}
			if (!pawn.training.GetWanted(this.def.trainable))
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x06005DB7 RID: 23991 RVA: 0x00205D5C File Offset: 0x00203F5C
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (pawnsListForReading[i].training != null && !pawnsListForReading[i].training.HasLearned(this.def.trainable))
					{
						bool flag;
						AcceptanceReport acceptanceReport = pawnsListForReading[i].training.CanAssignToTrain(this.def.trainable, out flag);
						if (flag && acceptanceReport.Accepted)
						{
							bool wanted = pawnsListForReading[i].training.GetWanted(this.def.trainable);
							if (Event.current.button == 0)
							{
								if (!wanted)
								{
									pawnsListForReading[i].training.SetWantedRecursive(this.def.trainable, true);
								}
							}
							else if (Event.current.button == 1 && wanted)
							{
								pawnsListForReading[i].training.SetWantedRecursive(this.def.trainable, false);
							}
						}
					}
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x06005DB8 RID: 23992 RVA: 0x00204190 File Offset: 0x00202390
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "CheckboxShiftClickTip".Translate();
		}
	}
}
