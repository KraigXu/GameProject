using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class RoomRequirement_ThingCount : RoomRequirement_Thing
	{
		
		public override bool Met(Room r, Pawn p = null)
		{
			return this.Count(r) >= this.count;
		}

		
		public int Count(Room r)
		{
			return r.ThingCount(this.thingDef);
		}

		
		public override string Label(Room r = null)
		{
			bool flag = !this.labelKey.NullOrEmpty();
			string text = flag ? this.labelKey.Translate() : this.thingDef.label;
			if (r != null)
			{
				return string.Concat(new object[]
				{
					text,
					" ",
					this.Count(r),
					"/",
					this.count
				});
			}
			if (!flag)
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
			return text + " x" + this.count;
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			{
				
			}
			IEnumerator<string> enumerator = null;
			if (this.count <= 0)
			{
				yield return "count must be larger than 0";
			}
			yield break;
			yield break;
		}

		
		public int count;
	}
}
