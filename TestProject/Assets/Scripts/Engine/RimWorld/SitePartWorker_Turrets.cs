using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGenNew;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class SitePartWorker_Turrets : SitePartWorker
	{
		
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

		
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.mortarsCount = Rand.RangeInclusive(0, 1);
			sitePartParams.turretsCount = Mathf.Clamp(Mathf.RoundToInt(sitePartParams.threatPoints / ThingDefOf.Turret_MiniTurret.building.combatPower), 2, 11);
			return sitePartParams;
		}

		
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string threatsInfo = this.GetThreatsInfo(part.parms);
			outExtraDescriptionRules.Add(new Rule_String("threatsInfo", threatsInfo));
		}

		
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + this.GetThreatsInfo(sitePart.parms);
		}

		
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

		
		private const int MinTurrets = 2;

		
		private const int MaxTurrets = 11;

		
		private List<string> threatsTmp = new List<string>();
	}
}
