using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001041 RID: 4161
	public class RoomRequirement_Thing : RoomRequirement
	{
		// Token: 0x06006372 RID: 25458 RVA: 0x002286D4 File Offset: 0x002268D4
		public override bool Met(Room r, Pawn p = null)
		{
			return r.ContainsThing(this.thingDef);
		}

		// Token: 0x06006373 RID: 25459 RVA: 0x002286E4 File Offset: 0x002268E4
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_Thing roomRequirement_Thing = (RoomRequirement_Thing)other;
			return this.thingDef == roomRequirement_Thing.thingDef;
		}

		// Token: 0x06006374 RID: 25460 RVA: 0x00228711 File Offset: 0x00226911
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey.Translate() : this.thingDef.label) + ((r != null) ? " 0/1" : "");
		}

		// Token: 0x06006375 RID: 25461 RVA: 0x00228751 File Offset: 0x00226951
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.thingDef == null)
			{
				yield return "thingDef is null";
			}
			yield break;
		}

		// Token: 0x06006376 RID: 25462 RVA: 0x00228761 File Offset: 0x00226961
		public override bool PlayerHasResearched()
		{
			return this.thingDef.IsResearchFinished;
		}

		// Token: 0x04003C90 RID: 15504
		public ThingDef thingDef;
	}
}
