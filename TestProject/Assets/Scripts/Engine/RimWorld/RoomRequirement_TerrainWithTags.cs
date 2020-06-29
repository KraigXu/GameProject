using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class RoomRequirement_TerrainWithTags : RoomRequirement
	{
		
		public override bool Met(Room r, Pawn p = null)
		{
			foreach (IntVec3 c in r.Cells)
			{
				List<string> list = c.GetTerrain(r.Map).tags;
				if (list.NullOrEmpty<string>())
				{
					return false;
				}
				bool flag = false;
				foreach (string item in list)
				{
					if (this.tags.Contains(item))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_TerrainWithTags roomRequirement_TerrainWithTags = (RoomRequirement_TerrainWithTags)other;
			foreach (string item in this.tags)
			{
				if (!roomRequirement_TerrainWithTags.tags.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			{
				
			}
			IEnumerator<string> enumerator = null;
			if (string.IsNullOrEmpty(this.labelKey))
			{
				yield return "does not define a label key";
			}
			if (this.tags.NullOrEmpty<string>())
			{
				yield return "tags are null or empty";
			}
			yield break;
			yield break;
		}

		
		public List<string> tags;
	}
}
