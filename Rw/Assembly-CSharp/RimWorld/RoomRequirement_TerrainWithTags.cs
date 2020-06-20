using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001040 RID: 4160
	public class RoomRequirement_TerrainWithTags : RoomRequirement
	{
		// Token: 0x0600636D RID: 25453 RVA: 0x00228594 File Offset: 0x00226794
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

		// Token: 0x0600636E RID: 25454 RVA: 0x00228650 File Offset: 0x00226850
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

		// Token: 0x0600636F RID: 25455 RVA: 0x002286C4 File Offset: 0x002268C4
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
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

		// Token: 0x04003C8F RID: 15503
		public List<string> tags;
	}
}
