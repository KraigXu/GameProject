using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class SitePartWorker_MechCluster : SitePartWorker
	{
		
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			List<Thing> list = new List<Thing>();
			foreach (Thing thing in map.listerThings.AllThings)
			{
				if ((thing.def.building != null && thing.def.building.buildingTags != null && thing.def.building.buildingTags.Contains("MechClusterMember")) || (thing is Pawn && thing.def.race.IsMechanoid))
				{
					list.Add(thing);
				}
			}
			lookTargets = new LookTargets(list);
			return arrivedLetterPart;
		}

		
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, 750f);
			return sitePartParams;
		}

		
		public const float MinPoints = 750f;
	}
}
