using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class Alert_MonumentMarkerMissingBlueprints : Alert
	{
		
		
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

		
		public Alert_MonumentMarkerMissingBlueprints()
		{
			this.defaultLabel = "MonumentMarkerMissingBlueprints".Translate();
			this.defaultExplanation = "MonumentMarkerMissingBlueprintsDesc".Translate();
		}

		
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
