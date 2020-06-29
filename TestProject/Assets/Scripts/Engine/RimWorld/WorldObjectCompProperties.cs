using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class WorldObjectCompProperties
	{
		
		public virtual IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has WorldObjectCompProperties with null compClass.";
			}
			yield break;
		}

		
		public virtual void ResolveReferences(WorldObjectDef parentDef)
		{
		}

		
		[TranslationHandle]
		public Type compClass = typeof(WorldObjectComp);
	}
}
