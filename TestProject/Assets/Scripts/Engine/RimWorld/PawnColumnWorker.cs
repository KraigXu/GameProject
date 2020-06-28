using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020008E7 RID: 2279
	[StaticConstructorOnStartup]
	public abstract class PawnColumnWorker
	{
		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06003687 RID: 13959 RVA: 0x00017A00 File Offset: 0x00015C00
		protected virtual Color DefaultHeaderColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06003688 RID: 13960 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Small;
			}
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x001276F0 File Offset: 0x001258F0
		public virtual void DoHeader(Rect rect, PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				GUI.color = this.DefaultHeaderColor;
				Text.Anchor = TextAnchor.LowerCenter;
				Rect rect2 = rect;
				rect2.y += 3f;
				Widgets.Label(rect2, this.def.LabelCap.Resolve().Truncate(rect.width, null));
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				Text.Font = GameFont.Small;
			}
			else if (this.def.HeaderIcon != null)
			{
				Vector2 headerIconSize = this.def.HeaderIconSize;
				int num = (int)((rect.width - headerIconSize.x) / 2f);
				GUI.DrawTexture(new Rect(rect.x + (float)num, rect.yMax - headerIconSize.y, headerIconSize.x, headerIconSize.y).ContractedBy(2f), this.def.HeaderIcon);
			}
			if (table.SortingBy == this.def)
			{
				Texture2D texture2D = table.SortingDescending ? PawnColumnWorker.SortingDescendingIcon : PawnColumnWorker.SortingIcon;
				GUI.DrawTexture(new Rect(rect.xMax - (float)texture2D.width - 1f, rect.yMax - (float)texture2D.height - 1f, (float)texture2D.width, (float)texture2D.height), texture2D);
			}
			if (this.def.HeaderInteractable)
			{
				Rect interactableHeaderRect = this.GetInteractableHeaderRect(rect, table);
				if (Mouse.IsOver(interactableHeaderRect))
				{
					Widgets.DrawHighlight(interactableHeaderRect);
					string headerTip = this.GetHeaderTip(table);
					if (!headerTip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(interactableHeaderRect, headerTip);
					}
				}
				if (Widgets.ButtonInvisible(interactableHeaderRect, true))
				{
					this.HeaderClicked(rect, table);
				}
			}
		}

		// Token: 0x0600368A RID: 13962
		public abstract void DoCell(Rect rect, Pawn pawn, PawnTable table);

		// Token: 0x0600368B RID: 13963 RVA: 0x001278BC File Offset: 0x00125ABC
		public virtual int GetMinWidth(PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				int result = Mathf.CeilToInt(Text.CalcSize(this.def.LabelCap).x);
				Text.Font = GameFont.Small;
				return result;
			}
			if (this.def.HeaderIcon != null)
			{
				return Mathf.CeilToInt(this.def.HeaderIconSize.x);
			}
			return 1;
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x00127936 File Offset: 0x00125B36
		public virtual int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x0012793D File Offset: 0x00125B3D
		public virtual int GetOptimalWidth(PawnTable table)
		{
			return this.GetMinWidth(table);
		}

		// Token: 0x0600368E RID: 13966 RVA: 0x00127946 File Offset: 0x00125B46
		public virtual int GetMinCellHeight(Pawn pawn)
		{
			return 30;
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x0012794C File Offset: 0x00125B4C
		public virtual int GetMinHeaderHeight(PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				int result = Mathf.CeilToInt(Text.CalcSize(this.def.LabelCap).y);
				Text.Font = GameFont.Small;
				return result;
			}
			if (this.def.HeaderIcon != null)
			{
				return Mathf.CeilToInt(this.def.HeaderIconSize.y);
			}
			return 0;
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual int Compare(Pawn a, Pawn b)
		{
			return 0;
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x001279C8 File Offset: 0x00125BC8
		protected virtual Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
		{
			float num = Mathf.Min(25f, headerRect.height);
			return new Rect(headerRect.x, headerRect.yMax - num, headerRect.width, num);
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x00127A04 File Offset: 0x00125C04
		protected virtual void HeaderClicked(Rect headerRect, PawnTable table)
		{
			if (this.def.sortable && !Event.current.shift)
			{
				if (Event.current.button == 0)
				{
					if (table.SortingBy != this.def)
					{
						table.SortBy(this.def, true);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						return;
					}
					if (table.SortingDescending)
					{
						table.SortBy(this.def, false);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						return;
					}
					table.SortBy(null, false);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					return;
				}
				else if (Event.current.button == 1)
				{
					if (table.SortingBy != this.def)
					{
						table.SortBy(this.def, false);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						return;
					}
					if (table.SortingDescending)
					{
						table.SortBy(null, false);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
						return;
					}
					table.SortBy(this.def, true);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x00127B00 File Offset: 0x00125D00
		protected virtual string GetHeaderTip(PawnTable table)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.def.headerTip.NullOrEmpty())
			{
				stringBuilder.Append(this.def.headerTip);
			}
			if (this.def.sortable)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("ClickToSortByThisColumn".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001F1F RID: 7967
		public PawnColumnDef def;

		// Token: 0x04001F20 RID: 7968
		protected const int DefaultCellHeight = 30;

		// Token: 0x04001F21 RID: 7969
		private static readonly Texture2D SortingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Sorting", true);

		// Token: 0x04001F22 RID: 7970
		private static readonly Texture2D SortingDescendingIcon = ContentFinder<Texture2D>.Get("UI/Icons/SortingDescending", true);

		// Token: 0x04001F23 RID: 7971
		private const int IconMargin = 2;
	}
}
