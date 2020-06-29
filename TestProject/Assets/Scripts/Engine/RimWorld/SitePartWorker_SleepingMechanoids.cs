﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Grammar;

namespace RimWorld
{
	
	public class SitePartWorker_SleepingMechanoids : SitePartWorker
	{
		
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			IEnumerable<Pawn> source = from x in map.mapPawns.AllPawnsSpawned
			where x.RaceProps.IsMechanoid
			select x;
			Pawn pawn = (from x in source
			where x.GetLord() != null && x.GetLord().LordJob is LordJob_SleepThenAssaultColony
			select x).FirstOrDefault<Pawn>();
			if (pawn == null)
			{
				pawn = source.FirstOrDefault<Pawn>();
			}
			lookTargets = pawn;
			return arrivedLetterPart;
		}

		
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int mechanoidsCount = this.GetMechanoidsCount(part.site, part.parms);
			outExtraDescriptionRules.Add(new Rule_String("count", mechanoidsCount.ToString()));
			outExtraDescriptionConstants.Add("count", mechanoidsCount.ToString());
		}

		
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(this.GetMechanoidsCount(site, sitePart.parms), "Enemies".Translate());
		}

		
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, FactionDefOf.Mechanoid.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat));
			return sitePartParams;
		}

		
		private int GetMechanoidsCount(Site site, SitePartParams parms)
		{
			return PawnGroupMakerUtility.GeneratePawnKindsExample(new PawnGroupMakerParms
			{
				tile = site.Tile,
				faction = Faction.OfMechanoids,
				groupKind = PawnGroupKindDefOf.Combat,
				points = parms.threatPoints,
				seed = new int?(SleepingMechanoidsSitePartUtility.GetPawnGroupMakerSeed(parms))
			}).Count<PawnKindDef>();
		}
	}
}
