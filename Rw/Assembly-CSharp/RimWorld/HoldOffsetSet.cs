using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000893 RID: 2195
	public class HoldOffsetSet
	{
		// Token: 0x06003554 RID: 13652 RVA: 0x00123584 File Offset: 0x00121784
		public HoldOffset Pick(Rot4 rotation)
		{
			if (rotation == Rot4.North)
			{
				return this.northDefault;
			}
			if (rotation == Rot4.East)
			{
				return this.east;
			}
			if (rotation == Rot4.South)
			{
				return this.south;
			}
			if (rotation == Rot4.West)
			{
				return this.west;
			}
			return null;
		}

		// Token: 0x04001CF2 RID: 7410
		public HoldOffset northDefault;

		// Token: 0x04001CF3 RID: 7411
		public HoldOffset east;

		// Token: 0x04001CF4 RID: 7412
		public HoldOffset south;

		// Token: 0x04001CF5 RID: 7413
		public HoldOffset west;
	}
}
