using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200083A RID: 2106
	public class ThoughtWorker_IsIndoorsForUndergrounder : ThoughtWorker
	{
		// Token: 0x06003483 RID: 13443 RVA: 0x001200CC File Offset: 0x0011E2CC
		public static bool IsAwakeAndIndoors(Pawn p, out bool isNaturalRoof)
		{
			isNaturalRoof = false;
			if (!p.Awake())
			{
				return false;
			}
			if (p.Position.UsesOutdoorTemperature(p.Map))
			{
				return false;
			}
			RoofDef roofDef = p.Map.roofGrid.RoofAt(p.Position);
			if (roofDef == null)
			{
				return false;
			}
			isNaturalRoof = roofDef.isNatural;
			return true;
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x00120120 File Offset: 0x0011E320
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			bool flag;
			return ThoughtWorker_IsIndoorsForUndergrounder.IsAwakeAndIndoors(p, out flag) && !flag;
		}
	}
}
