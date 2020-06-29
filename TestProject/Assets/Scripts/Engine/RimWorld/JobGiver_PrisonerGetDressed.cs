using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_PrisonerGetDressed : ThinkNode_JobGiver
	{
		
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.guest.PrisonerIsSecure && pawn.apparel != null)
			{
				if (pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0)
				{
					RoyalTitleDef def = pawn.royalty.MostSeniorTitle.def;
					if (def != null && def.requiredApparel != null)
					{
						for (int i = 0; i < def.requiredApparel.Count; i++)
						{
							if (!def.requiredApparel[i].IsMet(pawn))
							{
								Apparel apparel = this.FindGarmentSatisfyingTitleRequirement(pawn, def.requiredApparel[i]);
								if (apparel != null)
								{
									Job job = JobMaker.MakeJob(JobDefOf.Wear, apparel);
									job.ignoreForbidden = true;
									return job;
								}
							}
						}
					}
				}
				if (!pawn.apparel.BodyPartGroupIsCovered(BodyPartGroupDefOf.Legs))
				{
					Apparel apparel2 = this.FindGarmentCoveringPart(pawn, BodyPartGroupDefOf.Legs);
					if (apparel2 != null)
					{
						Job job2 = JobMaker.MakeJob(JobDefOf.Wear, apparel2);
						job2.ignoreForbidden = true;
						return job2;
					}
				}
				if (!pawn.apparel.BodyPartGroupIsCovered(BodyPartGroupDefOf.Torso))
				{
					Apparel apparel3 = this.FindGarmentCoveringPart(pawn, BodyPartGroupDefOf.Torso);
					if (apparel3 != null)
					{
						Job job3 = JobMaker.MakeJob(JobDefOf.Wear, apparel3);
						job3.ignoreForbidden = true;
						return job3;
					}
				}
			}
			return null;
		}

		
		private Apparel FindGarmentCoveringPart(Pawn pawn, BodyPartGroupDef bodyPartGroupDef)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room.isPrisonCell)
			{
				foreach (IntVec3 c in room.Cells)
				{
					List<Thing> thingList = c.GetThingList(pawn.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Apparel apparel = thingList[i] as Apparel;
						if (apparel != null && apparel.def.apparel.bodyPartGroups.Contains(bodyPartGroupDef) && pawn.CanReserve(apparel, 1, -1, null, false) && !apparel.IsBurning() && (!EquipmentUtility.IsBiocoded(apparel) || EquipmentUtility.IsBiocodedFor(apparel, pawn)) && ApparelUtility.HasPartsToWear(pawn, apparel.def))
						{
							return apparel;
						}
					}
				}
			}
			return null;
		}

		
		private Apparel FindGarmentSatisfyingTitleRequirement(Pawn pawn, RoyalTitleDef.ApparelRequirement req)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room.isPrisonCell)
			{
				foreach (IntVec3 c in room.Cells)
				{
					List<Thing> thingList = c.GetThingList(pawn.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Apparel apparel = thingList[i] as Apparel;
						if (apparel != null && req.ApparelMeetsRequirement(thingList[i].def, false) && pawn.CanReserve(apparel, 1, -1, null, false) && !apparel.IsBurning() && (!EquipmentUtility.IsBiocoded(apparel) || EquipmentUtility.IsBiocodedFor(apparel, pawn)) && ApparelUtility.HasPartsToWear(pawn, apparel.def))
						{
							return apparel;
						}
					}
				}
			}
			return null;
		}
	}
}
