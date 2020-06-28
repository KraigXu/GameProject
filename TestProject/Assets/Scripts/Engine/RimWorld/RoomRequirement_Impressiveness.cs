using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001044 RID: 4164
	public class RoomRequirement_Impressiveness : RoomRequirement
	{
		// Token: 0x06006385 RID: 25477 RVA: 0x002289CC File Offset: 0x00226BCC
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey : "RoomRequirementImpressiveness").Translate() + " " + ((r != null) ? (Mathf.Round(r.GetStat(RoomStatDefOf.Impressiveness)) + "/") : "") + this.impressiveness;
		}

		// Token: 0x06006386 RID: 25478 RVA: 0x00228A45 File Offset: 0x00226C45
		public override bool Met(Room r, Pawn p = null)
		{
			return Mathf.Round(r.GetStat(RoomStatDefOf.Impressiveness)) >= (float)this.impressiveness;
		}

		// Token: 0x06006387 RID: 25479 RVA: 0x00228A63 File Offset: 0x00226C63
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.impressiveness <= 0)
			{
				yield return "impressiveness must be larger than 0";
			}
			yield break;
			yield break;
		}

		// Token: 0x04003C93 RID: 15507
		public int impressiveness;
	}
}
