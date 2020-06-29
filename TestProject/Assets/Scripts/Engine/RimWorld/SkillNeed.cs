using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class SkillNeed
	{
		
		public virtual float ValueFor(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		
		public SkillDef skill;
	}
}
