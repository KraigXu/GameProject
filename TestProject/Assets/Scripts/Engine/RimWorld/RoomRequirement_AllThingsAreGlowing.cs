using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class RoomRequirement_AllThingsAreGlowing : RoomRequirement
	{
		
		public override bool Met(Room r, Pawn p = null)
		{
			IEnumerator<Thing> enumerator = r.ContainedThings(this.thingDef).GetEnumerator();
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

		
		public ThingDef thingDef;
	}
}
