using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005D7 RID: 1495
	public class LordToil_ExitMapFighting : LordToil_ExitMap
	{
		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x000F4E64 File Offset: 0x000F3064
		public override DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBestAndDefendSelf;
			}
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x000F4E6B File Offset: 0x000F306B
		public LordToil_ExitMapFighting(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false) : base(locomotion, canDig, interruptCurrentJob)
		{
		}
	}
}
