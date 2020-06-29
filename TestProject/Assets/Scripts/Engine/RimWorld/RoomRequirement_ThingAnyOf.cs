using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class RoomRequirement_ThingAnyOf : RoomRequirement
	{
		
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey.Translate() : this.things[0].label) + ((r != null) ? " 0/1" : "");
		}

		
		public override bool Met(Room r, Pawn p = null)
		{
			foreach (ThingDef def in this.things)
			{
				if (r.ContainsThing(def))
				{
					return true;
				}
			}
			return false;
		}

		
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_ThingAnyOf roomRequirement_ThingAnyOf = (RoomRequirement_ThingAnyOf)other;
			foreach (ThingDef item in this.things)
			{
				if (!roomRequirement_ThingAnyOf.things.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.things.NullOrEmpty<ThingDef>())
			{
				yield return "things are null or empty";
			}
			yield break;
			yield break;
		}

		
		public override bool PlayerHasResearched()
		{
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i].IsResearchFinished)
				{
					return true;
				}
			}
			return false;
		}

		
		public List<ThingDef> things;
	}
}
