using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class HediffCompProperties
	{
		
		public virtual void PostLoad()
		{
		}

		
		public virtual IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return "compClass is null";
			}
			int num;
			for (int i = 0; i < parentDef.comps.Count; i = num + 1)
			{
				if (parentDef.comps[i] != this && parentDef.comps[i].compClass == this.compClass)
				{
					yield return "two comps with same compClass: " + this.compClass;
				}
				num = i;
			}
			yield break;
		}

		
		[TranslationHandle]
		public Type compClass;
	}
}
