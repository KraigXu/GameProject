using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A85 RID: 2693
	[StaticConstructorOnStartup]
	public static class PowerNetGraphics
	{
		// Token: 0x06003FAB RID: 16299 RVA: 0x00152B18 File Offset: 0x00150D18
		public static void PrintWirePieceConnecting(SectionLayer layer, Thing A, Thing B, bool forPowerOverlay)
		{
			Material mat = PowerNetGraphics.WireMat;
			float y = AltitudeLayer.SmallWire.AltitudeFor();
			if (forPowerOverlay)
			{
				mat = PowerOverlayMats.MatConnectorLine;
				y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			}
			Vector3 center = (A.TrueCenter() + B.TrueCenter()) / 2f;
			center.y = y;
			Vector3 v = B.TrueCenter() - A.TrueCenter();
			Vector2 size = new Vector2(1f, v.MagnitudeHorizontal());
			float rot = v.AngleFlat();
			Printer_Plane.PrintPlane(layer, center, size, mat, rot, false, null, null, 0.01f, 0f);
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x00152BAC File Offset: 0x00150DAC
		public static void RenderAnticipatedWirePieceConnecting(IntVec3 userPos, Rot4 rotation, IntVec2 thingSize, Thing transmitter)
		{
			Vector3 vector = GenThing.TrueCenter(userPos, rotation, thingSize, AltitudeLayer.MapDataOverlay.AltitudeFor());
			if (userPos != transmitter.Position)
			{
				Vector3 vector2 = transmitter.TrueCenter();
				vector2.y = AltitudeLayer.MapDataOverlay.AltitudeFor();
				Vector3 pos = (vector + vector2) / 2f;
				Vector3 v = vector2 - vector;
				Vector3 s = new Vector3(1f, 1f, v.MagnitudeHorizontal());
				Quaternion q = Quaternion.LookRotation(vector2 - vector);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(pos, q, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, PowerOverlayMats.MatConnectorAnticipated, 0);
			}
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x00152C54 File Offset: 0x00150E54
		public static void PrintOverlayConnectorBaseFor(SectionLayer layer, Thing t)
		{
			Vector3 center = t.TrueCenter();
			center.y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			Printer_Plane.PrintPlane(layer, center, new Vector2(1f, 1f), PowerOverlayMats.MatConnectorBase, 0f, false, null, null, 0.01f, 0f);
		}

		// Token: 0x04002517 RID: 9495
		private const AltitudeLayer WireAltitude = AltitudeLayer.SmallWire;

		// Token: 0x04002518 RID: 9496
		private static readonly Material WireMat = MaterialPool.MatFrom("Things/Special/Power/Wire");
	}
}
