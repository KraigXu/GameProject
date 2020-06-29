using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class PawnColumnWorker_Icon : PawnColumnWorker
	{
		
		// (get) Token: 0x06005D2C RID: 23852 RVA: 0x00204700 File Offset: 0x00202900
		protected virtual int Width
		{
			get
			{
				return 26;
			}
		}

		
		// (get) Token: 0x06005D2D RID: 23853 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		protected virtual int Padding
		{
			get
			{
				return 2;
			}
		}

		
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

		
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(this.GetIconSize(pawn).y));
		}

		
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		
		private int GetValueToCompare(Pawn pawn)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			if (!(iconFor != null))
			{
				return int.MinValue;
			}
			return iconFor.GetInstanceID();
		}

		
		protected abstract Texture2D GetIconFor(Pawn pawn);

		
		protected virtual string GetIconTip(Pawn pawn)
		{
			return null;
		}

		
		protected virtual Color GetIconColor(Pawn pawn)
		{
			return Color.white;
		}

		
		protected virtual void ClickedIcon(Pawn pawn)
		{
		}

		
		protected virtual void PaintedIcon(Pawn pawn)
		{
		}

		
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
