using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class RoomRequirement_Thing : RoomRequirement
	{
		
		public override bool Met(Room r, Pawn p = null)
		{
			return r.ContainsThing(this.thingDef);
		}

		
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_Thing roomRequirement_Thing = (RoomRequirement_Thing)other;
			return this.thingDef == roomRequirement_Thing.thingDef;
		}

		
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey.Translate() : this.thingDef.label) + ((r != null) ? " 0/1" : "");
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.thingDef == null)
			{
				yield return "thingDef is null";
			}
			yield break;
		}

		
		public override bool PlayerHasResearched()
		{
			return this.thingDef.IsResearchFinished;
		}

		
		public ThingDef thingDef;
	}
}
