using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106B RID: 4203
	public class PlaceWorker_TurretTop : PlaceWorker
	{
		// Token: 0x060063FE RID: 25598 RVA: 0x0022A71C File Offset: 0x0022891C
		public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(GraphicDatabase.Get<Graphic_Single>(def.building.turretGunDef.graphicData.texPath, ShaderDatabase.Cutout, new Vector2(def.building.turretTopDrawSize, def.building.turretTopDrawSize), Color.white), def, ghostCol).DrawFromDef(GenThing.TrueCenter(loc, rot, def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), rot, def, (float)TurretTop.ArtworkRotation);
		}
	}
}
