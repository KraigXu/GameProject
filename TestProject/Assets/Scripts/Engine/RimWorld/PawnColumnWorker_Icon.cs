using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDA RID: 3802
	public abstract class PawnColumnWorker_Icon : PawnColumnWorker
	{
		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x06005D2C RID: 23852 RVA: 0x00204700 File Offset: 0x00202900
		protected virtual int Width
		{
			get
			{
				return 26;
			}
		}

		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x06005D2D RID: 23853 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		protected virtual int Padding
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06005D2E RID: 23854 RVA: 0x00204704 File Offset: 0x00202904
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			if (iconFor != null)
			{
				Vector2 iconSize = this.GetIconSize(pawn);
				int num = (int)((rect.width - iconSize.x) / 2f);
				int num2 = Mathf.Max((int)((30f - iconSize.y) / 2f), 0);
				Rect rect2 = new Rect(rect.x + (float)num, rect.y + (float)num2, iconSize.x, iconSize.y);
				GUI.color = this.GetIconColor(pawn);
				GUI.DrawTexture(rect2.ContractedBy((float)this.Padding), iconFor);
				GUI.color = Color.white;
				if (Mouse.IsOver(rect2))
				{
					string iconTip = this.GetIconTip(pawn);
					if (!iconTip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(rect2, iconTip);
					}
				}
				if (Widgets.ButtonInvisible(rect2, false))
				{
					this.ClickedIcon(pawn);
				}
				if (Mouse.IsOver(rect2) && Input.GetMouseButton(0))
				{
					this.PaintedIcon(pawn);
				}
			}
		}

		// Token: 0x06005D2F RID: 23855 RVA: 0x00204800 File Offset: 0x00202A00
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x06005D30 RID: 23856 RVA: 0x0020405E File Offset: 0x0020225E
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06005D31 RID: 23857 RVA: 0x00204814 File Offset: 0x00202A14
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(this.GetIconSize(pawn).y));
		}

		// Token: 0x06005D32 RID: 23858 RVA: 0x00204834 File Offset: 0x00202A34
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06005D33 RID: 23859 RVA: 0x00204858 File Offset: 0x00202A58
		private int GetValueToCompare(Pawn pawn)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			if (!(iconFor != null))
			{
				return int.MinValue;
			}
			return iconFor.GetInstanceID();
		}

		// Token: 0x06005D34 RID: 23860
		protected abstract Texture2D GetIconFor(Pawn pawn);

		// Token: 0x06005D35 RID: 23861 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected virtual string GetIconTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06005D36 RID: 23862 RVA: 0x00017A00 File Offset: 0x00015C00
		protected virtual Color GetIconColor(Pawn pawn)
		{
			return Color.white;
		}

		// Token: 0x06005D37 RID: 23863 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void ClickedIcon(Pawn pawn)
		{
		}

		// Token: 0x06005D38 RID: 23864 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void PaintedIcon(Pawn pawn)
		{
		}

		// Token: 0x06005D39 RID: 23865 RVA: 0x00204882 File Offset: 0x00202A82
		protected virtual Vector2 GetIconSize(Pawn pawn)
		{
			if (this.GetIconFor(pawn) == null)
			{
				return Vector2.zero;
			}
			return new Vector2((float)this.Width, (float)this.Width);
		}
	}
}
