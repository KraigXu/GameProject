using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_FillFermentingBarrel : WorkGiver_Scanner
	{
		
		// (get) Token: 0x060030B6 RID: 12470 RVA: 0x001115DD File Offset: 0x0010F7DD
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
			}
		}

		
		// (get) Token: 0x060030B7 RID: 12471 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		
		public static void ResetStaticData()
		{
			WorkGiver_FillFermentingBarrel.TemperatureTrans = "BadTemperature".Translate().ToLower();
			WorkGiver_FillFermentingBarrel.NoWortTrans = "NoWort".Translate();
		}

		
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = t as Building_FermentingBarrel;
			if (building_FermentingBarrel == null || building_FermentingBarrel.Fermented || building_FermentingBarrel.SpaceLeftForWort <= 0)
			{
				return false;
			}
			float ambientTemperature = building_FermentingBarrel.AmbientTemperature;
			CompProperties_TemperatureRuinable compProperties = building_FermentingBarrel.def.GetCompProperties<CompProperties_TemperatureRuinable>();
			if (ambientTemperature < compProperties.minSafeTemperature + 2f || ambientTemperature > compProperties.maxSafeTemperature - 2f)
			{
				JobFailReason.Is(WorkGiver_FillFermentingBarrel.TemperatureTrans, null);
				return false;
			}
			if (t.IsForbidden(pawn) || !pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			if (this.FindWort(pawn, building_FermentingBarrel) == null)
			{
				JobFailReason.Is(WorkGiver_FillFermentingBarrel.NoWortTrans, null);
				return false;
			}
			return !t.IsBurning();
		}

		
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel barrel = (Building_FermentingBarrel)t;
			Thing t2 = this.FindWort(pawn, barrel);
			return JobMaker.MakeJob(JobDefOf.FillFermentingBarrel, t, t2);
		}

		
		private Thing FindWort(Pawn pawn, Building_FermentingBarrel barrel)
		{
			Predicate<Thing> validator = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Wort), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		
		private static string TemperatureTrans;

		
		private static string NoWortTrans;
	}
}
