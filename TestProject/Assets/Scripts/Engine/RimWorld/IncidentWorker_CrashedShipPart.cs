using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class IncidentWorker_CrashedShipPart : IncidentWorker
	{
		
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ((Map)parms.target).listerThings.ThingsOfDef(this.def.mechClusterBuilding).Count <= 0;
		}

		
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			//IncidentWorker_CrashedShipPart.c__DisplayClass4_0 c__DisplayClass4_ = new IncidentWorker_CrashedShipPart.c__DisplayClass4_0();
			//c__DisplayClass4_.map = (Map)parms.target;
			//List<TargetInfo> list = new List<TargetInfo>();
			//c__DisplayClass4_.shipPartDef = this.def.mechClusterBuilding;
			//IntVec3 intVec = IncidentWorker_CrashedShipPart.FindDropPodLocation(c__DisplayClass4_.map, (IntVec3 spot) => c__DisplayClass4_.<TryExecuteWorker>g__CanPlaceAt|0(spot));
			//if (intVec == IntVec3.Invalid)
			//{
			//	return false;
			//}
			//float points = Mathf.Max(parms.points * 0.9f, 300f);
			//List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			//{
			//	groupKind = PawnGroupKindDefOf.Combat,
			//	tile = c__DisplayClass4_.map.Tile,
			//	faction = Faction.OfMechanoids,
			//	points = points
			//}, true).ToList<Pawn>();
			//Thing thing = ThingMaker.MakeThing(c__DisplayClass4_.shipPartDef, null);
			//thing.SetFaction(Faction.OfMechanoids, null);
			//LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_SleepThenMechanoidsDefend(new List<Thing>
			//{
			//	thing
			//}, Faction.OfMechanoids, 28f, intVec, false, false), c__DisplayClass4_.map, list2);
			//DropPodUtility.DropThingsNear(intVec, c__DisplayClass4_.map, list2.Cast<Thing>(), 110, false, false, true, true);
			//foreach (Pawn thing2 in list2)
			//{
			//	CompCanBeDormant compCanBeDormant = thing2.TryGetComp<CompCanBeDormant>();
			//	if (compCanBeDormant != null)
			//	{
			//		compCanBeDormant.ToSleep();
			//	}
			//}
			//list.AddRange(from p in list2
			//select new TargetInfo(p));
			//GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(ThingDefOf.CrashedShipPartIncoming, thing), intVec, c__DisplayClass4_.map, WipeMode.Vanish);
			//list.Add(new TargetInfo(intVec, c__DisplayClass4_.map, false));
			//base.SendStandardLetter(parms, list, Array.Empty<NamedArgument>());
			return true;
		}

		
		private static IntVec3 FindDropPodLocation(Map map, Predicate<IntVec3> validator)
		{
			for (int i = 0; i < 200; i++)
			{
				IntVec3 intVec = RCellFinder.FindSiegePositionFrom_NewTemp(DropCellFinder.FindRaidDropCenterDistant_NewTemp(map, true), map, true);
				if (validator(intVec))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		
		private const float ShipPointsFactor = 0.9f;

		
		private const int IncidentMinimumPoints = 300;

		
		private const float DefendRadius = 28f;
	}
}
