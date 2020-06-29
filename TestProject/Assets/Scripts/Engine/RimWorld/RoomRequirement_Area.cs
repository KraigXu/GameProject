using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class RoomRequirement_Area : RoomRequirement
	{
		
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey : "RoomRequirementArea").Translate(((r != null) ? (r.CellCount + "/") : "") + this.area);
		}

		
		public override bool Met(Room r, Pawn p = null)
		{
			return r.CellCount >= this.area;
		}

		
		public override IEnumerable<string> ConfigErrors()
		{


			IEnumerator<string> enumerator = null;
			if (this.area <= 0)
			{
				yield return "area must be larger than 0";
			}
			yield break;
			yield break;
		}

		
		public int area;
	}
}
