using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_MoteEmitter : CompProperties
	{
		
		public CompProperties_MoteEmitter()
		{
			this.compClass = typeof(CompMoteEmitter);
		}

		
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.mote == null)
			{
				yield return "CompMoteEmitter must have a mote assigned.";
			}
			yield break;
		}

		
		public ThingDef mote;

		
		public Vector3 offset;

		
		public int emissionInterval = -1;

		
		public bool maintain;

		
		public string saveKeysPrefix;
	}
}
