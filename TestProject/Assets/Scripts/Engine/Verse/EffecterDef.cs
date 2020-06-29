using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class EffecterDef : Def
	{
		
		public Effecter Spawn()
		{
			return new Effecter(this);
		}

		
		public List<SubEffecterDef> children;

		
		public float positionRadius;

		
		public FloatRange offsetTowardsTarget;
	}
}
