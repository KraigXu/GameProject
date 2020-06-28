using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C00 RID: 3072
	public class SitePartWorker_MechCluster : SitePartWorker
	{
		// Token: 0x06004908 RID: 18696 RVA: 0x0018CE44 File Offset: 0x0018B044
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

		// Token: 0x06004909 RID: 18697 RVA: 0x0018CF0C File Offset: 0x0018B10C
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, 750f);
			return sitePartParams;
		}

		// Token: 0x040029CC RID: 10700
		public const float MinPoints = 750f;
	}
}
