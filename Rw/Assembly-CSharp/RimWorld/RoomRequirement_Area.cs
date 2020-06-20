using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103F RID: 4159
	public class RoomRequirement_Area : RoomRequirement
	{
		// Token: 0x06006368 RID: 25448 RVA: 0x002284FC File Offset: 0x002266FC
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey : "RoomRequirementArea").Translate(((r != null) ? (r.CellCount + "/") : "") + this.area);
		}

		// Token: 0x06006369 RID: 25449 RVA: 0x00228561 File Offset: 0x00226761
		public override bool Met(Room r, Pawn p = null)
		{
			return r.CellCount >= this.area;
		}

		// Token: 0x0600636A RID: 25450 RVA: 0x00228574 File Offset: 0x00226774
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.area <= 0)
			{
				yield return "area must be larger than 0";
			}
			yield break;
			yield break;
		}

		// Token: 0x04003C8E RID: 15502
		public int area;
	}
}
