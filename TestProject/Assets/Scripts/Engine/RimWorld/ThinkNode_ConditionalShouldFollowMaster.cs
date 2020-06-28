﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D6 RID: 2006
	public class ThinkNode_ConditionalShouldFollowMaster : ThinkNode_Conditional
	{
		// Token: 0x0600339E RID: 13214 RVA: 0x0011DABB File Offset: 0x0011BCBB
		protected override bool Satisfied(Pawn pawn)
		{
			return ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn);
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x0011DAC4 File Offset: 0x0011BCC4
		public static bool ShouldFollowMaster(Pawn pawn)
		{
			if (!pawn.Spawned || pawn.playerSettings == null)
			{
				return false;
			}
			Pawn respectedMaster = pawn.playerSettings.RespectedMaster;
			if (respectedMaster == null)
			{
				return false;
			}
			if (respectedMaster.Spawned)
			{
				if (pawn.playerSettings.followDrafted && respectedMaster.Drafted && pawn.CanReach(respectedMaster, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return true;
				}
				if (pawn.playerSettings.followFieldwork && respectedMaster.mindState.lastJobTag == JobTag.Fieldwork && pawn.CanReach(respectedMaster, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			else
			{
				Pawn carriedBy = respectedMaster.CarriedBy;
				if (carriedBy != null && carriedBy.HostileTo(respectedMaster) && pawn.CanReach(carriedBy, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
