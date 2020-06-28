using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B3D RID: 2877
	public class RecordWorker_TimeOnFire : RecordWorker
	{
		// Token: 0x060043B1 RID: 17329 RVA: 0x0016C798 File Offset: 0x0016A998
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsBurning();
		}
	}
}
