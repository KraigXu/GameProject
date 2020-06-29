using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ChemicalDef : Def
	{
		
		public override IEnumerable<string> ConfigErrors()
		{

			IEnumerator<string> enumerator = null;
			if (this.addictionHediff == null)
			{
				yield return "addictionHediff is null";
			}
			yield break;
			yield break;
		}

		
		public HediffDef addictionHediff;

		
		public HediffDef toleranceHediff;

		
		public bool canBinge = true;

		
		public float onGeneratedAddictedToleranceChance;

		
		public List<HediffGiver_Event> onGeneratedAddictedEvents;
	}
}
