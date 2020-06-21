using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001279 RID: 4729
	[StaticConstructorOnStartup]
	public class EscapeShipComp : WorldObjectComp
	{
		// Token: 0x06006EE2 RID: 28386 RVA: 0x0026A82C File Offset: 0x00268A2C
		public override void PostMapGenerate()
		{
			Building building = ((MapParent)this.parent).Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.Ship_Reactor).FirstOrDefault<Building>();
			Building_ShipReactor building_ShipReactor;
			if (building != null && (building_ShipReactor = (building as Building_ShipReactor)) != null)
			{
				building_ShipReactor.charlonsReactor = true;
			}
		}

		// Token: 0x06006EE3 RID: 28387 RVA: 0x0026A872 File Offset: 0x00268A72
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in CaravanArrivalAction_VisitEscapeShip.GetFloatMenuOptions(caravan, (MapParent)this.parent))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}
	}
}
