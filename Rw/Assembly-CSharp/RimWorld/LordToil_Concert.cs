using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A6 RID: 1958
	public class LordToil_Concert : LordToil_Party
	{
		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060032EE RID: 13038 RVA: 0x0011B041 File Offset: 0x00119241
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x0011B049 File Offset: 0x00119249
		public LordToil_Concert(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef, float joyPerTick = 3.5E-05f) : base(spot, gatheringDef, joyPerTick)
		{
			this.organizer = organizer;
		}

		// Token: 0x04001B74 RID: 7028
		private Pawn organizer;
	}
}
