using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF0 RID: 3568
	public class Alert_ActivatorCountdown : Alert
	{
		// Token: 0x06005670 RID: 22128 RVA: 0x001CA8D4 File Offset: 0x001C8AD4
		public Alert_ActivatorCountdown()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06005671 RID: 22129 RVA: 0x001CA8F0 File Offset: 0x001C8AF0
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

		// Token: 0x06005672 RID: 22130 RVA: 0x001CA9C8 File Offset: 0x001C8BC8
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.ActivatorCountdowns);
		}

		// Token: 0x06005673 RID: 22131 RVA: 0x001CA9E4 File Offset: 0x001C8BE4
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

		// Token: 0x06005674 RID: 22132 RVA: 0x001CAA58 File Offset: 0x001C8C58
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

		// Token: 0x04002F2F RID: 12079
		private List<Thing> activatorCountdownsResult = new List<Thing>();
	}
}
