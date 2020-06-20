using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000700 RID: 1792
	public class JoyGiver_ViewArt : JoyGiver
	{
		// Token: 0x06002F64 RID: 12132 RVA: 0x0010AA88 File Offset: 0x00108C88
		public override Job TryGiveJob(Pawn pawn)
		{
			bool allowedOutside = JoyUtility.EnjoyableOutsideNow(pawn, null);
			Job result;
			try
			{
				JoyGiver_ViewArt.candidates.AddRange(pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Art).Where(delegate(Thing thing)
				{
					if (thing.Faction != Faction.OfPlayer || thing.IsForbidden(pawn) || (!allowedOutside && !thing.Position.Roofed(thing.Map)) || !pawn.CanReserveAndReach(thing, PathEndMode.Touch, Danger.None, 1, -1, null, false) || !thing.IsPoliticallyProper(pawn))
					{
						return false;
					}
					CompArt compArt = thing.TryGetComp<CompArt>();
					if (compArt == null)
					{
						Log.Error("No CompArt on thing being considered for viewing: " + thing, false);
						return false;
					}
					if (!compArt.CanShowArt || !compArt.Props.canBeEnjoyedAsArt)
					{
						return false;
					}
					Room room = thing.GetRoom(RegionType.Set_Passable);
					return room != null && ((room.Role != RoomRoleDefOf.Bedroom && room.Role != RoomRoleDefOf.Barracks && room.Role != RoomRoleDefOf.PrisonCell && room.Role != RoomRoleDefOf.PrisonBarracks && room.Role != RoomRoleDefOf.Hospital) || (pawn.ownership != null && pawn.ownership.OwnedRoom != null && pawn.ownership.OwnedRoom == room));
				}));
				Thing t;
				if (!JoyGiver_ViewArt.candidates.TryRandomElementByWeight((Thing target) => Mathf.Max(target.GetStatValue(StatDefOf.Beauty, true), 0.5f), out t))
				{
					result = null;
				}
				else
				{
					result = JobMaker.MakeJob(this.def.jobDef, t);
				}
			}
			finally
			{
				JoyGiver_ViewArt.candidates.Clear();
			}
			return result;
		}

		// Token: 0x04001ABD RID: 6845
		private static List<Thing> candidates = new List<Thing>();
	}
}
