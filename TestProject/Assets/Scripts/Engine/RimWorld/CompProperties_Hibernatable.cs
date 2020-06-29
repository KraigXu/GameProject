using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Hibernatable : CompProperties
	{
		
		public CompProperties_Hibernatable()
		{
			this.compClass = typeof(CompHibernatable);
		}

		
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in this.ConfigErrors(parentDef))
			{
				
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Concat(new object[]
				{
					"CompHibernatable needs tickerType ",
					TickerType.Normal,
					", has ",
					parentDef.tickerType
				});
			}
			yield break;
			yield break;
		}

		
		public float startupDays = 14f;

		
		public IncidentTargetTagDef incidentTargetWhileStarting;
	}
}
