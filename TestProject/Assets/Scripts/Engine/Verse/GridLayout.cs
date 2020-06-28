using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000397 RID: 919
	public class GridLayout
	{
		// Token: 0x06001B05 RID: 6917 RVA: 0x000A6024 File Offset: 0x000A4224
		public GridLayout(Rect container, int cols = 1, int rows = 1, float outerPadding = 4f, float innerPadding = 4f)
		{
			this.container = new Rect(container);
			this.cols = cols;
			this.innerPadding = innerPadding;
			this.outerPadding = outerPadding;
			float num = container.width - outerPadding * 2f - (float)(cols - 1) * innerPadding;
			float num2 = container.height - outerPadding * 2f - (float)(rows - 1) * innerPadding;
			this.colWidth = num / (float)cols;
			this.rowHeight = num2 / (float)rows;
			this.colStride = this.colWidth + innerPadding;
			this.rowStride = this.rowHeight + innerPadding;
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x000A60C0 File Offset: 0x000A42C0
		public GridLayout(float colWidth, float rowHeight, int cols, int rows, float outerPadding = 4f, float innerPadding = 4f)
		{
			this.colWidth = colWidth;
			this.rowHeight = rowHeight;
			this.cols = cols;
			this.innerPadding = innerPadding;
			this.outerPadding = outerPadding;
			this.colStride = colWidth + innerPadding;
			this.rowStride = rowHeight + innerPadding;
			this.container = new Rect(0f, 0f, outerPadding * 2f + colWidth * (float)cols + innerPadding * (float)cols - 1f, outerPadding * 2f + rowHeight * (float)rows + innerPadding * (float)rows - 1f);
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x000A6158 File Offset: 0x000A4358
		public Rect GetCellRectByIndex(int index, int colspan = 1, int rowspan = 1)
		{
			int col = index % this.cols;
			int row = index / this.cols;
			return this.GetCellRect(col, row, colspan, rowspan);
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x000A6184 File Offset: 0x000A4384
		public Rect GetCellRect(int col, int row, int colspan = 1, int rowspan = 1)
		{
			return new Rect(Mathf.Floor(this.container.x + this.outerPadding + (float)col * this.colStride), Mathf.Floor(this.container.y + this.outerPadding + (float)row * this.rowStride), Mathf.Ceil(this.colWidth) * (float)colspan + this.innerPadding * (float)(colspan - 1), Mathf.Ceil(this.rowHeight) * (float)rowspan + this.innerPadding * (float)(rowspan - 1));
		}

		// Token: 0x0400100E RID: 4110
		public Rect container;

		// Token: 0x0400100F RID: 4111
		private int cols;

		// Token: 0x04001010 RID: 4112
		private float outerPadding;

		// Token: 0x04001011 RID: 4113
		private float innerPadding;

		// Token: 0x04001012 RID: 4114
		private float colStride;

		// Token: 0x04001013 RID: 4115
		private float rowStride;

		// Token: 0x04001014 RID: 4116
		private float colWidth;

		// Token: 0x04001015 RID: 4117
		private float rowHeight;
	}
}
