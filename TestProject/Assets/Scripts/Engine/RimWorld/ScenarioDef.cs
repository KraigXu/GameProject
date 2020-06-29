using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ScenarioDef : Def
	{
		
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.scenario.name.NullOrEmpty())
			{
				this.scenario.name = this.label;
			}
			if (this.scenario.description.NullOrEmpty())
			{
				this.scenario.description = this.description;
			}
			this.scenario.Category = ScenarioCategory.FromDef;
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.scenario == null)
			{
				yield return "null scenario";
			}
			foreach (string text in this.scenario.ConfigErrors())
			{
				
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		
		public Scenario scenario;
	}
}
