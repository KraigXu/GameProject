using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001043 RID: 4163
	public class RoomRequirement_ThingAnyOf : RoomRequirement
	{
		// Token: 0x0600637E RID: 25470 RVA: 0x0022885C File Offset: 0x00226A5C
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey.Translate() : this.things[0].label) + ((r != null) ? " 0/1" : "");
		}

		// Token: 0x0600637F RID: 25471 RVA: 0x002288B0 File Offset: 0x00226AB0
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

		// Token: 0x06006380 RID: 25472 RVA: 0x0022890C File Offset: 0x00226B0C
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

		// Token: 0x06006381 RID: 25473 RVA: 0x00228980 File Offset: 0x00226B80
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
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

		// Token: 0x06006382 RID: 25474 RVA: 0x00228990 File Offset: 0x00226B90
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

		// Token: 0x04003C92 RID: 15506
		public List<ThingDef> things;
	}
}
