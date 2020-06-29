using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_ThreatsGenerator : QuestPartActivable, IIncidentMakerQuestPart
	{
		
		public IEnumerable<FiringIncident> MakeIntervalIncidents()
		{
			if (this.mapParent != null && this.mapParent.HasMap)
			{
				return ThreatsGenerator.MakeIntervalIncidents(this.parms, this.mapParent.Map, base.EnableTick + this.threatStartTicks);
			}
			return Enumerable.Empty<FiringIncident>();
		}

		
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "Log future incidents from " + base.GetType().Name, true, true, true))
			{
				StorytellerUtility.DebugLogTestFutureIncidents(false, null, this, 50);
			}
			curY += rect.height + 4f;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThreatsGeneratorParams>(ref this.parms, "parms", Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.threatStartTicks, "threatStartTicks", 0, false);
		}

		
		public ThreatsGeneratorParams parms;

		
		public MapParent mapParent;

		
		public int threatStartTicks;
	}
}
