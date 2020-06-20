using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003A5 RID: 933
	public abstract class Listing
	{
		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001B72 RID: 7026 RVA: 0x000A83B6 File Offset: 0x000A65B6
		public float CurHeight
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001B74 RID: 7028 RVA: 0x000A83CE File Offset: 0x000A65CE
		// (set) Token: 0x06001B73 RID: 7027 RVA: 0x000A83BE File Offset: 0x000A65BE
		public float ColumnWidth
		{
			get
			{
				return this.columnWidthInt;
			}
			set
			{
				this.columnWidthInt = value;
				this.hasCustomColumnWidth = true;
			}
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x000A83D6 File Offset: 0x000A65D6
		public void NewColumn()
		{
			this.curY = 0f;
			this.curX += this.ColumnWidth + 17f;
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x000A83FC File Offset: 0x000A65FC
		protected void NewColumnIfNeeded(float neededHeight)
		{
			if (this.maxOneColumn)
			{
				return;
			}
			if (this.curY + neededHeight > this.listingRect.height)
			{
				this.NewColumn();
			}
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x000A8422 File Offset: 0x000A6622
		public Rect GetRect(float height)
		{
			this.NewColumnIfNeeded(height);
			Rect result = new Rect(this.curX, this.curY, this.ColumnWidth, height);
			this.curY += height;
			return result;
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x000A8451 File Offset: 0x000A6651
		public void Gap(float gapHeight = 12f)
		{
			this.curY += gapHeight;
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x000A8464 File Offset: 0x000A6664
		public void GapLine(float gapHeight = 12f)
		{
			float y = this.curY + gapHeight / 2f;
			Color color = GUI.color;
			GUI.color = color * new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(this.curX, y, this.ColumnWidth);
			GUI.color = color;
			this.curY += gapHeight;
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x000A84D0 File Offset: 0x000A66D0
		public virtual void Begin(Rect rect)
		{
			this.listingRect = rect;
			if (this.hasCustomColumnWidth)
			{
				if (this.columnWidthInt > this.listingRect.width)
				{
					Log.Error(string.Concat(new object[]
					{
						"Listing set ColumnWith to ",
						this.columnWidthInt,
						" which is more than the whole listing rect width of ",
						this.listingRect.width,
						". Clamping."
					}), false);
					this.columnWidthInt = this.listingRect.width;
				}
			}
			else
			{
				this.columnWidthInt = this.listingRect.width;
			}
			this.curX = 0f;
			this.curY = 0f;
			GUI.BeginGroup(rect);
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x000A8589 File Offset: 0x000A6789
		public virtual void End()
		{
			GUI.EndGroup();
		}

		// Token: 0x0400103C RID: 4156
		public float verticalSpacing = 2f;

		// Token: 0x0400103D RID: 4157
		protected Rect listingRect;

		// Token: 0x0400103E RID: 4158
		protected float curY;

		// Token: 0x0400103F RID: 4159
		protected float curX;

		// Token: 0x04001040 RID: 4160
		private float columnWidthInt;

		// Token: 0x04001041 RID: 4161
		private bool hasCustomColumnWidth;

		// Token: 0x04001042 RID: 4162
		public bool maxOneColumn;

		// Token: 0x04001043 RID: 4163
		public const float ColumnSpacing = 17f;

		// Token: 0x04001044 RID: 4164
		public const float DefaultGap = 12f;
	}
}
