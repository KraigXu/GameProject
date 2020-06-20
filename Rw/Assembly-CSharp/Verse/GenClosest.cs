using System;
using System.Collections;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200043E RID: 1086
	public static class GenClosest
	{
		// Token: 0x06002077 RID: 8311 RVA: 0x000C5FB8 File Offset: 0x000C41B8
		private static bool EarlyOutSearch(IntVec3 start, Map map, ThingRequest thingReq, IEnumerable<Thing> customGlobalSearchSet, Predicate<Thing> validator)
		{
			if (thingReq.group == ThingRequestGroup.Everything)
			{
				Log.Error("Cannot do ClosestThingReachable searching everything without restriction.", false);
				return true;
			}
			if (!start.InBounds(map))
			{
				Log.Error(string.Concat(new object[]
				{
					"Did FindClosestThing with start out of bounds (",
					start,
					"), thingReq=",
					thingReq
				}), false);
				return true;
			}
			return thingReq.group == ThingRequestGroup.Nothing || ((thingReq.IsUndefined || map.listerThings.ThingsMatching(thingReq).Count == 0) && customGlobalSearchSet.EnumerableNullOrEmpty<Thing>());
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x000C604C File Offset: 0x000C424C
		public static Thing ClosestThingReachable(IntVec3 root, Map map, ThingRequest thingReq, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, IEnumerable<Thing> customGlobalSearchSet = null, int searchRegionsMin = 0, int searchRegionsMax = -1, bool forceAllowGlobalSearch = false, RegionType traversableRegionTypes = RegionType.Set_Passable, bool ignoreEntirelyForbiddenRegions = false)
		{
			bool flag = searchRegionsMax < 0 || forceAllowGlobalSearch;
			if (!flag && customGlobalSearchSet != null)
			{
				Log.ErrorOnce("searchRegionsMax >= 0 && customGlobalSearchSet != null && !forceAllowGlobalSearch. customGlobalSearchSet will never be used.", 634984, false);
			}
			if (!flag && !thingReq.IsUndefined && !thingReq.CanBeFoundInRegion)
			{
				Log.ErrorOnce("ClosestThingReachable with thing request group " + thingReq.group + " and global search not allowed. This will never find anything because this group is never stored in regions. Either allow global search or don't call this method at all.", 518498981, false);
				return null;
			}
			if (GenClosest.EarlyOutSearch(root, map, thingReq, customGlobalSearchSet, validator))
			{
				return null;
			}
			Thing thing = null;
			bool flag2 = false;
			if (!thingReq.IsUndefined && thingReq.CanBeFoundInRegion)
			{
				int num = (searchRegionsMax > 0) ? searchRegionsMax : 30;
				int num2;
				thing = GenClosest.RegionwiseBFSWorker(root, map, thingReq, peMode, traverseParams, validator, null, searchRegionsMin, num, maxDistance, out num2, traversableRegionTypes, ignoreEntirelyForbiddenRegions);
				flag2 = (thing == null && num2 < num);
			}
			if (thing == null && flag && !flag2)
			{
				if (traversableRegionTypes != RegionType.Set_Passable)
				{
					Log.ErrorOnce("ClosestThingReachable had to do a global search, but traversableRegionTypes is not set to passable only. It's not supported, because Reachability is based on passable regions only.", 14384767, false);
				}
				Predicate<Thing> validator2 = (Thing t) => map.reachability.CanReach(root, t, peMode, traverseParams) && (validator == null || validator(t));
				IEnumerable<Thing> searchSet = customGlobalSearchSet ?? map.listerThings.ThingsMatching(thingReq);
				thing = GenClosest.ClosestThing_Global(root, searchSet, maxDistance, validator2, null);
			}
			return thing;
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000C61BC File Offset: 0x000C43BC
		public static Thing ClosestThing_Regionwise_ReachablePrioritized(IntVec3 root, Map map, ThingRequest thingReq, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null, int minRegions = 24, int maxRegions = 30)
		{
			if (!thingReq.IsUndefined && !thingReq.CanBeFoundInRegion)
			{
				Log.ErrorOnce("ClosestThing_Regionwise_ReachablePrioritized with thing request group " + thingReq.group + ". This will never find anything because this group is never stored in regions. Most likely a global search should have been used.", 738476712, false);
				return null;
			}
			if (GenClosest.EarlyOutSearch(root, map, thingReq, null, validator))
			{
				return null;
			}
			if (maxRegions < minRegions)
			{
				Log.ErrorOnce("maxRegions < minRegions", 754343, false);
			}
			Thing result = null;
			if (!thingReq.IsUndefined)
			{
				int num;
				result = GenClosest.RegionwiseBFSWorker(root, map, thingReq, peMode, traverseParams, validator, priorityGetter, minRegions, maxRegions, maxDistance, out num, RegionType.Set_Passable, false);
			}
			return result;
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x000C6250 File Offset: 0x000C4450
		public static Thing RegionwiseBFSWorker(IntVec3 root, Map map, ThingRequest req, PathEndMode peMode, TraverseParms traverseParams, Predicate<Thing> validator, Func<Thing, float> priorityGetter, int minRegions, int maxRegions, float maxDistance, out int regionsSeen, RegionType traversableRegionTypes = RegionType.Set_Passable, bool ignoreEntirelyForbiddenRegions = false)
		{
			regionsSeen = 0;
			if (traverseParams.mode == TraverseMode.PassAllDestroyableThings)
			{
				Log.Error("RegionwiseBFSWorker with traverseParams.mode PassAllDestroyableThings. Use ClosestThingGlobal.", false);
				return null;
			}
			if (traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater)
			{
				Log.Error("RegionwiseBFSWorker with traverseParams.mode PassAllDestroyableThingsNotWater. Use ClosestThingGlobal.", false);
				return null;
			}
			if (!req.IsUndefined && !req.CanBeFoundInRegion)
			{
				Log.ErrorOnce("RegionwiseBFSWorker with thing request group " + req.group + ". This group is never stored in regions. Most likely a global search should have been used.", 385766189, false);
				return null;
			}
			Region region = root.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return null;
			}
			float maxDistSquared = maxDistance * maxDistance;
			RegionEntryPredicate entryCondition = (Region from, Region to) => to.Allows(traverseParams, false) && (maxDistance > 5000f || to.extentsClose.ClosestDistSquaredTo(root) < maxDistSquared);
			Thing closestThing = null;
			float closestDistSquared = 9999999f;
			float bestPrio = float.MinValue;
			int regionsSeenScan = 0;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				int regionsSeenScan;
				if (RegionTraverser.ShouldCountRegion(r))
				{
					regionsSeenScan = regionsSeenScan;
					regionsSeenScan++;
				}
				if (!r.IsDoorway && !r.Allows(traverseParams, true))
				{
					return false;
				}
				if (!ignoreEntirelyForbiddenRegions || !r.IsForbiddenEntirely(traverseParams.pawn))
				{
					List<Thing> list = r.ListerThings.ThingsMatching(req);
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing = list[i];
						if (ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, peMode, traverseParams.pawn))
						{
							float num = (priorityGetter != null) ? priorityGetter(thing) : 0f;
							if (num >= bestPrio)
							{
								float num2 = (float)(thing.Position - root).LengthHorizontalSquared;
								if ((num > bestPrio || num2 < closestDistSquared) && num2 < maxDistSquared && (validator == null || validator(thing)))
								{
									closestThing = thing;
									closestDistSquared = num2;
									bestPrio = num;
								}
							}
						}
					}
				}
				return regionsSeenScan >= minRegions && closestThing != null;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
			regionsSeen = regionsSeenScan;
			return closestThing;
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x000C63A8 File Offset: 0x000C45A8
		public static Thing ClosestThing_Global(IntVec3 center, IEnumerable searchSet, float maxDistance = 99999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null)
		{
			GenClosest.<>c__DisplayClass5_0 <>c__DisplayClass5_;
			<>c__DisplayClass5_.center = center;
			<>c__DisplayClass5_.priorityGetter = priorityGetter;
			<>c__DisplayClass5_.validator = validator;
			if (searchSet == null)
			{
				return null;
			}
			<>c__DisplayClass5_.closestDistSquared = 2.14748365E+09f;
			<>c__DisplayClass5_.chosen = null;
			<>c__DisplayClass5_.bestPrio = float.MinValue;
			<>c__DisplayClass5_.maxDistanceSquared = maxDistance * maxDistance;
			IList<Thing> list;
			IList<Pawn> list2;
			IList<Building> list3;
			IList<IAttackTarget> list4;
			if ((list = (searchSet as IList<Thing>)) != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					GenClosest.<ClosestThing_Global>g__Process|5_0(list[i], ref <>c__DisplayClass5_);
				}
			}
			else if ((list2 = (searchSet as IList<Pawn>)) != null)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					GenClosest.<ClosestThing_Global>g__Process|5_0(list2[j], ref <>c__DisplayClass5_);
				}
			}
			else if ((list3 = (searchSet as IList<Building>)) != null)
			{
				for (int k = 0; k < list3.Count; k++)
				{
					GenClosest.<ClosestThing_Global>g__Process|5_0(list3[k], ref <>c__DisplayClass5_);
				}
			}
			else if ((list4 = (searchSet as IList<IAttackTarget>)) != null)
			{
				for (int l = 0; l < list4.Count; l++)
				{
					GenClosest.<ClosestThing_Global>g__Process|5_0((Thing)list4[l], ref <>c__DisplayClass5_);
				}
			}
			else
			{
				foreach (object obj in searchSet)
				{
					GenClosest.<ClosestThing_Global>g__Process|5_0((Thing)obj, ref <>c__DisplayClass5_);
				}
			}
			return <>c__DisplayClass5_.chosen;
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000C651C File Offset: 0x000C471C
		public static Thing ClosestThing_Global_Reachable(IntVec3 center, Map map, IEnumerable<Thing> searchSet, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null)
		{
			GenClosest.<>c__DisplayClass6_0 <>c__DisplayClass6_;
			<>c__DisplayClass6_.center = center;
			<>c__DisplayClass6_.priorityGetter = priorityGetter;
			<>c__DisplayClass6_.map = map;
			<>c__DisplayClass6_.peMode = peMode;
			<>c__DisplayClass6_.traverseParams = traverseParams;
			<>c__DisplayClass6_.validator = validator;
			if (searchSet == null)
			{
				return null;
			}
			<>c__DisplayClass6_.debug_changeCount = 0;
			<>c__DisplayClass6_.debug_scanCount = 0;
			<>c__DisplayClass6_.bestThing = null;
			<>c__DisplayClass6_.bestPrio = float.MinValue;
			<>c__DisplayClass6_.maxDistanceSquared = maxDistance * maxDistance;
			<>c__DisplayClass6_.closestDistSquared = 2.14748365E+09f;
			IList<Thing> list;
			IList<Pawn> list2;
			IList<Building> list3;
			if ((list = (searchSet as IList<Thing>)) != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					GenClosest.<ClosestThing_Global_Reachable>g__Process|6_0(list[i], ref <>c__DisplayClass6_);
				}
			}
			else if ((list2 = (searchSet as IList<Pawn>)) != null)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					GenClosest.<ClosestThing_Global_Reachable>g__Process|6_0(list2[j], ref <>c__DisplayClass6_);
				}
			}
			else if ((list3 = (searchSet as IList<Building>)) != null)
			{
				for (int k = 0; k < list3.Count; k++)
				{
					GenClosest.<ClosestThing_Global_Reachable>g__Process|6_0(list3[k], ref <>c__DisplayClass6_);
				}
			}
			else
			{
				foreach (Thing t in searchSet)
				{
					GenClosest.<ClosestThing_Global_Reachable>g__Process|6_0(t, ref <>c__DisplayClass6_);
				}
			}
			return <>c__DisplayClass6_.bestThing;
		}

		// Token: 0x040013CF RID: 5071
		private const int DefaultLocalTraverseRegionsBeforeGlobal = 30;
	}
}
