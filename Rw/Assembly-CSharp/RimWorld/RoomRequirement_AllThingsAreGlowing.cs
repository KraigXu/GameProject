using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001045 RID: 4165
	public class RoomRequirement_AllThingsAreGlowing : RoomRequirement
	{
		// Token: 0x0600638A RID: 25482 RVA: 0x00228A74 File Offset: 0x00226C74
		public override bool Met(Room r, Pawn p = null)
		{
			using (IEnumerator<Thing> enumerator = r.ContainedThings(this.thingDef).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.TryGetComp<CompGlower>().Glows)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600638B RID: 25483 RVA: 0x00228AD4 File Offset: 0x00226CD4
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.thingDef == null)
			{
				yield return "thingDef is null";
				yield break;
			}
			if (this.thingDef.GetCompProperties<CompProperties_Glower>() == null)
			{
				yield return "No comp glower on thingDef";
			}
			yield break;
		}

		// Token: 0x04003C94 RID: 15508
		public ThingDef thingDef;
	}
}
