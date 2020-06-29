using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Rottable : CompProperties
	{
		
		
		public int TicksToRotStart
		{
			get
			{
				return Mathf.RoundToInt(this.daysToRotStart * 60000f);
			}
		}

		
		
		public int TicksToDessicated
		{
			get
			{
				return Mathf.RoundToInt(this.daysToDessicated * 60000f);
			}
		}

		
		public CompProperties_Rottable()
		{
			this.compClass = typeof(CompRottable);
		}

		
		public CompProperties_Rottable(float daysToRotStart)
		{
			this.daysToRotStart = daysToRotStart;
		}

		
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in this.ConfigErrors(parentDef))
			{
				
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal && parentDef.tickerType != TickerType.Rare)
			{
				yield return string.Concat(new object[]
				{
					"CompRottable needs tickerType ",
					TickerType.Rare,
					" or ",
					TickerType.Normal,
					", has ",
					parentDef.tickerType
				});
			}
			yield break;
			yield break;
		}

		
		public float daysToRotStart = 2f;

		
		public bool rotDestroys;

		
		public float rotDamagePerDay = 40f;

		
		public float daysToDessicated = 999f;

		
		public float dessicatedDamagePerDay;

		
		public bool disableIfHatcher;
	}
}
