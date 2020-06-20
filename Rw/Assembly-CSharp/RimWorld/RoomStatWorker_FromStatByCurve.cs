using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A9C RID: 2716
	public class RoomStatWorker_FromStatByCurve : RoomStatWorker
	{
		// Token: 0x06003FFF RID: 16383 RVA: 0x00154754 File Offset: 0x00152954
		public override float GetScore(Room room)
		{
			return this.def.curve.Evaluate(room.GetStat(this.def.inputStat));
		}
	}
}
