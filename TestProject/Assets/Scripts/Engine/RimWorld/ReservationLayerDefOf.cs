using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F96 RID: 3990
	[DefOf]
	public static class ReservationLayerDefOf
	{
		// Token: 0x0600609D RID: 24733 RVA: 0x002171CD File Offset: 0x002153CD
		static ReservationLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ReservationLayerDefOf));
		}

		// Token: 0x04003A51 RID: 14929
		public static ReservationLayerDef Floor;

		// Token: 0x04003A52 RID: 14930
		public static ReservationLayerDef Ceiling;
	}
}
