﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		
		
		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "MapScatteredWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		
		public const string MapScatteredWithTag = "MapScatteredWith";
	}
}
