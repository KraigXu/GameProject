using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001007 RID: 4103
	public class StatPart_Outdoors : StatPart
	{
		// Token: 0x0600623C RID: 25148 RVA: 0x00220F2E File Offset: 0x0021F12E
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.OutdoorsFactor(req);
		}

		// Token: 0x0600623D RID: 25149 RVA: 0x00220F3C File Offset: 0x0021F13C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing.GetRoom(RegionType.Set_All) != null)
			{
				string str;
				if (this.ConsideredOutdoors(req))
				{
					str = "Outdoors".Translate();
				}
				else
				{
					str = "Indoors".Translate();
				}
				return str + ": x" + this.OutdoorsFactor(req).ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600623E RID: 25150 RVA: 0x00220FA5 File Offset: 0x0021F1A5
		private float OutdoorsFactor(StatRequest req)
		{
			if (this.ConsideredOutdoors(req))
			{
				return this.factorOutdoors;
			}
			return this.factorIndoors;
		}

		// Token: 0x0600623F RID: 25151 RVA: 0x00220FC0 File Offset: 0x0021F1C0
		private bool ConsideredOutdoors(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					return room.OutdoorsForWork || (req.HasThing && req.Thing.Spawned && !req.Thing.Map.roofGrid.Roofed(req.Thing.Position));
				}
			}
			return false;
		}

		// Token: 0x04003BE2 RID: 15330
		private float factorIndoors = 1f;

		// Token: 0x04003BE3 RID: 15331
		private float factorOutdoors = 1f;
	}
}
