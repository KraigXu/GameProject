using System;
using Verse;

namespace RimWorld
{
	
	public class RoomOutline
	{
		
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

		
		public RoomOutline(CellRect rect)
		{
			this.rect = rect;
		}

		
		public CellRect rect;
	}
}
