using System;
using RimWorld;

namespace Verse.AI.Group
{
	
	public class LordToil_ExitMapFighting : LordToil_ExitMap
	{
		
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x000F4E64 File Offset: 0x000F3064
		public override DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBestAndDefendSelf;
			}
		}

		
		public LordToil_ExitMapFighting(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false) : base(locomotion, canDig, interruptCurrentJob)
		{
		}
	}
}
