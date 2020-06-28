using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E05 RID: 3589
	public class Alert_MonumentMarkerMissingBlueprints : Alert
	{
		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x060056BF RID: 22207 RVA: 0x001CC384 File Offset: 0x001CA584
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (!monumentMarker.complete)
						{
							SketchEntity firstEntityWithMissingBlueprint = monumentMarker.FirstEntityWithMissingBlueprint;
							if (firstEntityWithMissingBlueprint != null)
							{
								this.targets.Add(new GlobalTargetInfo(firstEntityWithMissingBlueprint.pos + monumentMarker.Position, maps[i], false));
							}
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x060056C0 RID: 22208 RVA: 0x001CC433 File Offset: 0x001CA633
		public Alert_MonumentMarkerMissingBlueprints()
		{
			this.defaultLabel = "MonumentMarkerMissingBlueprints".Translate();
			this.defaultExplanation = "MonumentMarkerMissingBlueprintsDesc".Translate();
		}

		// Token: 0x060056C1 RID: 22209 RVA: 0x001CC470 File Offset: 0x001CA670
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04002F43 RID: 12099
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
