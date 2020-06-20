using System;
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
	// Token: 0x02000C04 RID: 3076
	public class SitePartWorker_SleepingMechanoids : SitePartWorker
	{
		// Token: 0x06004916 RID: 18710 RVA: 0x0018D244 File Offset: 0x0018B444
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

		// Token: 0x06004917 RID: 18711 RVA: 0x0018D2C8 File Offset: 0x0018B4C8
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int mechanoidsCount = this.GetMechanoidsCount(part.site, part.parms);
			outExtraDescriptionRules.Add(new Rule_String("count", mechanoidsCount.ToString()));
			outExtraDescriptionConstants.Add("count", mechanoidsCount.ToString());
		}

		// Token: 0x06004918 RID: 18712 RVA: 0x0018D320 File Offset: 0x0018B520
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(this.GetMechanoidsCount(site, sitePart.parms), "Enemies".Translate());
		}

		// Token: 0x06004919 RID: 18713 RVA: 0x0018D374 File Offset: 0x0018B574
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, FactionDefOf.Mechanoid.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat));
			return sitePartParams;
		}

		// Token: 0x0600491A RID: 18714 RVA: 0x0018D3A0 File Offset: 0x0018B5A0
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
