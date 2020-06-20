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
	// Token: 0x02000C06 RID: 3078
	public class SitePartWorker_Turrets : SitePartWorker
	{
		// Token: 0x0600491D RID: 18717 RVA: 0x0018D3FC File Offset: 0x0018B5FC
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			Thing t;
			if ((t = map.listerThings.AllThings.FirstOrDefault((Thing x) => x is Building_TurretGun && x.HostileTo(Faction.OfPlayer))) == null)
			{
				t = map.listerThings.AllThings.FirstOrDefault((Thing x) => x is Building_TurretGun);
			}
			lookTargets = t;
			return arrivedLetterPart;
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x0018D47C File Offset: 0x0018B67C
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.mortarsCount = Rand.RangeInclusive(0, 1);
			sitePartParams.turretsCount = Mathf.Clamp(Mathf.RoundToInt(sitePartParams.threatPoints / ThingDefOf.Turret_MiniTurret.building.combatPower), 2, 11);
			return sitePartParams;
		}

		// Token: 0x0600491F RID: 18719 RVA: 0x0018D4C8 File Offset: 0x0018B6C8
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string threatsInfo = this.GetThreatsInfo(part.parms);
			outExtraDescriptionRules.Add(new Rule_String("threatsInfo", threatsInfo));
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x0018D4FE File Offset: 0x0018B6FE
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + this.GetThreatsInfo(sitePart.parms);
		}

		// Token: 0x06004921 RID: 18721 RVA: 0x0018D520 File Offset: 0x0018B720
		private string GetThreatsInfo(SitePartParams parms)
		{
			this.threatsTmp.Clear();
			int num = parms.mortarsCount + 1;
			if (parms.turretsCount != 0)
			{
				string value;
				if (parms.turretsCount == 1)
				{
					value = "Turret".Translate();
				}
				else
				{
					value = "Turrets".Translate();
				}
				this.threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(parms.turretsCount.ToString(), value));
			}
			if (parms.mortarsCount != 0)
			{
				string value;
				if (parms.mortarsCount == 1)
				{
					value = "Mortar".Translate();
				}
				else
				{
					value = "Mortars".Translate();
				}
				this.threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(parms.mortarsCount.ToString(), value));
			}
			if (num != 0)
			{
				string value;
				if (num == 1)
				{
					value = "Enemy".Translate();
				}
				else
				{
					value = "Enemies".Translate();
				}
				this.threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(num.ToString(), value));
			}
			return this.threatsTmp.ToCommaList(true);
		}

		// Token: 0x040029CD RID: 10701
		private const int MinTurrets = 2;

		// Token: 0x040029CE RID: 10702
		private const int MaxTurrets = 11;

		// Token: 0x040029CF RID: 10703
		private List<string> threatsTmp = new List<string>();
	}
}
