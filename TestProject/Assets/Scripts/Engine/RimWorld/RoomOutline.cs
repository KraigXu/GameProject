using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A61 RID: 2657
	public class RoomOutline
	{
		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06003EC6 RID: 16070 RVA: 0x0014DC16 File Offset: 0x0014BE16
		public int CellsCountIgnoringWalls
		{
			get
			{
				if (this.rect.Width <= 2 || this.rect.Height <= 2)
				{
					return 0;
				}
				return (this.rect.Width - 2) * (this.rect.Height - 2);
			}
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x0014DC51 File Offset: 0x0014BE51
		public RoomOutline(CellRect rect)
		{
			this.rect = rect;
		}

		// Token: 0x04002493 RID: 9363
		public CellRect rect;
	}
}
