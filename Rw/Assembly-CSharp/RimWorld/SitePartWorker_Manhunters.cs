using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000BFF RID: 3071
	public class SitePartWorker_Manhunters : SitePartWorker
	{
		// Token: 0x06004902 RID: 18690 RVA: 0x0018CCBC File Offset: 0x0018AEBC
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = (from x in map.mapPawns.AllPawnsSpawned
			where x.MentalStateDef == MentalStateDefOf.Manhunter || x.MentalStateDef == MentalStateDefOf.ManhunterPermanent
			select x).FirstOrDefault<Pawn>();
			return arrivedLetterPart;
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x0018CD10 File Offset: 0x0018AF10
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			if (ManhunterPackGenStepUtility.TryGetAnimalsKind(sitePartParams.threatPoints, tile, out sitePartParams.animalKind))
			{
				sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, sitePartParams.animalKind.combatPower);
			}
			return sitePartParams;
		}

		// Token: 0x06004904 RID: 18692 RVA: 0x0018CD58 File Offset: 0x0018AF58
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int animalsCount = this.GetAnimalsCount(part.parms);
			string output = GenLabel.BestKindLabel(part.parms.animalKind, Gender.None, true, animalsCount);
			outExtraDescriptionRules.Add(new Rule_String("count", animalsCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("kindLabel", output));
			outExtraDescriptionConstants.Add("count", animalsCount.ToString());
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x0018CDCC File Offset: 0x0018AFCC
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			int animalsCount = this.GetAnimalsCount(sitePart.parms);
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(animalsCount.ToString(), GenLabel.BestKindLabel(sitePart.parms.animalKind, Gender.None, true, animalsCount));
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x0018CE30 File Offset: 0x0018B030
		private int GetAnimalsCount(SitePartParams parms)
		{
			return ManhunterPackIncidentUtility.GetAnimalsCount(parms.animalKind, parms.threatPoints);
		}
	}
}
