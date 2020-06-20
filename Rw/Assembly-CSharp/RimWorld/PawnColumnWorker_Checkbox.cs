using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000ECE RID: 3790
	public abstract class PawnColumnWorker_Checkbox : PawnColumnWorker
	{
		// Token: 0x06005CE6 RID: 23782 RVA: 0x00203F72 File Offset: 0x00202172
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06005CE7 RID: 23783 RVA: 0x00203F84 File Offset: 0x00202184
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!this.HasCheckbox(pawn))
			{
				return;
			}
			int num = (int)((rect.width - 24f) / 2f);
			int num2 = Mathf.Max(3, 0);
			Vector2 vector = new Vector2(rect.x + (float)num, rect.y + (float)num2);
			Rect rect2 = new Rect(vector.x, vector.y, 24f, 24f);
			bool value = this.GetValue(pawn);
			bool flag = value;
			Widgets.Checkbox(vector, ref value, 24f, false, this.def.paintable, null, null);
			if (Mouse.IsOver(rect2))
			{
				string tip = this.GetTip(pawn);
				if (!tip.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect2, tip);
				}
			}
			if (value != flag)
			{
				this.SetValue(pawn, value);
			}
		}

		// Token: 0x06005CE8 RID: 23784 RVA: 0x0020404E File Offset: 0x0020224E
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x06005CE9 RID: 23785 RVA: 0x0020405E File Offset: 0x0020225E
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x00204073 File Offset: 0x00202273
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x06005CEB RID: 23787 RVA: 0x00204084 File Offset: 0x00202284
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06005CEC RID: 23788 RVA: 0x002040A7 File Offset: 0x002022A7
		private int GetValueToCompare(Pawn pawn)
		{
			if (!this.HasCheckbox(pawn))
			{
				return 0;
			}
			if (!this.GetValue(pawn))
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06005CED RID: 23789 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected virtual string GetTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06005CEE RID: 23790 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool HasCheckbox(Pawn pawn)
		{
			return true;
		}

		// Token: 0x06005CEF RID: 23791
		protected abstract bool GetValue(Pawn pawn);

		// Token: 0x06005CF0 RID: 23792
		protected abstract void SetValue(Pawn pawn, bool value);

		// Token: 0x06005CF1 RID: 23793 RVA: 0x002040C0 File Offset: 0x002022C0
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (this.HasCheckbox(pawnsListForReading[i]))
					{
						if (Event.current.button == 0)
						{
							if (!this.GetValue(pawnsListForReading[i]))
							{
								this.SetValue(pawnsListForReading[i], true);
							}
						}
						else if (Event.current.button == 1 && this.GetValue(pawnsListForReading[i]))
						{
							this.SetValue(pawnsListForReading[i], false);
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

		// Token: 0x06005CF2 RID: 23794 RVA: 0x00204190 File Offset: 0x00202390
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "CheckboxShiftClickTip".Translate();
		}

		// Token: 0x040032BB RID: 12987
		public const int HorizontalPadding = 2;
	}
}
