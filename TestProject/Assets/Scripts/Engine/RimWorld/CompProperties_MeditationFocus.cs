using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_MeditationFocus : CompProperties_StatOffsetBase
	{
		
		public CompProperties_MeditationFocus()
		{
			this.compClass = typeof(CompMeditationFocus);
		}

		
		public override IEnumerable<string> GetExplanationAbstract(ThingDef def)
		{
			int num;
			for (int i = 0; i < this.offsets.Count; i = num + 1)
			{
				string explanationAbstract = this.offsets[i].GetExplanationAbstract(def);
				if (!explanationAbstract.NullOrEmpty())
				{
					yield return explanationAbstract;
				}
				num = i;
			}
			yield break;
		}

		
		public List<MeditationFocusDef> focusTypes = new List<MeditationFocusDef>();
	}
}
