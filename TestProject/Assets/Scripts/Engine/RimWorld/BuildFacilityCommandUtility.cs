﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public static class BuildFacilityCommandUtility
	{
		
		public static IEnumerable<Command> BuildFacilityCommands(BuildableDef building)
		{
			ThingDef thingDef = building as ThingDef;
			if (thingDef == null)
			{
				yield break;
			}
			CompProperties_AffectedByFacilities affectedByFacilities = thingDef.GetCompProperties<CompProperties_AffectedByFacilities>();
			if (affectedByFacilities == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < affectedByFacilities.linkableFacilities.Count; i = num + 1)
			{
				Designator_Build designator_Build = BuildCopyCommandUtility.FindAllowedDesignator(affectedByFacilities.linkableFacilities[i], true);
				if (designator_Build != null)
				{
					yield return designator_Build;
				}
				num = i;
			}
			yield break;
		}
	}
}
