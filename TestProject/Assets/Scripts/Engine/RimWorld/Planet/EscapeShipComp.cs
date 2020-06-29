using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	
	[StaticConstructorOnStartup]
	public class EscapeShipComp : WorldObjectComp
	{
		
		public override void PostMapGenerate()
		{
			Building building = ((MapParent)this.parent).Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.Ship_Reactor).FirstOrDefault<Building>();
			Building_ShipReactor building_ShipReactor;
			if (building != null && (building_ShipReactor = (building as Building_ShipReactor)) != null)
			{
				building_ShipReactor.charlonsReactor = true;
			}
		}

		
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
