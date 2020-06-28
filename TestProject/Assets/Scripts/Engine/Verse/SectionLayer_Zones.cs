using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200019F RID: 415
	internal class SectionLayer_Zones : SectionLayer
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x000428F8 File Offset: 0x00040AF8
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawWorldOverlays;
			}
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x000428FF File Offset: 0x00040AFF
		public SectionLayer_Zones(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Zone;
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00042913 File Offset: 0x00040B13
		public override void DrawLayer()
		{
			if (OverlayDrawHandler.ShouldDrawZones)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00042924 File Offset: 0x00040B24
		public override void Regenerate()
		{
			float y = AltitudeLayer.Zone.AltitudeFor();
			ZoneManager zoneManager = base.Map.zoneManager;
			CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
			cellRect.ClipInsideMap(base.Map);
			base.ClearSubMeshes(MeshParts.All);
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					Zone zone = zoneManager.ZoneAt(new IntVec3(i, 0, j));
					if (zone != null && !zone.hidden)
					{
						LayerSubMesh subMesh = base.GetSubMesh(zone.Material);
						int count = subMesh.verts.Count;
						subMesh.verts.Add(new Vector3((float)i, y, (float)j));
						subMesh.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
						subMesh.tris.Add(count);
						subMesh.tris.Add(count + 1);
						subMesh.tris.Add(count + 2);
						subMesh.tris.Add(count);
						subMesh.tris.Add(count + 2);
						subMesh.tris.Add(count + 3);
					}
				}
			}
			base.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
		}
	}
}
