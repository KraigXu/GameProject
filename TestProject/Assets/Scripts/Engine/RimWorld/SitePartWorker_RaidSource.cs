using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C03 RID: 3075
	public class SitePartWorker_RaidSource : SitePartWorker_Outpost
	{
		// Token: 0x06004912 RID: 18706 RVA: 0x0018D0B4 File Offset: 0x0018B2B4
		public override void SitePartWorkerTick(SitePart sitePart)
		{
			base.SitePartWorkerTick(sitePart);
			if (sitePart.lastRaidTick == -1 || (float)Find.TickManager.TicksGame > (float)sitePart.lastRaidTick + 90000f)
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome && sitePart.site.IsHashIntervalTick(2500) && Rand.MTBEventOccurs(QuestTuning.PointsToRaidSourceRaidsMTBDaysCurve.Evaluate(sitePart.parms.threatPoints), 60000f, 2500f))
					{
						this.StartRaid(maps[i], sitePart);
					}
				}
			}
		}

		// Token: 0x06004913 RID: 18707 RVA: 0x0018D158 File Offset: 0x0018B358
		private void StartRaid(Map map, SitePart sitePart)
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, map);
			incidentParms.forced = true;
			incidentParms.points *= 0.6f;
			if (IncidentDefOf.RaidEnemy.Worker.CanFireNow(incidentParms, false))
			{
				IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
				sitePart.lastRaidTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06004914 RID: 18708 RVA: 0x0018D1C0 File Offset: 0x0018B3C0
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int enemiesCount = base.GetEnemiesCount(part.site, part.parms);
			float num = QuestTuning.PointsToRaidSourceRaidsMTBDaysCurve.Evaluate(part.parms.threatPoints);
			outExtraDescriptionRules.Add(new Rule_String("enemiesCount", enemiesCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("mtbDays", ((int)(num * 60000f)).ToStringTicksToPeriod(true, false, false, true)));
		}
	}
}
