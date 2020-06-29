using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class Alert_ActivatorCountdown : Alert
	{
		
		public Alert_ActivatorCountdown()
		{
			this.defaultPriority = AlertPriority.High;
		}

		
		
		private List<Thing> ActivatorCountdowns
		{
			get
			{
				this.activatorCountdownsResult.Clear();
				foreach (Map map in Find.Maps)
				{
					if (map.mapPawns.AnyColonistSpawned)
					{
						foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForDef(ThingDefOf.ActivatorCountdown)))
						{
							CompSendSignalOnCountdown compSendSignalOnCountdown = thing.TryGetComp<CompSendSignalOnCountdown>();
							if (compSendSignalOnCountdown != null && compSendSignalOnCountdown.ticksLeft > 0)
							{
								this.activatorCountdownsResult.Add(thing);
							}
						}
					}
				}
				return this.activatorCountdownsResult;
			}
		}

		
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.ActivatorCountdowns);
		}

		
		public override string GetLabel()
		{
			int count = this.ActivatorCountdowns.Count;
			if (count > 1)
			{
				return "ActivatorCountdownMultiple".Translate(count);
			}
			if (count == 0)
			{
				return "";
			}
			CompSendSignalOnCountdown compSendSignalOnCountdown = this.ActivatorCountdowns[0].TryGetComp<CompSendSignalOnCountdown>();
			return "ActivatorCountdown".Translate(compSendSignalOnCountdown.ticksLeft.ToStringTicksToPeriod(true, false, true, true));
		}

		
		public override TaggedString GetExplanation()
		{
			int num = this.ActivatorCountdowns.Count<Thing>();
			if (num > 1)
			{
				return "ActivatorCountdownDescMultiple".Translate(num);
			}
			if (num == 0)
			{
				return "";
			}
			return "ActivatorCountdownDesc".Translate();
		}

		
		private List<Thing> activatorCountdownsResult = new List<Thing>();
	}
}
