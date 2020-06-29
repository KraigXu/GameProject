using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class CompProperties
	{
		
		public CompProperties()
		{
		}

		
		public CompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		
		public virtual void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
		}

		
		public virtual IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has CompProperties with null compClass.";
			}
			yield break;
		}

		
		public virtual void ResolveReferences(ThingDef parentDef)
		{
		}

		
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			yield break;
		}

		
		public virtual void PostLoadSpecial(ThingDef parent)
		{
		}

		
		[TranslationHandle]
		public Type compClass = typeof(ThingComp);
	}
}
