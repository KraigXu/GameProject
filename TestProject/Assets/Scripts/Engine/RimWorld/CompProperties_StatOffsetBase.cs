using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_StatOffsetBase : CompProperties
	{
		
		public virtual IEnumerable<string> GetExplanationAbstract(ThingDef def)
		{
			yield break;
		}

		
		public virtual float GetMaxOffset(bool forAbstract = false)
		{
			float num = 0f;
			for (int i = 0; i < this.offsets.Count; i++)
			{
				num += this.offsets[i].MaxOffset(forAbstract);
			}
			return num;
		}

		
		public StatDef statDef;

		
		public List<FocusStrengthOffset> offsets = new List<FocusStrengthOffset>();
	}
}
