﻿using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000701 RID: 1793
	public class JoyGiver_VisitGrave : JoyGiver
	{
		// Token: 0x06002F67 RID: 12135 RVA: 0x0010AB58 File Offset: 0x00108D58
		public override Job TryGiveJob(Pawn pawn)
		{
			bool allowedOutside = JoyUtility.EnjoyableOutsideNow(pawn, null);
			Thing t;
			if (!pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Grave).Where(delegate(Thing x)
			{
				Building_Grave building_Grave = (Building_Grave)x;
				return x.Faction == Faction.OfPlayer && building_Grave.HasCorpse && !building_Grave.IsForbidden(pawn) && building_Grave.Corpse.InnerPawn.Faction == Faction.OfPlayer && (allowedOutside || building_Grave.Position.Roofed(building_Grave.Map)) && pawn.CanReserveAndReach(x, PathEndMode.Touch, Danger.None, 1, -1, null, false) && building_Grave.IsPoliticallyProper(pawn);
			}).TryRandomElementByWeight(delegate(Thing x)
			{
				float lengthHorizontal = (x.Position - pawn.Position).LengthHorizontal;
				return Mathf.Max(150f - lengthHorizontal, 5f);
			}, out t))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t);
		}
	}
}
