using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001068 RID: 4200
	[StaticConstructorOnStartup]
	public class PlaceWorker_FuelingPort : PlaceWorker
	{
		// Token: 0x060063F1 RID: 25585 RVA: 0x0022A2CC File Offset: 0x002284CC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (def.building == null || !def.building.hasFuelingPort)
			{
				return;
			}
			if (!FuelingPortUtility.GetFuelingPortCell(center, rot).Standable(currentMap))
			{
				return;
			}
			PlaceWorker_FuelingPort.DrawFuelingPortCell(center, rot);
		}

		// Token: 0x060063F2 RID: 25586 RVA: 0x0022A30C File Offset: 0x0022850C
		public static void DrawFuelingPortCell(IntVec3 center, Rot4 rot)
		{
			Vector3 position = FuelingPortUtility.GetFuelingPortCell(center, rot).ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, PlaceWorker_FuelingPort.FuelingPortCellMaterial, 0);
		}

		// Token: 0x04003CDD RID: 15581
		private static readonly Material FuelingPortCellMaterial = MaterialPool.MatFrom("UI/Overlays/FuelingPort", ShaderDatabase.Transparent);
	}
}
