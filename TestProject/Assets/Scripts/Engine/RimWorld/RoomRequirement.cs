using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public abstract class RoomRequirement
	{
		
		public abstract bool Met(Room r, Pawn p = null);

		
		public virtual string Label(Room r = null)
		{
			return this.labelKey.Translate();
		}

		
		public string LabelCap(Room r = null)
		{
			return this.Label(r).CapitalizeFirst();
		}

		
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		
		public virtual bool SameOrSubsetOf(RoomRequirement other)
		{
			return base.GetType() == other.GetType();
		}

		
		public virtual bool PlayerHasResearched()
		{
			return true;
		}

		
		[NoTranslate]
		public string labelKey;
	}
}
